using Data.Context;
using Data.Models;

namespace Data.Repository
{
	public class SellerRepository : GenericRepository<Seller>, ISellerRepository
	{
		public SellerRepository(OnlineShopDbContext context) : base(context)
		{
		}
	}
}
