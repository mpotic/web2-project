using System.Collections.Generic;

namespace Business.Dto.Seller
{
	public class SellerListDto : IDto
	{
		public List<SellerInfoDto> Sellers { get; set; }
	}
}
