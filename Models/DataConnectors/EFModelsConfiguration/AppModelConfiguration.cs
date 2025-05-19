using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataConnectors.EFModelsConfiguration
{
    internal class AppModelConfiguration : IEntityTypeConfiguration<AppModel>
    {
        public void Configure(EntityTypeBuilder<AppModel> builder)
        {
            builder.ToTable("Apps").HasKey(e => e.Id);
            builder.Property(e => e.Password).HasMaxLength(30).IsRequired();
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.IsFavourite).IsRequired();
        }
    }
    
}
