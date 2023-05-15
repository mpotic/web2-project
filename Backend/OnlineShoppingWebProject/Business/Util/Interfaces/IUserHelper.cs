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

		IUser FindByIdAndRole(long id, string role);

		void UpdateBasicUserData(IUser currentUser, IUser newUser);
		
		bool UploadProfileImage(IUser user, IFormFile profileImage);

		byte[] GetProfileImage(string profileImageName);
	}
}
