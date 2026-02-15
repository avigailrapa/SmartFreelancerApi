namespace Service.Interfaces
{
    public interface IUserService<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> UpdateItem(int id, T item);
        Task DeleteItem(int id);
    }
}
