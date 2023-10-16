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
				if (IsOrderPending(order))
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
				if (!IsOrderPending(order))
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

		public string CalculateDeliveryRemainingTime(DateTime placedTime, int deliveryTimeInSeconds)
		{
			int secondsPassed = (int)(GetDateTimeAsCEST(DateTime.Now) - placedTime).TotalSeconds;
			int secondsLeft = deliveryTimeInSeconds - secondsPassed;

			if(secondsLeft < 0)
			{
				secondsLeft = 0;
			}

			TimeSpan timeSpan = TimeSpan.FromSeconds(secondsLeft);
			string formattedTime = timeSpan.ToString(@"hh\:mm\:ss");

			return formattedTime;
		}

		public bool IsOrderPending(IOrder order)
		{
			int secondsPassed = (int)(GetDateTimeAsCEST(DateTime.Now) - order.PlacedTime).TotalSeconds;
			if (order.DeliveryDurationInSeconds > secondsPassed)
			{
				return true;
			}

			return false;
		}
	}
}
