using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configuration.EFModelsConfiguration
{
    class WebSiteModelConfiguration : IEntityTypeConfiguration<WebSiteModel>
    {
        public void Configure(EntityTypeBuilder<WebSiteModel> builder)
        {
            builder.ToTable("WebSites").HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.WebAddress).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Password).HasMaxLength(30).IsRequired();
            builder.Property(e => e.Login).HasMaxLength(50).IsRequired();
            builder.Property(e => e.IsFavourite).IsRequired();
        }
    }
}
