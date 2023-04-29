using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
	public class ArticleRepository : GenericRepository<Article>, IArticleRepository
	{
		public ArticleRepository(OnlineShopDbContext context) : base(context)
		{
		}
	}
}
