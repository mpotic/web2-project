using Business.Dto.Seller;
using Business.Result;

namespace Business.Services
{
	public interface IAdminService
	{
		IServiceOperationResult UpdateSellerApprovalStatus(SellerApprovalStatusDto sellerApprovalDto);

		IServiceOperationResult GetAllSellers();
	}
}
