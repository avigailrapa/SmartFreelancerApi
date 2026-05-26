using AutoMapper;
using Common.Dto;
using Common.Enums;
using Common.Exceptions;
using Repository.Entities;
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
		private const double DeadlineUrgentDays = 3.0;
		private const double DeadlineNearDays = 7.0;
		private const double DeadlineComfortableDays = 14.0;
		private const double UrgentBonus = 1.5;
		private const double NearBonus = 1.25;
		private const double ComfortableBonus = 1.1;


		private static bool IsDeadlineFeasible(Job job, Freelancer freelancer)
		{
			double totalDays = (freelancer.AvailableUntil - DateTime.UtcNow).TotalDays;
			if (totalDays <= 0) return false;

			DateTime effectiveDeadline;

			if (job.Deadline == default)
				effectiveDeadline = freelancer.AvailableUntil;
			else if (job.Deadline < freelancer.AvailableUntil)
				effectiveDeadline = job.Deadline;
			else
				effectiveDeadline = freelancer.AvailableUntil;

			double daysAvailable = (effectiveDeadline - DateTime.UtcNow).TotalDays;
			if (daysAvailable <= 0) return false;

			double hoursPerDay = (double)freelancer.AvailableHours / totalDays;
			double reachableHours = daysAvailable * hoursPerDay;

			return reachableHours >= job.RequiredHours;
		}


		private static double GetDeadlineMultiplier(Job job)
		{
			if (job.Deadline == default) return 1.0;

			double daysLeft = (job.Deadline - DateTime.UtcNow).TotalDays;

			return daysLeft switch
			{
				<= DeadlineUrgentDays => UrgentBonus,
				<= DeadlineNearDays => NearBonus,
				<= DeadlineComfortableDays => ComfortableBonus,
				_ => 1.0
			};
		}


		private static (bool isMatch, double skillScore) IsSkillMatch(
		   Job job,
		   HashSet<int> freelancerSkillIds,
		   HashSet<int> freelancerSpecialtyIds)
		{
			var jobRequiredSkills = job.RequiredSkills
				.Where(s => s.Type == CategoryType.Skill)
				.ToList();

			var jobSpecialty = job.RequiredSkills
				.FirstOrDefault(s => s.Type == CategoryType.Specialty);

			if (jobRequiredSkills.Count == 0 && jobSpecialty == null)
				return (true, 1.0);

			int matchedSkills = jobRequiredSkills
				.Count(s => freelancerSkillIds.Contains(s.CategoryId));

			int matchedSpecialties = jobSpecialty != null &&
				freelancerSpecialtyIds.Contains(jobSpecialty.CategoryId) ? 1 : 0;

			double score = (matchedSkills + matchedSpecialties * 2.0)
						 / (jobRequiredSkills.Count + (jobSpecialty != null ? 1 : 0));

			return (score >= MinimumMatchThreshold, score);
		}

		private async Task<List<(JobDto job, decimal value)>> GetJobsMatchingFreelancerSkills(Freelancer freelancer)
		{
			var openJobs = await jobRepository.GetOpenJobs();
			var filteredJobs = openJobs.Where(job => job.MainCategoryId == freelancer.MainCategoryId);

			var freelancerSkillIds = freelancer.Skills
				.Select(s => s.CategoryId)
				.ToHashSet();

			var freelancerSpecialtyIds = freelancer.Specializations
				.Select(s => s.CategoryId)
				.ToHashSet();

			var matchingJobs = new List<(JobDto job, decimal value)>();

			foreach (var job in filteredJobs)
			{
				if (!IsDeadlineFeasible(job, freelancer)) continue;

				var (isMatch, skillsScore) = IsSkillMatch(job, freelancerSkillIds, freelancerSpecialtyIds);
				if (!isMatch) continue;

				decimal baseValue = job.RequiredHours * job.MaxPayPerHour;
				double deadlineMultiplier = GetDeadlineMultiplier(job);
				decimal weightedValue = baseValue * (decimal)skillsScore * (decimal)deadlineMultiplier;

				matchingJobs.Add((mapper.Map<JobDto>(job), weightedValue));
			}

			return [.. matchingJobs.OrderByDescending(j => j.value)];
		}


		public async Task<List<JobDto>> GetOptimalJobsForFreelancer(int freelancerId)
		{
			var freelancer = await freelancerRepository.GetById(freelancerId) ?? throw new NotFoundException("Freelancer not found");

			var availableHours = freelancer.AvailableHours;
			if (availableHours <= 0) return [];

			List<(JobDto job, decimal value)> matchingJobs = await GetJobsMatchingFreelancerSkills(freelancer);
			if (matchingJobs.Count == 0) return [];

			matchingJobs = [.. matchingJobs.Where(j => j.job.RequiredHours > 0 && j.job.RequiredHours <= availableHours)];

			int n = matchingJobs.Count;
			decimal[,] dp = new decimal[n + 1, availableHours + 1];

			for (int i = 1; i <= n; i++)
			{
				var (job, value) = matchingJobs[i - 1];
				int hours = job.RequiredHours;

				for (int h = 0; h <= availableHours; h++)
				{
					if (hours > h)
						dp[i, h] = dp[i - 1, h];
					else
						dp[i, h] = Math.Max(dp[i - 1, h], dp[i - 1, h - hours] + value);
				}
			}

			return ReconstructOptimalJobs(dp, matchingJobs, availableHours);
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