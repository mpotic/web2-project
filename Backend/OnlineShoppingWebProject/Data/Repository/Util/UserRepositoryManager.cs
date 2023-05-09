using Data.Models;
using Data.UnitOfWork;

namespace Data.Repository.Util
{
	public class UserRepositoryManager : IUserRepositoryManager
	{
		private readonly IUnitOfWork unitOfWork;

		public UserRepositoryManager(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public bool UpdateUser(IUser user)
		{
			if (user is Admin)
			{
				unitOfWork.AdminRepository.Update((Admin)user);
			}
			else if (user is Customer)
			{
				unitOfWork.CustomerRepository.Update((Customer)user);
			}
			else if (user is Seller)
			{
				unitOfWork.SellerRepository.Update((Seller)user);
			}
			else
			{
				return false;
			}

			return true;
		}
	}
}
