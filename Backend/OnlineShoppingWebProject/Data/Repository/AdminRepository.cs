using Data.Context;
using Data.Models;

namespace Data.Repository
{
	public class AdminRepository : GenericRepository<Admin>, IAdminRepository
	{
		public AdminRepository(OnlineShopDbContext context) : base(context)
		{
		}
	}
}
