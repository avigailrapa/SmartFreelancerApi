using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.Dto
{
	public class FreelancerDto
	{
		public int FreelancerId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Image { get; set; }
		public string Bio { get; set; }
		public int AvailableHours { get; set; }
		public int HourlyRate { get; set; }
		public ExperienceLevel ExperienceLevel { get; set; }
		public FreelancerStatus Status { get; set; }
	}
}
