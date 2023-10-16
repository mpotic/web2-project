using System.Collections.Generic;

namespace Data.Models
{
	public class Seller : User, ISeller
	{
		public ICollection<Article> Articles { get; set; }

		public SellerApprovalStatus ApprovalStatus { get; set; }
	}
}
