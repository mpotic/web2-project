﻿namespace Business.Dto.Article
{
	public class ArticleUpdateDto
	{
		public string NewName { get; set; }

		public string Description { get; set; }

		public int Quantity { get; set; }

		public double Price { get; set; }
		
		public string CurrentName { get; set; }
	}
}
