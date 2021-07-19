﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_SOLICITACAOBOLETOMap : IEntityTypeConfiguration<PROC_PAGNET_SOLICITACAOBOLETO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_SOLICITACAOBOLETO> entity)
        {

            entity.HasKey(pc => new { pc.CODEMISSAOFATURAMENTO });
            
        }
    }
}