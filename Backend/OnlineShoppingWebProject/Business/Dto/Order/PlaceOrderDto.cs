using System.Collections.Generic;

namespace Business.Dto.Order
{
	public class PlaceOrderDto : IDto
	{
		public string Comment { get; set; }

		public string Address { get; set; }

		public ICollection<PlaceItemDto> Items { get; set; }
	}
}
