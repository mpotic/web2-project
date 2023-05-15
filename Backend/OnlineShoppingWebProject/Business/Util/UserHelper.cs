using Business.Dto.User;
using Business.TokenHelper;
using Data.Models;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Business.Util
{
	public class UserHelper : IUserHelper
	{
		private const string profileImageRelativePath = "../ProfileImages";

		public string ProfileImagesRelativePath => profileImageRelativePath;

		private IUnitOfWork unitOfWork;

		public UserHelper(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public IUser FindUserByUsername(string username)
		{
			if (unitOfWork.AdminRepository.FindFirst(x => x.Username == username) is Admin admin)
			{
				return admin;
			}
			else if (unitOfWork.CustomerRepository.FindFirst(x => x.Username == username) is Customer customer)
			{
				return customer;
			}
			else if (unitOfWork.SellerRepository.FindFirst(x => x.Username == username) is Seller seller)
			{
				return seller;
			}

			return null;
		}

		public IUser FindUserByEmail(string email)
		{
			if (unitOfWork.AdminRepository.FindFirst(x => x.Email == email) is Admin admin)
			{
				return admin;
			}
			else if (unitOfWork.CustomerRepository.FindFirst(x => x.Email == email) is Customer customer)
			{
				return customer;

			}
			else if (unitOfWork.SellerRepository.FindFirst(x => x.Email == email) is Seller seller)
			{
				return seller;
			}

			return null;
		}

		public IUser FindByIdAndRole(long id, string role)
		{
			if (role == UserType.Admin.ToString() && unitOfWork.AdminRepository.FindFirst(x => x.Id == id) is Admin admin)
			{
				return admin;
			}
			else if (role == UserType.Customer.ToString() && unitOfWork.CustomerRepository.FindFirst(x => x.Id == id) is Customer customer)
			{
				return customer;

			}
			else if (role == UserType.Seller.ToString() && unitOfWork.SellerRepository.FindFirst(x => x.Id == id) is Seller seller)
			{
				return seller;
			}

			return null;
		}

		public IUser FindUserByJwt(string token, IUserTokenIssuer tokenIssuer)
		{
			long id = int.Parse(tokenIssuer.GetClaimValueFromToken(token, "id"));
			string role = (tokenIssuer.GetClaimValueFromToken(token, "role"));
			IUser user = FindByIdAndRole(id, role);

			return user;
		}

		public void UpdateBasicUserData(IUser currentUser, IUser newUser)
		{
			if (!string.IsNullOrWhiteSpace(newUser.Address))
			{
				currentUser.Address = newUser.Address;
			}

			if (!string.IsNullOrWhiteSpace(newUser.Firstname))
			{
				currentUser.Firstname = newUser.Firstname;
			}

			if (!string.IsNullOrWhiteSpace(newUser.Lastname))
			{
				currentUser.Lastname = newUser.Lastname;
			}

			if (!string.IsNullOrWhiteSpace(newUser.Username))
			{
				currentUser.Username = newUser.Username;
			}

			if (newUser.Birthdate != System.DateTime.MinValue)
			{
				currentUser.Birthdate = newUser.Birthdate;
			}
		}

		public bool UploadProfileImage(IUser user, IFormFile profileImage)
		{
			if (profileImage == null)
			{
				return false;
			}

			string profileImageDir = Path.Combine(Directory.GetCurrentDirectory(), ProfileImagesRelativePath);

			if (!Directory.Exists(profileImageDir))
			{
				Directory.CreateDirectory(profileImageDir);
			}

			string fileExtension = Path.GetExtension(profileImage.FileName);
			string profileImageFileName = Path.Combine(profileImageDir, user.Username) + fileExtension;

			using (FileStream fs = new FileStream(profileImageFileName, FileMode.Create))
			{
				profileImage.CopyTo(fs);
			}

			user.ProfileImage = user.Username + fileExtension;

			return true;
		}

		public byte[] GetProfileImage(string profileImageName)
		{
			string path = Path.Combine(ProfileImagesRelativePath, profileImageName);

			if (!File.Exists(path))
			{
				return null;
			}

			byte[] image = File.ReadAllBytes(path);

			return image;
		}

		public void UpdateProfileImagePath(IUser currentUser, string newUsername)
		{
			if (currentUser.Username == newUsername)
			{
				return;
			}

			string oldProfileImagePath = Path.Combine(Directory.GetCurrentDirectory(), ProfileImagesRelativePath, currentUser.ProfileImage);

			if (!File.Exists(oldProfileImagePath))
			{
				return;
			}

			string fileExtension = Path.GetExtension(currentUser.ProfileImage);
			string profileImageFileName = newUsername + fileExtension;

			string newProfileImagePath = Path.Combine(Directory.GetCurrentDirectory(), ProfileImagesRelativePath, profileImageFileName);
			File.Move(oldProfileImagePath, newProfileImagePath);

			currentUser.ProfileImage = profileImageFileName;
		}
	}
}
