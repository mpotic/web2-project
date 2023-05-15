using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dto.ArticleDto
{
	public class NewArticleDto
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }

		public IFormFile ProductImage { get; set; }
	}
}
