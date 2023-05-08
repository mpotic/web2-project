using Data.Models;

namespace Business.Util
{
	public interface IUserHelper
	{
		IUser FindUserByUsername(string username);

		IUser FindUserByEmail(string email);

		bool ValidateUser(IUser user);

		bool ValidateChangeableUserData(IUser user);
	}
}
