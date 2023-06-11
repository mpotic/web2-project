using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.Repository
{
	public interface IItemRepository : IGenericRepository<Item>
	{
		IEnumerable<Item> FindAllIncludeArticles(Expression<Func<Item, bool>> expression);
	}
}
