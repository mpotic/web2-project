using System.Collections.Generic;

namespace Business.Dto.Article
{
	public class ArticleListDto : IDto
	{
		public List<ArticleInfoDto> Articles { get; set; } 
	}
}
