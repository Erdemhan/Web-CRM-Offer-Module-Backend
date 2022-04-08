using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMWeb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using crmweb.Data.Entities;

namespace CRMWeb.Data.Configurations
{
    class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointment");

            builder.HasKey(p => p.Id);


        }

    }
}
