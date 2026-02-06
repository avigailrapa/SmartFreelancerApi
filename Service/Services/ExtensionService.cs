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
			//services.AddScoped<IsExist<BabyDto>, BabyService>();
			services.AddScoped<IService<UserDto>, UserService>();
			//services.AddScoped<IsExist<NurseDto>, NurseService>();
			services.AddScoped<IService<JobDto>, JobService>();
			services.AddScoped<IService<FreelancerDto>, FreelancerService>();
			services.AddScoped<IService<RatingDto>, RatingService>();

			return services;
		}
	}
}
