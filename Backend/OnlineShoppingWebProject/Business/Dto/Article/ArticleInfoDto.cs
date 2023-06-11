namespace Business.Dto.Article
{
	public class ArticleInfoDto : IDto
	{
		public ArticleInfoDto(string name, string description, int quantity, double price, byte[] productImage)
		{
			Name = name;
			Description = description;
			Quantity = quantity;
			Price = price;
			ProductImage = productImage;
		}

		public string Name { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }

		public byte[] ProductImage { get; set; }
	}
}
