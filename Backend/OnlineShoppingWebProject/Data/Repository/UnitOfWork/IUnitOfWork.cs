using Data.Repository;
using System;

namespace Data.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IAdminRepository AdminRepository { get; set; }

		ICustomerRepository CustomerRepository { get; set; }

		ISellerRepository SellerRepository { get; set; }

		IArticleRepository ArticleRepository { get; set; }

		IItemRepository ItemRepository { get; set; }

		IOrderRepository OrderRepository { get; set; }

		void Commit();
	}
}
