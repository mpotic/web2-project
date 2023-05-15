using Data.Models;
using System.Collections.Generic;

namespace Data.Repository
{
	public interface IArticleRepository : IGenericRepository<Article>
	{
		List<Article> GetAllArticlesFromSeller(long id);
	}
}
