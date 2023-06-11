using Business.Dto.Auth;
using Business.Dto.Order;
using Business.Result;

namespace Business.Services
{
	public interface ICustomerService
	{
		IServiceOperationResult GetAllArticles();

		IServiceOperationResult GetPendingOrders(JwtDto jwtDto);

		IServiceOperationResult GetFinishedOrders(JwtDto jwtDto);

		IServiceOperationResult PlaceOrder(PlaceOrderDto orderDto, JwtDto jwDto);

		IServiceOperationResult CancelOrder(long orderId, JwtDto jwtDto);

		IServiceOperationResult GetOrderDetails(long id);
	}
}
