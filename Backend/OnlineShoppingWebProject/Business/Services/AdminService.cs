using AutoMapper;
using Business.Dto.Seller;
using Business.Result;
using Business.Util;
using Data.Models;
using Data.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using Business.Dto.Order;

namespace Business.Services
{
	public class AdminService : IAdminService
	{
		private IUnitOfWork _unitOfWork;

		private IMapper _mapper;

		private IUserHelper userHelper;

		public AdminService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			userHelper = new UserHelper(_unitOfWork);
		}

		public IServiceOperationResult UpdateSellerApprovalStatus(SellerApprovalStatusDto sellerApprovalDto)
		{
			IServiceOperationResult operationResult;

			ISeller seller = (ISeller)userHelper.FindUserByUsername(sellerApprovalDto.SellerUsername);
			if (seller == null)
			{
				operationResult = new ServiceOperationResult(false, ServiceOperationErrorCode.NotFound, "Seller doesn't exist!");

				return operationResult;
			}

			seller.ApprovalStatus = sellerApprovalDto.SellerApprovalStatus ? SellerApprovalStatus.Approved : SellerApprovalStatus.Denied;

			_unitOfWork.SellerRepository.Update((Seller)seller);
			_unitOfWork.Commit();

			operationResult = new ServiceOperationResult(true);

			return operationResult;
		}

		public IServiceOperationResult GetAllSellers()
		{
			IServiceOperationResult operationResult;

			List<ISeller> sellers = _unitOfWork.SellerRepository.GetAll().ToList<ISeller>();
			List<SellerInfoDto> sellerInfoDtoList = _mapper.Map<List<SellerInfoDto>>(sellers);
			SellerListDto sellerListDto = new SellerListDto() { Sellers = sellerInfoDtoList };

			foreach(var sellerDto in sellerListDto.Sellers)
			{
				string imagePath = sellers.Find(seller => seller.Username == sellerDto.Username).ProfileImage;
				sellerDto.SellerProfileImage = userHelper.GetProfileImage(imagePath);
			}

			operationResult = new ServiceOperationResult(true, sellerListDto);

			return operationResult;
		}

		public IServiceOperationResult AllOrders()
		{
			IServiceOperationResult operationResult;

			List<IOrder> allOrders = _unitOfWork.OrderRepository.GetAll().ToList<IOrder>();
			OrderInfoListDto orderInfoListDto = new OrderInfoListDto()
			{
				Orders = _mapper.Map<List<OrderInfoDto>>(allOrders)
			};

			operationResult = new ServiceOperationResult(true, orderInfoListDto);

			return operationResult;
		}
	}
}
