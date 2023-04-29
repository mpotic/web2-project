using Data.Context;
using Data.Models;

namespace Data.Repository
{
	public class ItemRepository : GenericRepository<Item>, IItemRepository
	{
		public ItemRepository(OnlineShopDbContext context) : base(context)
		{
		}
	}
}
