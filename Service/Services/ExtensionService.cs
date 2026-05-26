using Common.Dto;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Service.Interfaces;

namespace Service.Services
{
	public static class ExtensionService
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddRepository();

			services.AddScoped<IService<CategoryDto>, CategoryService>();
			services.AddScoped<IRatingService, RatingService>();

			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IFreelancerService, FreelancerService>();
			services.AddScoped<IJobService, JobService>();

			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IMatchingService, MatchingService>();
			services.AddScoped<IProposalService, ProposalService>();

			return services;
		}
	}
}

