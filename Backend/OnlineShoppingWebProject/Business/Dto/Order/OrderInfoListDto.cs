using System.Collections.Generic;

namespace Business.Dto.Order
{
	public class OrderInfoListDto : IDto
	{
		public List<OrderInfoDto> Orders { get; set; }
	}
}
