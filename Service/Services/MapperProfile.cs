using AutoMapper;
using Common.Dto;
using Common.Enums;
using Repository.Entities;

namespace Service.Services;

public class MapperProfile : Profile
{
	public MapperProfile()
	{
		CreateMap<BecomeFreelancerDto, Freelancer>();

		CreateMap<Proposal, ProposalDto>()
			.ForMember(dest => dest.FreelancerName, opt => opt.MapFrom(src =>
				src.Freelancer != null && src.Freelancer.User != null ? src.Freelancer.User.FullName : "Unknown"))
			.ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src =>
				src.Job != null ? src.Job.Title : "No Title"))
			.ForMember(dest => dest.ClientId, opt => opt.MapFrom(src =>
				src.ClientId))
			.ForMember(dest => dest.ClientName, opt => opt.MapFrom(src =>
				src.Job != null && src.Job.Client != null ? src.Job.Client.FullName : (src.ClientName ?? "Unknown")));

		CreateMap<User, UserDto>()
			.ForMember(dest => dest.FreelancerId, opt => opt.MapFrom(src =>
				src.FreelancerProfile != null ? src.FreelancerProfile.FreelancerId : (int?)null))
			.ReverseMap();

		CreateMap<Freelancer, FreelancerDto>()
			.ForMember(dest => dest.ArrImage, opt => opt.Ignore())
			.ForMember(dest => dest.ImageFile, opt => opt.Ignore())
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
				src.User != null ? src.User.FullName : null))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
				src.User != null ? src.User.Email : null))
			.ForMember(dest => dest.MainCategoryName, opt => opt.MapFrom(src =>
				src.MainCategory != null ? src.MainCategory.Name : null))
			.ForMember(dest => dest.SkillIds, opt => opt.MapFrom(src =>
				src.Skills.Select(s => s.CategoryId)))
			.ForMember(dest => dest.SkillNames, opt => opt.MapFrom(src =>
				src.Skills.Select(s => s.Name)))
			.ForMember(dest => dest.SpecializationIds, opt => opt.MapFrom(src =>
				src.Specializations.Select(s => s.CategoryId)))
			.ForMember(dest => dest.SpecializationNames, opt => opt.MapFrom(src =>
				src.Specializations.Select(s => s.Name)))
			.ForMember(dest => dest.AverageStars, opt => opt.MapFrom(src =>
				src.RatingsReceived != null && src.RatingsReceived.Any()
					? src.RatingsReceived.Average(r => r.Stars)
					: (double?)null))
			.ForMember(dest => dest.LatestRating, opt => opt.MapFrom(src =>
				src.RatingsReceived != null
					? src.RatingsReceived.OrderByDescending(r => r.CreatedAt).FirstOrDefault()
					: null))
			.ReverseMap()
			.ForMember(dest => dest.Image, opt => opt.Ignore())
			.ForMember(dest => dest.User, opt => opt.Ignore())
			.ForMember(dest => dest.MainCategory, opt => opt.Ignore())
			.ForMember(dest => dest.Skills, opt => opt.Ignore())
			.ForMember(dest => dest.Specializations, opt => opt.Ignore())
			.ForMember(dest => dest.RatingsReceived, opt => opt.Ignore());

		CreateMap<Job, JobDto>()
		.ForMember(dest => dest.ClientName, opt => opt.MapFrom(src =>
			src.Client.FullName))
		.ForMember(dest => dest.MainCategoryName, opt => opt.MapFrom(src =>
			src.MainCategory.Name))
		.ForMember(dest => dest.AssignedFreelancerName, opt => opt.MapFrom(src =>
			src.AssignedFreelancer != null ? src.AssignedFreelancer.User.FullName : null))
		.ForMember(dest => dest.RequiredSkillIds, opt => opt.MapFrom(src =>
			src.RequiredSkills.Where(s => s.Type == CategoryType.Skill).Select(s => s.CategoryId)))
		.ForMember(dest => dest.RequiredSkillNames, opt => opt.MapFrom(src =>
			src.RequiredSkills.Where(s => s.Type == CategoryType.Skill).Select(s => s.Name)))
		.ForMember(dest => dest.SpecialtyCategoryId, opt => opt.MapFrom(src =>
			src.RequiredSkills.Where(s => s.Type == CategoryType.Specialty)
				.Select(s => s.CategoryId).FirstOrDefault()))
		.ForMember(dest => dest.SpecialtyCategoryName, opt => opt.MapFrom(src =>
			src.RequiredSkills.Where(s => s.Type == CategoryType.Specialty)
				.Select(s => s.Name).FirstOrDefault()));

		CreateMap<Rating, RatingDto>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src =>
				src.User != null ? src.User.FullName : "Unknown"))
			.ReverseMap();

		CreateMap<Category, CategoryDto>()
			.ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src =>
				src.SubCategories))
			.ReverseMap();
	}
}