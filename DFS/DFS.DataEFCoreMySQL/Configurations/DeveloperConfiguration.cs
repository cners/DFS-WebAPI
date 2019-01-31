using DFS.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFS.DataEFCoreMySQL.Configurations
{
    public class DeveloperConfiguration
    {
        public DeveloperConfiguration(EntityTypeBuilder<Developer> entity)
        {
            //entity.HasIndex(e=>e.DevID)
            //    .HasName
            //entity.HasIndex(e => e.DevID)
            //     .HasName("IFK_Developer");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
