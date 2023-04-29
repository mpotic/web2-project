using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public class Article : IArticle
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }

		public byte[] ProductImage { get; set; }

		public long SellerId { get; set; }

		public Seller Seller { get; set; }

		public ICollection<Item> Items { get; set; }
	}
}
