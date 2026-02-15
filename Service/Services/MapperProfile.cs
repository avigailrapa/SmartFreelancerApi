using System.Text;
using AutoMapper;
using Common.Dto;
using Repository.Entities;

namespace Service.Services;

public class MapperProfile : Profile
{
    public MapperProfile()
    {

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FreelancerId,
                       opt => opt.MapFrom(src => src.FreelancerProfile != null ? src.FreelancerProfile.FreelancerId : (int?)null))
            .ReverseMap();

        CreateMap<Freelancer, FreelancerDto>()
            .ForMember(dest => dest.ArrImage,
                       opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Image)))
            .ForMember(dest => dest.ImageFile, opt => opt.Ignore());

        CreateMap<FreelancerDto, Freelancer>()
            .ForMember(dest => dest.Image,
                       opt => opt.MapFrom(src => src.ArrImage != null ? Encoding.UTF8.GetString(src.ArrImage) : null));

        CreateMap<Job, JobDto>().ReverseMap();
        CreateMap<Rating, RatingDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}

