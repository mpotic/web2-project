using AutoMapper;
using Business.Dto.ArticleDto;
using Business.Dto.Auth;
using Business.Result;
using Business.TokenHelper;
using Business.Util;
using Data.Models;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace Business.Services
{
	public class SellerService : ISellerService
	{
		IUnitOfWork _unitOfWork;

		IUserTokenIssuer _tokenIssuer;

		IMapper _mapper;

		IUserHelper userHelper;

		IFiledValidationHelper validationHelper = new FieldValidationHelper();

		const string ArticleImageRelativePath = "../ArticleImages";

		public SellerService(IUnitOfWork unitOfWork, IUserTokenIssuer tokenIssuer, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_tokenIssuer = tokenIssuer;
			_mapper = mapper;
			userHelper = new UserHelper(unitOfWork);
		}

		public IServiceOperationResult AddArticle(NewArticleDto articleDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			if (validationHelper.AreStringPropsNullOrEmpty(articleDto))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Fields can not be left emtpy!");

				return operationResult;
			}

			if (articleDto.Quantity < 0)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Quantity can not be a negative number!");

				return operationResult;
			}

			long id = int.Parse(_tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "id"));
			string role = _tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "role");
			IUser user = userHelper.FindByIdAndRole(id, role);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Seller)user).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			if (_unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == user.Id && x.Name == articleDto.Name) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, $"Seller already has an article named \"{articleDto.Name}\"!");

				return operationResult;
			}

			Article article = _mapper.Map<Article>(articleDto);
			article.SellerId = user.Id;

			AddImageIfExists(article, articleDto.ProductImage, user.Id);

			_unitOfWork.ArticleRepository.Add(article);
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public IServiceOperationResult UpdateArticle(ArticleUpdateDto articleDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			long id = int.Parse(_tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "id"));
			string role = _tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "role");
			IUser user = userHelper.FindByIdAndRole(id, role);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Seller)user).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = _unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == user.Id && x.Name == articleDto.CurrentName);
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, $"Article named \"{articleDto.CurrentName}\" doesn't exist!");

				return operationResult;
			}

			if (_unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == user.Id && x.Name == articleDto.NewName) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, $"Seller already has an article named \"{articleDto.NewName}\"!");

				return operationResult;
			}

			UpdateArticleProps(articleDto, article);

			_unitOfWork.ArticleRepository.Update((Article)article);
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public IServiceOperationResult GetAllArticles(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			long id = int.Parse(_tokenIssuer.GetClaimValueFromToken(jwtDto.Token, "id"));
			
			List<Article> articles = _unitOfWork.ArticleRepository.GetAllArticlesFromSeller(id);
			
			if(articles.Count == 0)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound);

				return operationResult;
			}

			List<ArticleInfoDto> articleDtoList = AddProductImageIfExistsToArticles(articles);
			ArticleListDto response = new ArticleListDto() { Articles = articleDtoList };
			operationResult = new ServiceOperationResult(true, response);

			return operationResult;
		}

		private void AddImageIfExists(IArticle article, IFormFile receivedImage, long sellerId)
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

		private void UpdateArticleProps(ArticleUpdateDto articleDto, IArticle article)
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

		private void UpdateProductImagePath(IArticle article)
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

		private List<ArticleInfoDto> AddProductImageIfExistsToArticles(List<Article> articles)
		{
			List<ArticleInfoDto> articleDtoList = new List<ArticleInfoDto>();

			foreach(Article article in articles)
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
	}
}
