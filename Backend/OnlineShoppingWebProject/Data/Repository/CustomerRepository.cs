using Data.Context;
using Data.Models;

namespace Data.Repository
{
	public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
	{
		public CustomerRepository(OnlineShopDbContext context) : base(context)
		{
		}
	}
}
