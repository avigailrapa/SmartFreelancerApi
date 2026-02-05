using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Dto;
using Repository.Entities;

namespace Service.Services
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
			CreateMap<User, UserDto>().ReverseMap();
			CreateMap<Freelancer, FreelancerDto>().ReverseMap();
			CreateMap<Job, JobDto>().ReverseMap();
			CreateMap<Rating, RatingDto>().ReverseMap();
			CreateMap<Category, CategoryDto>().ReverseMap();


		}
	}
}
