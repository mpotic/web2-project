using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public interface ISeller : IUser
	{
		/// <summary>
		/// Navigational property specifying that a seller can have many articles.
		/// </summary>
		ICollection<Article> Articles { get; set; }

		SellerApprovalStatus ApprovalStatus { get; set; }
	}
}
