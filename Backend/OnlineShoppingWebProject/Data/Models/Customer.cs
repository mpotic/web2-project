using System.Collections.Generic;

namespace Data.Models
{
	public class Customer : User, ICustomer
	{
		public ICollection<Order> Orders { get; set; }
	}
}
