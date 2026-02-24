using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.interfaces;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
    internal class MatchingService(IRepository<Freelancer> freelancerRepository, IMapper mapper, IJobRepository jobRepository) : IMatchingService
    {
        private readonly IRepository<Freelancer> freelancerRepository = freelancerRepository;
        private readonly IMapper mapper = mapper;
        private readonly IJobRepository jobRepository = jobRepository;
        private const double MinimumMatchThreshold = 0.5;


        private async Task<List<(JobDto, double)>> GetJobsMatchingFreelancerSkills(Freelancer freelancer)
        {
            var openJobs = await jobRepository.GetOpenJobs();

            var freelancerMainSkills = freelancer.Skills.Select(s => s.ParentCategoryId ?? s.CategoryId).ToHashSet();

            var freelancerSubSkill = new HashSet<string>(
                freelancer.Skills.Where(s => s.ParentCategoryId != null).Select(s => s.Name),
                StringComparer.OrdinalIgnoreCase
                );


            var filteredJobs = openJobs.Where(job =>
            {
                return job.RequiredSkills
                 .Select(s => s.ParentCategoryId ?? s.CategoryId)
                 .Any(j => freelancerMainSkills.Contains(j));
            });


            var matchingJobs = new List<(JobDto Job, double Value)>();

            foreach (var job in filteredJobs)
            {
                if (IsSkillMatch(job, freelancerSubSkill))
                {
                    double value = job.RequiredHours * job.MaxPayPerHour;
                    JobDto jobDto = mapper.Map<JobDto>(job);
                    matchingJobs.Add((jobDto, value));

                }
            }
            return [.. matchingJobs.OrderByDescending(j => j.Value)]; ;
        }

        private static bool IsSkillMatch(Job job, HashSet<string> freelancerSubSkills)
        {
            var jobSubSkills = job.RequiredSkills.Where(s => s.ParentCategoryId != null).ToList();

            if (jobSubSkills.Count == 0) return true;

            int matchedSkills = jobSubSkills.Count(skill => freelancerSubSkills.Contains(skill.Name));
            double matchPercentage = (double)matchedSkills / jobSubSkills.Count;

            return matchPercentage >= MinimumMatchThreshold;
        }



        public async Task<List<JobDto>> GetOptimalJobsForFreelancer(int freelancerId)
        {
            var freelancer = await freelancerRepository.GetById(freelancerId) ?? throw new Exception("Freelancer not found");
            var availableHours = freelancer.AvailableHours;

            List<(JobDto, double)> matchingJobs = await GetJobsMatchingFreelancerSkills(freelancer);
            if (matchingJobs.Count == 0) return [];

            int n = matchingJobs.Count;

            double[,] dp = new double[n + 1, availableHours + 1];
            for (int i = 1; i <= n; i++)
            {
                var (job, value) = matchingJobs[i - 1];
                int hours = job.RequiredHours;

                for (int h = 0; h <= availableHours; h++)
                {
                    if (hours > h)
                    {
                        dp[i, h] = dp[i - 1, h];
                    }
                    else
                    {
                        dp[i, h] = Math.Max(dp[i - 1, h], dp[i - 1, h - hours] + value);
                    }
                }

            }
            var optimalJobs = ReconstructOptimalJobs(dp, matchingJobs, availableHours);
            return optimalJobs;
        }

        private static List<JobDto> ReconstructOptimalJobs(double[,] dp, List<(JobDto job, double value)> matchingJobs, int availableHours)
        {
            List<JobDto> optimalJobs = [];
            int n = matchingJobs.Count;
            int remainingHours = availableHours;
            int i = n;

            while (i > 0 && remainingHours > 0)
            {
                if (dp[i, remainingHours] != dp[i - 1, remainingHours])
                {
                    var (job, _) = matchingJobs[i - 1];
                    optimalJobs.Add(job);
                    remainingHours -= job.RequiredHours;
                }
                i--;
            }
            optimalJobs.Reverse();
            return optimalJobs;

        }

    }
}
