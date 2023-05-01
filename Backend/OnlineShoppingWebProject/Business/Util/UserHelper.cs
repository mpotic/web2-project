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
	}
}
