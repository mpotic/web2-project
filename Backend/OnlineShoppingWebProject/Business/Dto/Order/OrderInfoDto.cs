using System.Collections.Generic;

namespace Business.Dto.Order
{
	public class OrderInfoDto : IDto
	{
		public long Id { get; set; }

		public string Comment { get; set; }

		public string Address { get; set; }

		public double TotalPrice { get; set; }

		public ICollection<ItemInfoDto> Items { get; set; }
	}
}
