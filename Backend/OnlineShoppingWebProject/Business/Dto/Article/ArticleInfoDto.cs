namespace Business.Dto.Article
{
	public class ArticleInfoDto : IDto
	{
		public ArticleInfoDto(string name, string description, int quantity, byte[] productImage)
		{
			Name = name;
			Description = description;
			Quantity = quantity;
			ActualProductImage = productImage;
		}

		public string Name { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }

		public byte[] ActualProductImage { get; set; }
	}
}
