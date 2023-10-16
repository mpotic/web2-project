namespace Business.Dto.Order
{
	public class PlaceItemDto : IDto
	{
		public int Quantity { get; set; }

		public long ArticleId { get; set; }
	}
}
