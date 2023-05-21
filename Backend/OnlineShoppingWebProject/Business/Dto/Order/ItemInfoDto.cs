namespace Business.Dto.Order
{
	public class ItemInfoDto : IDto
	{
		public double PricePerUnit { get; set; }

		public int Quantity { get; set; }

		public string ArticleName { get; set; }
	}
}
