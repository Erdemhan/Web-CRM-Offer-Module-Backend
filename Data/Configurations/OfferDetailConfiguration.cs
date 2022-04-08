using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crmweb.Data.Entities;

namespace crmweb.Data.Configurations
{
    public class OfferDetailConfiguration : IEntityTypeConfiguration<OfferDetail>
    {
        public void Configure(EntityTypeBuilder<OfferDetail> builder)
        {
            builder.ToTable("OfferDetail");

            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.DetailOfferHeader)
                .WithMany(p=>p.OfferDetail)
                .HasForeignKey(p => p.OfferId)
                .HasConstraintName("FK_OfferDetail_OfferHeader")
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
