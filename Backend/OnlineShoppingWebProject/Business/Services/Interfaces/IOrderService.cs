using Business.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
	public interface IOrderService
	{
		IServiceOperationResult GetOrderDetails(int id);
	}
}
