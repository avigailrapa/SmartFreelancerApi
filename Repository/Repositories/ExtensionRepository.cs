using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
	public static class ExtensionRepository
	{
		public static IServiceCollection AddRepository(this IServiceCollection services)
		{
			services.AddScoped<IRepository<Category>, CategoryRepository>();
			services.AddScoped<IRepository<Freelancer>, FreelancerRepository>();
			services.AddScoped<RatingRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IJobRepository, JobRepository>();
			services.AddScoped<IProposalRepository, ProposalRepository>();
			return services;

		}
	}
}
