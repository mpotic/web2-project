using Data.Models;
using Data.UnitOfWork;

namespace Business.Util
{
	public class UserHelper : IUserHelper
	{
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

		public bool ValidateChangeableUserData(IUser user)
		{
			IAuthHelper authHelper = new AuthHelper();

			if (user.Id <= 0)
				return false;

			if (string.IsNullOrWhiteSpace(user.Firstname))
				return false;

			if (string.IsNullOrWhiteSpace(user.Lastname))
				return false;

			if (string.IsNullOrWhiteSpace(user.Username))
				return false;

			if (string.IsNullOrWhiteSpace(user.Address))
				return false;

			if (string.IsNullOrWhiteSpace(user.Email) || !authHelper.IsEmailValid(user.Email))
				return false;

			return true;
		}

		public void UpdateChangeableUserData(User oldUser, User newUser)
		{
			oldUser.Address = newUser.Address;
			oldUser.Firstname = newUser.Firstname;
			oldUser.Lastname = newUser.Lastname;
			oldUser.Username = newUser.Username;
			oldUser.Email = newUser.Email;
		}

		public bool ValidateUser(IUser user)
		{
			throw new System.NotImplementedException();
		}
	}
}
