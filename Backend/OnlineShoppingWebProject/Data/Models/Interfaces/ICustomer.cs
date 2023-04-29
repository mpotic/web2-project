using System.Collections.Generic;

namespace Data.Models
{
	public interface ICustomer : IUser
	{
		/// <summary>
		/// Navigational property.
		/// </summary>
		ICollection<Order> Orders { get; set; } 
	}
}
