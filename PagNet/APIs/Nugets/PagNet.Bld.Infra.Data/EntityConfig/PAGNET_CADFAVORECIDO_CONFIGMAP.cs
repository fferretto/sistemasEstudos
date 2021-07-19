using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PAGNET_CADFAVORECIDO_CONFIGMAP : IEntityTypeConfiguration<PAGNET_CADFAVORECIDO_CONFIG>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CADFAVORECIDO_CONFIG> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODFAVORECIDOCONFIG);


            // Table & Column Mappings
            entity.ToTable("PAGNET_CADFAVORECIDO_CONFIG");
            entity.Property(t => t.CODFAVORECIDOCONFIG).HasColumnName("CODFAVORECIDOCONFIG");
            entity.Property(t => t.CODFAVORECIDO).HasColumnName("CODFAVORECIDO");            
            entity.Property(t => t.REGRADIFERENCIADA).HasColumnName("REGRADIFERENCIADA");
            entity.Property(t => t.VALMINIMOCC).HasColumnName("VALMINIMOCC");
            entity.Property(t => t.VALTED).HasColumnName("VALTED");
            entity.Property(t => t.VALMINIMOTED).HasColumnName("VALMINIMOTED");
            entity.Property(t => t.CODCONTACORRENTE).HasColumnName("CODCONTACORRENTE");


            entity.HasOne(t => t.PAGNET_CADFAVORECIDO)
                .WithMany(t => t.PAGNET_CADFAVORECIDO_CONFIG)
                .HasForeignKey(t => t.CODFAVORECIDO);

        }
    }
}