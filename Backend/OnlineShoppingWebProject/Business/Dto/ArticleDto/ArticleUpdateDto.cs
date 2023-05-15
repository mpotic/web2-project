namespace Business.Dto.ArticleDto
{
	public class ArticleUpdateDto
	{
		public string NewName { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }
		
		public string CurrentName { get; set; }
	}
}
