using AutoMapper;
using Business.Dto.Seller;
using Business.Result;
using Business.Util;
using Data.Models;
using Data.UnitOfWork;

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

			ISeller seller = (ISeller)userHelper.FindUserByUsername(sellerApprovalDto.SellerName);
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
	}
}
