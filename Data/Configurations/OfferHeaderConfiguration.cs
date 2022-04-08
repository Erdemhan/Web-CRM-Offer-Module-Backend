using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crmweb.Data.Entities;

namespace crmweb.Data.Configurations
{
    public class OfferHeaderConfiguration : IEntityTypeConfiguration<OfferHeader>
    {
        public void Configure(EntityTypeBuilder<OfferHeader> builder)
        {
            builder.ToTable("OfferHeader");

            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Createby)
                .WithMany(p => p.CreatedOffers)
                .HasForeignKey(p => p.CreateById)
                .HasConstraintName("FK_OfferHeader_User");

            builder.HasOne(p => p.OfferCompany)
                .WithMany(p => p.OfferHeaders) 
                .HasForeignKey(p => p.CompanyId)
                .HasConstraintName("FK_OfferHeader_Company");

            builder.HasOne(p => p.OfferCompanyContact)
                .WithMany(p => p.OfferHeaders)
                .HasForeignKey(p => p.CompanyContactId)
                .HasConstraintName("FK_OfferHeader_CompanyContact");

        }
    }
}
