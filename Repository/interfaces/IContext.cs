using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;


namespace Repository.interfaces
{
	public interface IContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Freelancer> Freelancers { get; set; }
		public DbSet<Job> Jobs { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public Task Save();

	}
}
