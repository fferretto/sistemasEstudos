using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_DETALHAMENTO_COBRANCAMap : IEntityTypeConfiguration<PROC_PAGNET_DETALHAMENTO_COBRANCA>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_DETALHAMENTO_COBRANCA> entity)
        {
            entity.HasKey(pc => new { pc.DESCRICAO });
        }
    }
}