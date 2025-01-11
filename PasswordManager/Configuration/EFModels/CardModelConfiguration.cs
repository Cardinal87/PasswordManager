using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Configuration.EFModels
{
    class CardModelConfiguration : IEntityTypeConfiguration<CardModel>
    {
        public void Configure(EntityTypeBuilder<CardModel> builder)
        {
            builder.ToTable("Cards").HasKey(e => e.Id);
            builder.Property(e => e.Number).HasMaxLength(16).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Owner).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Cvc).HasMaxLength(3).IsRequired();
            builder.Property(e => e.Year).HasMaxLength(4).IsRequired();
            builder.Property(e => e.Month).HasMaxLength(2).IsRequired();
            builder.Property(e => e.IsFavourite).IsRequired();
        }
    }
}
