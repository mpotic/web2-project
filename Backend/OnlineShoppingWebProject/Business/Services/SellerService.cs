using AutoMapper;
using Business.Dto.Article;
using Business.Dto.Auth;
using Business.Dto.Order;
using Business.Result;
using Business.TokenHelper;
using Business.Util;
using Business.Util.Interfaces;
using Data.Models;
using Data.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
	public class SellerService : ISellerService
	{
		IUnitOfWork _unitOfWork;

		IMapper _mapper;

		IUserTokenIssuer _tokenIssuer;

		IUserHelper userHelper;

		IFiledValidationHelper validationHelper = new FieldValidationHelper();

		ISellerHelper sellerHelper = new SellerHelper();

		IOrderHelper orderHelper = new OrderHelper();

		public SellerService(IUnitOfWork unitOfWork, IMapper mapper, IUserTokenIssuer tokenIssuer)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_tokenIssuer = tokenIssuer;
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

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Seller)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			if (_unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == seller.Id && x.Name == articleDto.Name) != null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, $"Seller already has an article named \"{articleDto.Name}\"!");

				return operationResult;
			}

			Article article = _mapper.Map<Article>(articleDto);
			article.SellerId = seller.Id;

			sellerHelper.AddProductImageIfExists(article, articleDto.ProductImage, seller.Id);

			_unitOfWork.ArticleRepository.Add(article);
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public IServiceOperationResult UpdateArticle(ArticleUpdateDto articleDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Seller)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = _unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == seller.Id && x.Name == articleDto.CurrentName);
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound,
					$"Article named \"{articleDto.CurrentName}\" doesn't exist among sellers aricles!");

				return operationResult;
			}

			if (_unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == seller.Id && x.Name == articleDto.NewName && x.Id != article.Id) != null)
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

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Seller)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			List<IArticle> articles = _unitOfWork.ArticleRepository.GetAllArticlesFromSeller(seller.Id).ToList<IArticle>();

			if (articles.Count == 0)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller has no articles!");

				return operationResult;
			}

			List<ArticleInfoDto> articleDtoList = sellerHelper.IncludeImageAndReturnArticlesInfo(articles);
			ArticleListDto response = new ArticleListDto() { Articles = articleDtoList };
			operationResult = new ServiceOperationResult(true, response);

			return operationResult;
		}

		public IServiceOperationResult GetArticleDetails(JwtDto jwtDto, string name)
		{
			IServiceOperationResult operationResult;

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Seller)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = _unitOfWork.ArticleRepository.FindFirst(article => article.Name == name && article.SellerId == seller.Id);

			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Article not found!");

				return operationResult;
			}

			var image = sellerHelper.GetArticleProductImage(article);
			ArticleInfoDto articleDto = new ArticleInfoDto(article.Id, article.Name, article.Description, article.Quantity, article.Price, image);
			operationResult = new ServiceOperationResult(true, articleDto);

			return operationResult;
		}

		public IServiceOperationResult UpdateArticleProductImage(ArticleProductImageUpdateDto articleDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Seller)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = (_unitOfWork.ArticleRepository.FindFirst(x => x.SellerId == seller.Id && x.Name == articleDto.Name));
			if (article == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound,
					$"Article named \"{articleDto.Name}\" doesn't exist among sellers aricles!");

				return operationResult;
			}

			if(articleDto.ProductImage == null)
			{
				sellerHelper.DeleteArticleProductImageIfExists(article);
				article.ProductImage = null;
			}

			sellerHelper.AddProductImageIfExists(article, articleDto.ProductImage, seller.Id);

			_unitOfWork.ArticleRepository.Update((Article)article);
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public IServiceOperationResult DeleteArticle(string articleName, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			if (((Seller)seller).ApprovalStatus != SellerApprovalStatus.Approved)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller isn't approved!");

				return operationResult;
			}

			IArticle article = _unitOfWork.ArticleRepository.FindFirst(x => x.Name == articleName && x.SellerId == seller.Id);
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

		public IServiceOperationResult GetFinishedOrders(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			// Find all orders that have items which belong to the seller in question
			List<IOrder> orders = _unitOfWork.OrderRepository.FindAllIncludeItemsIncludeArticles(
				order => order.Items.FirstOrDefault(item => item.Article.SellerId == seller.Id) != null).ToList<IOrder>();

			orders.RemoveAll(order => orderHelper.IsOrderPending(order));

			// Only take items that are related to the article of the seller in question
			orders.ForEach(order => order.Items = order.Items.ToList().FindAll(item => item.Article.SellerId == seller.Id));

			OrderInfoListDto orderListDto = new OrderInfoListDto()
			{
				Orders = _mapper.Map<List<OrderInfoDto>>(orders)
			};

			foreach (var orderDto in orderListDto.Orders)
			{
				IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
				orderDto.RemainingTime = orderHelper.CalculateDeliveryRemainingTime(orderDto.PlacedTime, relatedOrder.DeliveryDurationInSeconds);
			}

			operationResult = new ServiceOperationResult(true, orderListDto);

			return operationResult;
		}

		public IServiceOperationResult GetPendingOrders(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			// Find all orders that have items which belong to the seller in question
			List<IOrder> orders = _unitOfWork.OrderRepository.FindAllIncludeItemsIncludeArticles(
				order => order.Items.FirstOrDefault(item => item.Article.SellerId == seller.Id) != null).ToList<IOrder>();

			orders.RemoveAll(order => !orderHelper.IsOrderPending(order));

			// Only take items that are related to the article of the seller in question
			orders.ForEach(order => order.Items = order.Items.ToList().FindAll(item => item.Article.SellerId == seller.Id));

			OrderInfoListDto orderListDto = new OrderInfoListDto()
			{
				Orders = _mapper.Map<List<OrderInfoDto>>(orders)
			};

			foreach (var orderDto in orderListDto.Orders)
			{
				IOrder relatedOrder = orders.Find(x => x.Id == orderDto.Id);
				orderDto.RemainingTime = orderHelper.CalculateDeliveryRemainingTime(orderDto.PlacedTime, relatedOrder.DeliveryDurationInSeconds);
			}

			operationResult = new ServiceOperationResult(true, orderListDto);

			return operationResult;
		}

		public IServiceOperationResult GetOrderDetails(JwtDto jwtDto, long id)
		{
			IServiceOperationResult operationResult;

			ISeller seller = (ISeller)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;	
			}

			IOrder order = _unitOfWork.OrderRepository.GetById(id);
			if (order == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound,
					$"Order with the id \"{id}\" has not been found!");

				return operationResult;
			}

			OrderInfoDto orderDto = _mapper.Map<OrderInfoDto>(order);
			orderDto.RemainingTime = orderHelper.CalculateDeliveryRemainingTime(orderDto.PlacedTime, order.DeliveryDurationInSeconds);

			List<IItem> items = _unitOfWork.ItemRepository.FindAllIncludeArticles((item) => item.OrderId == id).ToList<IItem>();
			items.RemoveAll(item => item.Article.SellerId != seller.Id);
			orderDto.Items = _mapper.Map<List<ItemInfoDto>>(items);

			foreach (var orderItem in orderDto.Items)
			{
				IArticle article = items.Find(item => item.ArticleId == orderItem.ArticleId).Article;
				byte[] image = sellerHelper.GetArticleProductImage(article);
				orderItem.ArticleImage = image;
			}

			operationResult = new ServiceOperationResult(true, orderDto);

			return operationResult;
		}
	}
}
