using Data.Models;

namespace Data.Repository.Util
{
	public interface IUserRepositoryManager
	{
		bool UpdateUser(IUser user);
	}
}
