using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_SOLICITACAOBOLETOMap : IEntityTypeConfiguration<PROC_PAGNET_SOLICITACAOBOLETO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_SOLICITACAOBOLETO> entity)
        {

            entity.HasKey(pc => new { pc.CODEMISSAOFATURAMENTO });
            
        }
    }
}