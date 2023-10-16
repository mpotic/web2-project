using Microsoft.AspNetCore.Http;

namespace Business.Dto.Article
{
	public class ArticleProductImageUpdateDto
	{
		public string Name { get; set; }

		public IFormFile ProductImage { get; set; }
	}
}
