using Data.Context;
using Data.Repository;

namespace Data.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private OnlineShopDbContext _context;

		public UnitOfWork(OnlineShopDbContext context)
		{
			_context = context;
			AdminRepository = new AdminRepository(context);
			CustomerRepository = new CustomerRepository(context);
			SellerRepository = new SellerRepository(context);
			ArticleRepository = new ArticleRepository(context);
			ItemRepository = new ItemRepository(context);
			OrderRepository = new OrderRepository(context);
		}

		public IAdminRepository AdminRepository { get; set; }

		public ICustomerRepository CustomerRepository { get; set; }

		public ISellerRepository SellerRepository { get; set; }

		public IArticleRepository ArticleRepository { get; set; }

		public IItemRepository ItemRepository { get; set; }

		public IOrderRepository OrderRepository { get; set; }

		public void Commit()
		{
			_context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
