using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
