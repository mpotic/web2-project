using System.Collections.Generic;

namespace Business.Dto.ArticleDto
{
	public class ArticleListDto : IDto
	{
		public List<ArticleInfoDto> Articles { get; set; } 
	}
}
