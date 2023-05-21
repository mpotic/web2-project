using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repository
{
	public interface IOrderRepository : IGenericRepository<Order>
	{
		IEnumerable<Order> FindAllIncludeItems(Expression<Func<Order, bool>> expression);
	}
}
