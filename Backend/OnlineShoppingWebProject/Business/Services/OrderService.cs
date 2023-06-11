using AutoMapper;
using Business.Dto.Order;
using Business.Result;
using Business.Services;
using Business.Util;
using Business.Util.Interfaces;
using Data.Models;
using Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		private readonly IOrderHelper orderHelper = new OrderHelper();

		private readonly ISellerHelper sellerHelper = new SellerHelper();

		public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public IServiceOperationResult GetOrderDetails(int id)
		{
			IServiceOperationResult operationResult;

			IOrder order = _unitOfWork.OrderRepository.GetById(id);
			if(order == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, 
					$"Order with the id \"{id}\" has not been found!");

				return operationResult;
			}
			
			OrderInfoDto orderDto = _mapper.Map<OrderInfoDto>(order);

			List<IItem> items = _unitOfWork.ItemRepository.FindAllIncludeArticles((item) => item.ArticleId == id).ToList<IItem>();
			orderDto.Items = _mapper.Map<List<ItemInfoDto>>(items);

			foreach(var orderItem in orderDto.Items)
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
