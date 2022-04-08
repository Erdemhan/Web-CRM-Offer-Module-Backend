using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Entities;

namespace crmweb.Data.Configurations
{
    public class CompanyContactConfiguration : IEntityTypeConfiguration<CompanyContact>
    {
        public void Configure(EntityTypeBuilder<CompanyContact> builder)
        {
            builder.ToTable("CompanyContact");
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Company)
                .WithMany(p => p.CompanyContact)
                .HasForeignKey(p => p.CompanyId)
                .HasConstraintName("FK_CompanyContact_Company")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
