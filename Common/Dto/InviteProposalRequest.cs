using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
	public class InviteProposalRequest
	{
		public int? FreelancerId { get; set; }
		public int JobId { get; set; }

		[Required]
		[Range(10.0, double.MaxValue, ErrorMessage = "Hourly rate must be greater than 10")]
		public decimal HourlyRate { get; set; }


		[Range(1, int.MaxValue, ErrorMessage = "Estimated hours must be greater than 1")]
		public int EstimatedHours { get; set; }

		public string Message { get; set; }
	}
}