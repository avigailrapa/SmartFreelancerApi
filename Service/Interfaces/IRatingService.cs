using Common.Dto;
using Common.Dto.Common.Dto;

namespace Service.Interfaces
{
	public interface IRatingService
	{
		Task<RatingDto> AddRating(RatingCreateDto ratingDto, int userId);

	}
}
