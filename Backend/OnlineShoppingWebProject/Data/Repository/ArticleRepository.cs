using Data.Context;
using Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
	public class ArticleRepository : GenericRepository<Article>, IArticleRepository
	{
		public ArticleRepository(OnlineShopDbContext context) : base(context)
		{
		}

		public List<Article> GetAllArticlesFromSeller(long id)
		{
			List<Article> articles = _context.Articles.Where(x => x.SellerId == id).ToList();

			return articles;
		} 
	}
}
