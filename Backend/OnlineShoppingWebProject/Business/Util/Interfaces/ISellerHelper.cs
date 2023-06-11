using Business.Dto.Article;
using Data.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Business.Util.Interfaces
{
	interface ISellerHelper
	{
		void AddProductImageIfExists(IArticle article, IFormFile receivedImage, long sellerId);

		void UpdateArticleProps(ArticleUpdateDto articleDto, IArticle article);

		void UpdateProductImagePath(IArticle article);

		List<ArticleInfoDto> IncludeImageAndReturnArticlesInfo(List<IArticle> articles);

		void DeleteArticleProductImageIfExists(IArticle article);
		
		byte[] GetArticleProductImage(IArticle article);
	}
}
