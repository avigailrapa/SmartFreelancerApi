using System.Text;
using AutoMapper;
using Common.Dto;
using Repository.Entities;

namespace Service.Services
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<User, UserDto>().ReverseMap();

			CreateMap<Freelancer, FreelancerDto>()
				.ForMember(dest => dest.ArrImage,
				opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Image)))
	           .ForMember(dest => dest.ImageFile, opt => opt.Ignore());

			// DTO -> Entity
			CreateMap<FreelancerDto, Freelancer>()
				.ForMember(dest => dest.Image,
						   opt => opt.MapFrom(src => src.ArrImage!= null ? Encoding.UTF8.GetString(src.ArrImage) : null));

			CreateMap<Job, JobDto>().ReverseMap();
			CreateMap<Rating, RatingDto>().ReverseMap();
			CreateMap<Category, CategoryDto>().ReverseMap();
		}
	}
}
