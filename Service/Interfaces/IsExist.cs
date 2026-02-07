using Common;

namespace Service.Interfaces
{
    public interface IsExist<T>
    {
        public T Exist(Login login);
	}
}
