using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dto.Seller
{
	public class SellerListDto : IDto
	{
		public List<SellerInfoDto> Sellers { get; set; }
	}
}
