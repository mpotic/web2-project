using System;
using System.Collections.Generic;

namespace Data.Models
{
	public class Order : IOrder
	{
		public long Id { get; set; }
		
		public double TotalPrice { get; set; }
		
		public DateTime PlacedTime { get; set; }
		
		public int DeliveryDurationInSeconds { get; set; }
		
		public string Comment { get; set; }
		
		public string Address { get; set; }
		
		public long? CustomerId { get; set; }
		
		public Customer Customer { get; set; }
		
		public ICollection<Item> Items { get; set; }
	}
}
