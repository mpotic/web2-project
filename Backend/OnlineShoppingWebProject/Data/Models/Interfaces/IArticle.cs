using System.Collections.Generic;

namespace Data.Models
{
	public interface IArticle
	{
		long Id { get; set; }

		string Name { get; set; }

		string Description { get; set; }

		int Quantity { get; set; }

		string ProductImage { get; set; }

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
