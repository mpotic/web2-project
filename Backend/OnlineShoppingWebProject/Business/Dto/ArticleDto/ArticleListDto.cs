using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dto.ArticleDto
{
	public class ArticleListDto : IDto
	{
		public List<ArticleInfoDto> Articles { get; set; } 
	}
}
