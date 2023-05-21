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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		private readonly IUserTokenIssuer _tokenIssuer;

		private readonly IUserHelper userHelper;

		private readonly IOrderHelper orderHelper = new OrderHelper();

		private readonly IFiledValidationHelper validationHelper = new FieldValidationHelper();

		private readonly ISellerHelper sellerHelper = new SellerHelper();

		public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IUserTokenIssuer tokenIssuer)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_tokenIssuer = tokenIssuer;
			userHelper = new UserHelper(_unitOfWork);
		}

		public IServiceOperationResult GetAllArticles()
		{
			IServiceOperationResult operationResult;

			List<IArticle> articles = _unitOfWork.ArticleRepository.GetAll().ToList<IArticle>();
			List<ArticleInfoDto> articleListDto = sellerHelper.IncludeImageAndReturnArticlesInfo(articles);
			ArticleListDto articlesDto = new ArticleListDto() { Articles = articleListDto };

			operationResult = new ServiceOperationResult(true, articlesDto);

			return operationResult;
		}

		public IServiceOperationResult GetFinishedOrders(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ICustomer customer = (ICustomer)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (customer == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Customer doesn't exist!");

				return operationResult;
			}

			List<IOrder> orders = _unitOfWork.OrderRepository.FindAll(x => x.CustomerId == customer.Id).ToList<IOrder>();

			orders = orderHelper.GetFinishedOrders(orders);
			OrderInfoListDto orderListDto = new OrderInfoListDto()
			{
				Orders = _mapper.Map<List<OrderInfoDto>>(orders)
			};

			operationResult = new ServiceOperationResult(true, orderListDto);

			return operationResult;
		}

		public IServiceOperationResult GetPendingOrders(JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ICustomer customer = (ICustomer)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (customer == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Customer doesn't exist!");

				return operationResult;
			}

			List<IOrder> orders = _unitOfWork.OrderRepository.FindAllIncludeItems(x => x.CustomerId == customer.Id).ToList<IOrder>();

			orders = orderHelper.GetPendingOrders(orders);
			OrderInfoListDto orderListDto = new OrderInfoListDto()
			{
				Orders = _mapper.Map<List<OrderInfoDto>>(orders)
			};

			operationResult = new ServiceOperationResult(true, orderListDto);

			return operationResult;
		}

		public IServiceOperationResult PlaceOrder(PlaceOrderDto orderDto, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ICustomer customer = (ICustomer)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (customer == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Customer doesn't exist!");

				return operationResult;
			}

			if (validationHelper.AreStringPropsNullOrEmpty(orderDto))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Order data is not properly filled in!");

				return operationResult;
			}

			List<IArticle> associatedArticles = new List<IArticle>();
			foreach (var item in orderDto.Items)
			{
				if (validationHelper.AreStringPropsNullOrEmpty(item))
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, "Item data is not properly filled in!");

					return operationResult;
				}

				IArticle article = _unitOfWork.ArticleRepository.FindFirst(x => x.Id == item.ArticleId);

				if (article == null)
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, $"Article with id \"{item.ArticleId}\" doesn't exist!");

					return operationResult;
				}

				if(item.Quantity <= 0)
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, $"Quantity has to be at least 1 for every item!");

					return operationResult;
				}

				if(item.Quantity > article.Quantity)
				{
					operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.BadRequest, 
						$"There is not {item.Quantity} units of \"{article.Name}\" in storage!");

					return operationResult;
				}

				associatedArticles.Add(article);
			}

			Order order = _mapper.Map<Order>(orderDto);

			order.TotalPrice = order.Items.Sum(item => associatedArticles.Find(article => article.Id == item.ArticleId).Price * item.Quantity);

			foreach(var item in order.Items)
			{
				IArticle article = associatedArticles.Find(article => article.Id == item.ArticleId);
				item.PricePerUnit = article.Price;
				item.ArticleName = article.Name;
				article.Quantity -= item.Quantity;
				_unitOfWork.ArticleRepository.Update((Article)article);
				_unitOfWork.ItemRepository.Add(item);
			}

			order.PlacedTime = orderHelper.GetDateTimeAsCEST(DateTime.Now);
			order.DeliveryDurationInSeconds = new Random().Next(3600, 7200);
			order.CustomerId = customer.Id;

			_unitOfWork.OrderRepository.Add(order);
			
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public IServiceOperationResult CancleOrder(long orderId, JwtDto jwtDto)
		{
			IServiceOperationResult operationResult;

			ICustomer customer = (ICustomer)userHelper.FindUserByJwt(jwtDto.Token, _tokenIssuer);
			if (customer == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Customer doesn't exist!");

				return operationResult;
			}

			IOrder order = _unitOfWork.OrderRepository.FindFirst(x => x.Id == orderId);
			if(order == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Order doesn't exist!");

				return operationResult;
			}

			if (!orderHelper.IsOrderCancelable(order))
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.Conflict, 
					"Order can not be cancelled since it was placed more than an hour ago!");

				return operationResult;
			}

			_unitOfWork.OrderRepository.Remove((Order)order);
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}
	}
}
