using Repository.Entities;

namespace Repository.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		Task<User?> GetByEmail(string email);
	}
}
