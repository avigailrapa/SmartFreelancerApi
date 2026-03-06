using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Repositories;

namespace Service.Services
{
    public class RatingService(RatingRepository repository, IMapper mapper)
    {
        private readonly RatingRepository repository = repository;
        private readonly IMapper mapper = mapper;

        public async Task<RatingDto> AddRating(RatingDto ratingDto)
        {
            var rating = new Rating
            {
                FreelancerId = ratingDto.FreelancerId,
                UserId = ratingDto.UserId,
                Stars = ratingDto.Stars,
                Comment = ratingDto.Comment,
                CreatedAt = DateTime.Now
            };

            var added = await repository.AddItem(rating);

            return mapper.Map<RatingDto>(added);
        }


    }
}
