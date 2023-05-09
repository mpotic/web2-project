using Data.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Util
{
	public interface IUserHelper
	{
		string ProfileImagesRelativePath { get; }

		IUser FindUserByUsername(string username);
		
		IUser FindById(long id);

		IUser FindUserByEmail(string email);

		void UpdateBasicUserData(IUser currentUser, IUser newUser);
		
		bool UploadProfileImage(IUser user, IFormFile profileImage);
	}
}
