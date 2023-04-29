using Data.Context;
using Data.Models;

namespace Data.Repository
{
	public class ArticleRepository : GenericRepository<Article>, IArticleRepository
	{
		public ArticleRepository(OnlineShopDbContext context) : base(context)
		{
		}
	}
}
