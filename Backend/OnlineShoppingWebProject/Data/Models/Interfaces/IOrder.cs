using System;
using System.Collections.Generic;

namespace Data.Models
{
	public interface IOrder
	{
		long Id { get; set; }

		/// <summary>
		/// Sum of (Item.PricePerUnit * Item.Quantity) for each Item in Order.
		/// </summary>
		double TotalPrice { get; set; }
		
		/// <summary>
		/// Time at which the order was placed. 1hr after placement the order can't be cancelled.
		/// </summary>
		DateTime PlacedTime { get; set; }

		/// <summary>
		/// How long will the delivery take. Has to be > 1h. 
		/// </summary>
		int DeliveryDurationInSeconds { get; set; }

		string Comment { get; set; }

		string Address { get; set; }

		/// <summary>
		/// Foreign key.
		/// </summary>
		long? CustomerId { get; set; }

		/// <summary>
		/// Navigational property.
		/// </summary>
		Customer Customer { get; set; }

		/// <summary>
		/// Navigational property specifying that an Order can have many Items.
		/// </summary>
		ICollection<Item> Items { get; set; }
	}
}
