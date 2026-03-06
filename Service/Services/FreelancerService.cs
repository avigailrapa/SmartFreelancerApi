using System.Text;
using AutoMapper;
using Common.Dto;
using Common.Exceptions;
using Repository.Entities;
using Repository.interfaces;
using Service.Interfaces;

namespace Service.Services
{
    public class FreelancerService(IRepository<Freelancer> repository, IMapper mapper, IRepository<User> userRepository, IRepository<Category> categoryRepository) : IFreelancerService
    {
        private readonly IRepository<Freelancer> repository = repository;
        private readonly IMapper mapper = mapper;
        private readonly IRepository<User> userRepository = userRepository;
        private readonly IRepository<Category> categoryRepository = categoryRepository;

        public async Task DeleteItem(int id)
        {
            var freelancer = await repository.GetById(id) ?? throw new NotFoundException("Freelancer not found");
            await repository.DeleteItem(id);
        }
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

        public async Task<FreelancerDto> UpdateItem(int id, FreelancerDto freelancer)
        {
            var exists = await repository.GetById(id) ?? throw new NotFoundException("Freelancer not found");
            var updated = await repository.UpdateItem(id, mapper.Map<Freelancer>(freelancer));
            return mapper.Map<FreelancerDto>(updated);
        }

        public async Task<UserDto> BecomeFreelancer(int userId, FreelancerDto freelancerDto)
        {
            var user = await userRepository.GetById(userId) ?? throw new NotFoundException("User not found");

            if (freelancerDto.ImageFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(freelancerDto.ImageFile.FileName);
                var path = Path.Combine(Environment.CurrentDirectory, "Images/", fileName);
                using var fs = new FileStream(path, FileMode.Create);
                await freelancerDto.ImageFile.CopyToAsync(fs);

                freelancerDto.ArrImage = Encoding.UTF8.GetBytes(fileName);
            }
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
            var createdFreelancer = await repository.AddItem(freelancer);

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
    }
}









