using Data.Models;
using System;
using System.Collections.Generic;

namespace Business.Util
{
	public class OrderHelper : IOrderHelper
	{
		public List<IOrder> GetPendingOrders(List<IOrder> orders)
		{
			List<IOrder> pendingOrders = new List<IOrder>();

			foreach (var order in orders)
			{
				int secondsPassed = (int)(GetDateTimeAsCEST(DateTime.Now) - order.PlacedTime).TotalSeconds;
				if (order.DeliveryDurationInSeconds > secondsPassed)
				{
					pendingOrders.Add(order);
				}
			}

			return pendingOrders;
		}

		public List<IOrder> GetFinishedOrders(List<IOrder> orders)
		{
			List<IOrder> finishedOrders = new List<IOrder>();

			foreach (var order in orders)
			{
				int secondsPassed = (int)(GetDateTimeAsCEST(DateTime.Now) - order.PlacedTime).TotalSeconds;
				if (order.DeliveryDurationInSeconds < secondsPassed)
				{
					finishedOrders.Add(order);
				}
			}

			return finishedOrders;
		}

		public DateTime GetDateTimeAsCEST(DateTime dateTime)
		{
			var cest = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

			return cest;
		}

		public bool IsOrderCancelable(IOrder order)
		{
			int secondsPassed = (int)(GetDateTimeAsCEST(DateTime.Now) - order.PlacedTime).TotalSeconds;
			if (secondsPassed > 3600)
			{
				return false;
			}

			return true;
		}
	}
}
