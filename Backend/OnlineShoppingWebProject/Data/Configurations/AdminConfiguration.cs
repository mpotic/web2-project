using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
	internal class AdminConfiguration : IEntityTypeConfiguration<Admin>
	{
		public void Configure(EntityTypeBuilder<Admin> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			builder.HasIndex(x => x.Username)
				.IsUnique();
			builder.Property(x => x.Username)
				.IsRequired();

			builder.HasIndex(x => x.Email)
				.IsUnique();
			builder.Property(x => x.Email)
				.IsRequired();

			builder.Property(x => x.ProfileImage).HasColumnType("varbinary(max)");
		}
	}
}
