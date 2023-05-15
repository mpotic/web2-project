using Business.Dto.ArticleDto;
using Data.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Business.Util.Interfaces
{
	public interface ISellerHelper
	{
		void AddProductImageIfExists(IArticle article, IFormFile receivedImage, long sellerId);

		void UpdateArticleProps(ArticleUpdateDto articleDto, IArticle article);

		void UpdateProductImagePath(IArticle article);

		List<ArticleInfoDto> IncludeProductImageIfExistsToArticles(List<Article> articles);

		void DeleteArticleProductImageIfExists(IArticle article);
	}
}
