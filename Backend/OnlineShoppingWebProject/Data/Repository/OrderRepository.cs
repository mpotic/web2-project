﻿using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repository
{
	public class OrderRepository : GenericRepository<Order>, IOrderRepository
	{
		public OrderRepository(OnlineShopDbContext context) : base(context)
		{
		}

		public IEnumerable<Order> FindAllIncludeItems(Expression<Func<Order, bool>> expression)
		{
			var result = _context.Set<Order>().Include(order => order.Items).Where(expression).ToList();

			return result;
		}

		public IEnumerable<Order> FindAllIncludeItemsIncludeArticles(Expression<Func<Order, bool>> expression)
		{
			var result = _context.Set<Order>().Include(order => order.Items).ThenInclude(item => item.Article).Where(expression).ToList();

			return result;
		}

		public Order FindFirstIncludeItems(Expression<Func<Order, bool>> expression)
		{
			var result = _context.Set<Order>().Include(order => order.Items).FirstOrDefault(expression);

			return result;
		}
	}
}
