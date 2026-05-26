using AutoMapper;
using Common.Dto;
using Common.Exceptions;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service.Services
{
	public class FreelancerService(IRepository<Freelancer> repository, IMapper mapper, IUserRepository userRepository, IRepository<Category> categoryRepository) : IFreelancerService
	{
		private readonly IRepository<Freelancer> repository = repository;
		private readonly IMapper mapper = mapper;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IRepository<Category> categoryRepository = categoryRepository;


		public async Task<List<FreelancerDto>> GetAll()
		{
			var freelancers = await repository.GetAll();
			return mapper.Map<List<FreelancerDto>>(freelancers);
		}

		public async Task<FreelancerDto> GetById(int id)
		{
			var freelancer = await repository.GetById(id) ?? throw new NotFoundException("Freelancer not found");
			return mapper.Map<FreelancerDto>(freelancer);
		}


		public async Task<UserDto> BecomeFreelancer(int userId, BecomeFreelancerDto freelancerDto)
		{
			var user = await userRepository.GetById(userId) ?? throw new NotFoundException("User not found");

			if (user.FreelancerProfile != null)
				throw new BadRequestException("User is already a freelancer");

			var freelancer = mapper.Map<Freelancer>(freelancerDto);
			freelancer.UserId = userId;

			if (freelancerDto.SkillIds != null)
			{
				freelancer.Skills = [];
				foreach (var id in freelancerDto.SkillIds)
				{
					var category = await categoryRepository.GetById(id) ?? throw new BadRequestException($"Category with id {id} not found");
					freelancer.Skills.Add(category);
				}
			}

			if (freelancerDto.SpecializationIds != null)
			{
				freelancer.Specializations = [];
				foreach (var id in freelancerDto.SpecializationIds)
				{
					var category = await categoryRepository.GetById(id) ?? throw new BadRequestException($"Category with id {id} not found");
					freelancer.Specializations.Add(category);
				}
			}

			var createdFreelancer = await repository.AddItem(freelancer);

			if (freelancerDto.ImageFile != null)
			{
				var fileName = Guid.NewGuid() + Path.GetExtension(freelancerDto.ImageFile.FileName);
				var imagesPath = Path.Combine(Environment.CurrentDirectory, "Images");
				Directory.CreateDirectory(imagesPath);
				var path = Path.Combine(imagesPath, fileName);

				using var fs = new FileStream(path, FileMode.Create);
				await freelancerDto.ImageFile.CopyToAsync(fs);

				createdFreelancer.Image = fileName;
				await repository.UpdateItem(createdFreelancer.FreelancerId, createdFreelancer);
			}

			user.FreelancerProfile = createdFreelancer;
			await userRepository.UpdateItem(userId, user);

			return new UserDto
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email,
				FreelancerId = createdFreelancer.FreelancerId
			};
		}

		public async Task<FreelancerDto> UpdateItem(int id, FreelancerDto freelancer)
		{
			_ = await repository.GetById(id) ?? throw new NotFoundException("Freelancer not found");
			var updated = await repository.UpdateItem(id, mapper.Map<Freelancer>(freelancer));
			return mapper.Map<FreelancerDto>(updated);
		}

		public async Task DeductHoursAfterJobAccepted(int freelancerId, int jobRequiredHours)
		{
			var freelancer = await repository.GetById(freelancerId) ?? throw new NotFoundException("Freelancer not found");
			freelancer.AvailableHours = Math.Max(0, freelancer.AvailableHours - jobRequiredHours);
			await repository.UpdateItem(freelancer.FreelancerId, freelancer);
		}
	}
}
