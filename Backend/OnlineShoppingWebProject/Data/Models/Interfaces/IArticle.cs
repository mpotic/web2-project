using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public interface IArticle
	{
		long Id { get; set; }

		string Name { get; set; }

		string Description { get; set; }

		int Quantity { get; set; }

		byte[] ProductImage { get; set; }

		/// <summary>
		/// Foreign key.
		/// </summary>
		long SellerId { get; set; }

		/// <summary>
		/// Navigational property.
		/// </summary>
		Seller Seller { get; set; }

		/// <summary>
		/// Navigational property.
		/// </summary>
		ICollection<Item> Items { get; set; }
	}
}
