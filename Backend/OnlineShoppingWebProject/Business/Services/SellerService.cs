using AutoMapper;
using Business.Dto.ArticleDto;
using Business.Dto.Auth;
using Business.Result;
using Business.TokenHelper;
using Business.Util;
using Business.Util.Interfaces;
using Data.Models;
using Data.UnitOfWork;
using System.Collections.Generic;

namespace Business.Services
{
	public class SellerService : ISellerService
	{
		IUnitOfWork _unitOfWork;

		IUserTokenIssuer _tokenIssuer;

		IMapper _mapper;

		IUserHelper userHelper;

		IFiledValidationHelper validationHelper = new FieldValidationHelper();

		ISellerHelper sellerHelper = new SellerHelper();

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

			IUser user = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
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

			sellerHelper.AddProductImageIfExists(article, articleDto.ProductImage, user.Id);

			_unitOfWork.ArticleRepository.Add(article);
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public IServiceOperationResult UpdateArticle(ArticleUpdateDto articleDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
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
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, 
					$"Article named \"{articleDto.CurrentName}\" doesn't exist among sellers aricles!");

				return operationResult;
			}

			if (_unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == user.Id && x.Name == articleDto.NewName) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, $"Seller already has an article named \"{articleDto.NewName}\"!");

				return operationResult;
			}

			sellerHelper.UpdateArticleProps(articleDto, article);

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

			List<ArticleInfoDto> articleDtoList = sellerHelper.IncludeProductImageIfExistsToArticles(articles);
			ArticleListDto response = new ArticleListDto() { Articles = articleDtoList };
			operationResult = new ServiceOperationResult(true, response);

			return operationResult;
		}

		public IServiceOperationResult UpdateArticleProductImage(ArticleProductImageUpdateDto articleDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			IArticle article = (_unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == user.Id && x.Name == articleDto.Name));
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound,
					$"Article named \"{articleDto.Name}\" doesn't exist among sellers aricles!");

				return operationResult;
			}

			sellerHelper.AddProductImageIfExists(article, articleDto.ProductImage, user.Id);

			_unitOfWork.ArticleRepository.Update((Article)article);
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public IServiceOperationResult DeleteArticle(string articleName, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			IUser user = userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (user == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			IArticle article = _unitOfWork.ArticleRepository.FindFirst(x => x.Name == articleName && x.Id == user.Id);
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "The article doesn't exist!");

				return operationResult;
			}

			_unitOfWork.ArticleRepository.Remove((Article)article);
			_unitOfWork.Commit();

			sellerHelper.DeleteArticleProductImageIfExists(article);

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}
	}
}
