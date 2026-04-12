using AutoMapper;
using Common.Dto;
using Common.Enums;
using Common.Exceptions;
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


        private async Task<List<(JobDto, decimal)>> GetJobsMatchingFreelancerSkills(Freelancer freelancer)
        {
            var openJobs = await jobRepository.GetOpenJobs();
            var filteredJobs = openJobs.Where(job => job.MainCategoryId == freelancer.MainCategoryId && job.Deadline >= DateTime.Now);


            var freelancerSkillIds = freelancer.Skills
                .Select(s => s.CategoryId)
                .ToHashSet();

            var freelancerSpecialtyIds = freelancer.Skills
              .Where(s => s.ParentCategoryId.HasValue)
              .Select(s => s.ParentCategoryId!.Value)
              .ToHashSet();

            foreach (var spec in freelancer.Specializations)
            {
                freelancerSpecialtyIds.Add(spec.CategoryId);
            }


            var matchingJobs = new List<(JobDto Job, decimal Value)>();

            foreach (var job in filteredJobs)
            {
                if (IsSkillMatch(job, freelancerSkillIds))
                {
                    decimal value = job.RequiredHours * job.MaxPayPerHour;
                    matchingJobs.Add((mapper.Map<JobDto>(job), value));
                }
            }
            return [.. matchingJobs.OrderByDescending(j => j.Value)];
        }


        private static bool IsSkillMatch(Job job, HashSet<int> freelancerSkillIds)
        {
            var jobRequiredSkills = job.RequiredSkills
                .Where(s => s.Type == CategoryType.Skill)
                .ToList();

            if (jobRequiredSkills.Count == 0) return true;

            int matchedSkillsCount = jobRequiredSkills.Count(s => freelancerSkillIds.Contains(s.CategoryId));
            double skillsScore = (double)matchedSkillsCount / jobRequiredSkills.Count;

            return skillsScore >= MinimumMatchThreshold;
        }




        public async Task<List<JobDto>> GetOptimalJobsForFreelancer(int freelancerId)
        {
            var freelancer = await freelancerRepository.GetById(freelancerId) ?? throw new NotFoundException("Freelancer not found");

            var availableHours = freelancer.AvailableHours;

            List<(JobDto, decimal)> matchingJobs = await GetJobsMatchingFreelancerSkills(freelancer);
            if (matchingJobs.Count == 0) return [];

            int n = matchingJobs.Count;

            decimal[,] dp = new decimal[n + 1, availableHours + 1];
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

        private static List<JobDto> ReconstructOptimalJobs(decimal[,] dp, List<(JobDto job, decimal value)> matchingJobs, int availableHours)
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
