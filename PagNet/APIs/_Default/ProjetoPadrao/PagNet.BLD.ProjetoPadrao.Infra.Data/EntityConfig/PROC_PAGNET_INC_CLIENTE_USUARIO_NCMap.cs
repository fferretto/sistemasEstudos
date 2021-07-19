using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_INC_CLIENTE_USUARIO_NCMap : IEntityTypeConfiguration<PROC_PAGNET_INC_CLIENTE_USUARIO_NC>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_INC_CLIENTE_USUARIO_NC> entity)
        {
            entity.HasKey(pc => new { pc.CODCLIENTE });

            entity.Property(t => t.CODCLIENTE).HasColumnName("CODCLIENTE");
        }
    }
}