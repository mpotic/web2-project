using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public interface IItem
	{
		/// <summary>
		/// Primary key.
		/// </summary>
		long Id { get; set; }

		string Name { get; set; }

		double PricePerUnit { get; set; }

		int Quantity { get; set; }

		/// <summary>
		/// Redundancy so that in case the associated Article gets deleted, this field still holds the information about the Article that was ordered.
		/// </summary>
		string ArticleName { get; set; }

		/// <summary>
		/// Foreign key.
		/// </summary>
		long OrderId { get; set; }

		/// <summary>
		/// Navigational property.
		/// </summary>
		Order Order { get; set; }

		/// <summary>
		/// Foreign key (nullable).
		/// </summary>
		long? ArticleId { get; set; }

		/// <summary>
		/// Navigational property.
		/// </summary>
		Article Article { get; set; }
	}
}
