using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
	internal class ArticleConfiguration : IEntityTypeConfiguration<Article>
	{
		public void Configure(EntityTypeBuilder<Article> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			builder.Property(x => x.ProductImage)
				.HasColumnType("varbinary(max)");

			builder.HasMany(x => x.Items)
				.WithOne(x => x.Article)
				.HasForeignKey(x => x.ArticleId)
				.OnDelete(DeleteBehavior.ClientSetNull);
		}
	}
}
