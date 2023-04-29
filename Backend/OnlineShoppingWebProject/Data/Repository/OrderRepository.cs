using Data.Context;
using Data.Models;

namespace Data.Repository
{
	public class OrderRepository : GenericRepository<Order>, IOrderRepository
	{
		public OrderRepository(OnlineShopDbContext context) : base(context)
		{
		}
	}
}
