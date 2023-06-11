using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repository
{
	public class ItemRepository : GenericRepository<Item>, IItemRepository
	{
		public ItemRepository(OnlineShopDbContext context) : base(context)
		{
		}

		public IEnumerable<Item> FindAllIncludeArticles(Expression<Func<Item, bool>> expression)
		{
			var result = _context.Set<Item>().Include(item => item.Article).Where(expression).ToList();

			return result;
		}
	}
}
