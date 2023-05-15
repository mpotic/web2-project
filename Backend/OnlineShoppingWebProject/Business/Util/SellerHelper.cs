using Business.Dto.ArticleDto;
using Business.Util.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace Business.Util
{
	public class SellerHelper : ISellerHelper
	{
		private readonly string ArticleImageRelativePath = "../ArticleImages";

		public void AddProductImageIfExists(IArticle article, IFormFile receivedImage, long sellerId)
		{
			if (receivedImage == null)
			{
				return;
			}

			string profileImageDir = Path.Combine(Directory.GetCurrentDirectory(), ArticleImageRelativePath);

			if (!Directory.Exists(profileImageDir))
			{
				Directory.CreateDirectory(profileImageDir);
			}

			string fileExtension = Path.GetExtension(receivedImage.FileName);
			string imageName = sellerId + "_" + article.Name;
			string profileImageFileName = Path.Combine(profileImageDir, imageName) + fileExtension;

			using (FileStream fs = new FileStream(profileImageFileName, FileMode.Create))
			{
				receivedImage.CopyTo(fs);
			}

			article.ProductImage = imageName + fileExtension;
		}

		public void UpdateArticleProps(ArticleUpdateDto articleDto, IArticle article)
		{
			if (articleDto == null || article == null)
			{
				return;
			}

			if (!string.IsNullOrWhiteSpace(articleDto.NewName))
			{
				article.Name = articleDto.NewName;
				UpdateProductImagePath(article);
			}

			if (!string.IsNullOrWhiteSpace(articleDto.Description))
			{
				article.Description = articleDto.Description;
			}

			if (articleDto.Quantity >= 0)
			{
				article.Quantity = articleDto.Quantity;
			}
		}

		public void UpdateProductImagePath(IArticle article)
		{
			string oldProductImagePath = Path.Combine(Directory.GetCurrentDirectory(), ArticleImageRelativePath, article.ProductImage);

			if (!File.Exists(oldProductImagePath))
			{
				return;
			}

			string fileExtension = Path.GetExtension(article.ProductImage);
			string newProductImageName = article.SellerId + "_" + article.Name + fileExtension;

			string newProductImagePath = Path.Combine(Directory.GetCurrentDirectory(), ArticleImageRelativePath, newProductImageName);
			File.Move(oldProductImagePath, newProductImagePath);

			article.ProductImage = newProductImageName;
		}

		public List<ArticleInfoDto> IncludeProductImageIfExistsToArticles(List<Article> articles)
		{
			List<ArticleInfoDto> articleDtoList = new List<ArticleInfoDto>();

			foreach (Article article in articles)
			{
				byte[] productImage = GetArticleProductImage(article);
				articleDtoList.Add(new ArticleInfoDto(article.Name, article.Description, article.Quantity, productImage));
			}

			return articleDtoList;
		}

		private byte[] GetArticleProductImage(IArticle article)
		{
			string productImageName = article.ProductImage;
			string productImagePath = Path.Combine(Directory.GetCurrentDirectory(), ArticleImageRelativePath, productImageName);

			if (!File.Exists(productImagePath))
			{
				return null;
			}

			byte[] image = File.ReadAllBytes(productImagePath);

			return image;
		}

		public void DeleteArticleProductImageIfExists(IArticle article)
		{
			string productImageName = article.ProductImage;
			string productImagePath = Path.Combine(Directory.GetCurrentDirectory(), ArticleImageRelativePath, productImageName);

			if (!File.Exists(productImagePath))
			{
				return;
			}

			File.Delete(productImagePath);
		}
	}
}
