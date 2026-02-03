using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.interfaces;

namespace Repository.Repositories
{
	public static class ExtensionRepository
	{
		public static IServiceCollection AddRepository(this IServiceCollection services)
		{
			services.AddScoped<IRepository<Category>, CategoryRepository>();
			services.AddScoped<IRepository<Freelancer>, FreelancerRepository>();
			services.AddScoped<IRepository<Job>, JobRepository>();
			services.AddScoped<IRepository<Rating>, RatingRepository>();
			services.AddScoped<IRepository<User>, UserRepository>();
			return services;

		}
	}
}
