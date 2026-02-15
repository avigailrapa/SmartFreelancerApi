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
            services.AddScoped<IService<RatingDto>, RatingService>();

            services.AddScoped<IUserService<UserDto>, UserService>();
            services.AddScoped<IFreelancerService<FreelancerDto>, FreelancerService>();
            services.AddScoped<IJobService, JobService>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}

