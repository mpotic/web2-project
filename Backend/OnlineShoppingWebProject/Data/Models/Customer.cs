using System.Collections.Generic;

namespace Data.Models
{
	public class Customer : ICustomer
	{
		public long Id { get; set; }

		public string Firstname { get; set; }

		public string Lastname { get; set; }

		public string Username { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string Address { get; set; }

		public byte[] ProfileImage { get; set; }
	
		public ICollection<Order> Orders { get; set; }
	}
}
