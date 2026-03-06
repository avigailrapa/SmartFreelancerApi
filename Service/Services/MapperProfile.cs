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
            .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.MainCategoryName, opt => opt.MapFrom(src => src.MainCategory != null ? src.MainCategory.Name : null))
            .ForMember(dest => dest.SkillIds, opt => opt.MapFrom(src =>
             src.Skills != null ? src.Skills.Select(s => s.CategoryId).ToList() : new List<int>()));


        CreateMap<FreelancerDto, Freelancer>()
            .ForMember(dest => dest.Image,
                       opt => opt.MapFrom(src => src.ArrImage != null && src.ArrImage.Length > 0 ? Encoding.UTF8.GetString(src.ArrImage) : null))
            .ForMember(dest => dest.Skills, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.MainCategory, opt => opt.Ignore());

        CreateMap<Job, JobDto>().ReverseMap();

        CreateMap<Rating, RatingDto>().ReverseMap();

        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories))
            .ReverseMap();
    }
}