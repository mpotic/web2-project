﻿using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
	public class SellerConfiguration : IEntityTypeConfiguration<Seller>
	{
		public void Configure(EntityTypeBuilder<Seller> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).ValueGeneratedOnAdd();

			builder.HasIndex(x => x.Username)
				.IsUnique();
			builder.Property(x => x.Username)
				.IsRequired();

			builder.HasIndex(x => x.Email)
				.IsUnique();
			builder.Property(x => x.Email)
				.IsRequired();

			builder.Property(x => x.ProfileImage)
				.HasColumnType("varbinary(max)");

			builder.Property(x => x.ApprovalStatus)
				.HasConversion<string>();

			builder.HasMany(x => x.Articles)
				.WithOne(x => x.Seller)
				.HasForeignKey(x => x.SellerId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
