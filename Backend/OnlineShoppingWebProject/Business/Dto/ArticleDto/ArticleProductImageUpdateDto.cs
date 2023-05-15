using Microsoft.AspNetCore.Http;

namespace Business.Dto.ArticleDto
{
	public class ArticleProductImageUpdateDto
	{
		public string Name { get; set; }

		public IFormFile ProductImage { get; set; }
	}
}
