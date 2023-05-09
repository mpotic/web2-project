using Data.Models;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Business.Util
{
	public class UserHelper : IUserHelper
	{
		private readonly string profileImageRelativePath = "../ProfileImages";

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

		public IUser FindById(long id)
		{
			if (unitOfWork.AdminRepository.FindFirst(x => x.Id == id) is Admin admin)
			{
				return admin;
			}
			else if (unitOfWork.CustomerRepository.FindFirst(x => x.Id == id) is Customer customer)
			{
				return customer;

			}
			else if (unitOfWork.SellerRepository.FindFirst(x => x.Id == id) is Seller seller)
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

			if(newUser.Birthdate != System.DateTime.MinValue)
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
	}
}
