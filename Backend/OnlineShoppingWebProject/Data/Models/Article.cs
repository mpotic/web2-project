using System.Collections.Generic;

namespace Data.Models
{
	public class Article : IArticle
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }

		public string ProductImage { get; set; }

		public long SellerId { get; set; }

		public Seller Seller { get; set; }

		public ICollection<Item> Items { get; set; }
	}
}
