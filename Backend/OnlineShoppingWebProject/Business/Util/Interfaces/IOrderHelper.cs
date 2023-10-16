using Data.Models;
using System;
using System.Collections.Generic;

namespace Business.Util
{
	interface IOrderHelper
	{
		public List<IOrder> GetPendingOrders(List<IOrder> orders);

		List<IOrder> GetFinishedOrders(List<IOrder> orders);

		public DateTime GetDateTimeAsCEST(DateTime dateTime);

		bool IsOrderCancelable(IOrder order);

		string CalculateDeliveryRemainingTime(DateTime placedTime, int deliveryTimeInSeconds);

		bool IsOrderPending(IOrder order);
	}
}
