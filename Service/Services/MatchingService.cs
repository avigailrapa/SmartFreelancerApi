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


        private async Task<List<(JobDto, double)>> GetJobsMatchingFreelancerSkills(int freelancerId)
        {
            var freelancer = await freelancerRepository.GetById(freelancerId) ?? throw new Exception("Freelancer not found");
            var openJobs = await jobRepository.GetOpenJobs();

            var matchingJobs = new List<(JobDto Job, double Value)>();

            foreach (var job in openJobs)
            {
                if (IsSkillMatch(freelancer, job))
                {
                    double value = job.RequiredHours * job.MaxPayPerHour;
                    JobDto jobDto = mapper.Map<JobDto>(job);
                    matchingJobs.Add((jobDto, value));

                }
            }
            return [.. matchingJobs.OrderByDescending(j => j.Value)]; ;
        }

        private static bool IsSkillMatch(Freelancer freelancer, Job job)
        {
            if (job.RequiredSkills == null || job.RequiredSkills.Count == 0) return true;

            var freelancerSkillSet = new HashSet<string>(freelancer.Skills?.Select(s => s.Name) ?? [], StringComparer.OrdinalIgnoreCase);
            int matchedSkills = job.RequiredSkills.Count(skill => freelancerSkillSet.Contains(skill.Name));
            double matchPercentage = (double)matchedSkills / job.RequiredSkills.Count;

            return matchPercentage >= MinimumMatchThreshold;
        }



        public async Task<List<JobDto>> GetOptimalJobsForFreelancer(int freelancerId)
        {
            var freelancer = await freelancerRepository.GetById(freelancerId) ?? throw new Exception("Freelancer not found");
            var availableHours = freelancer.AvailableHours;

            List<(JobDto, double)> matchingJobs = await GetJobsMatchingFreelancerSkills(freelancerId);
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
