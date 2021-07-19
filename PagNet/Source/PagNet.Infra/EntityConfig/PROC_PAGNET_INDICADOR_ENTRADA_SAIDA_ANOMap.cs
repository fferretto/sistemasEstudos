using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANOMap : IEntityTypeConfiguration<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO> entity)
        {

            entity.HasKey(pc => new { pc.MESREF });

            entity.Property(t => t.MESREF).HasColumnName("MESREF");
            entity.Property(t => t.RECEITA).HasColumnName("RECEITA");
            entity.Property(t => t.DESPESA).HasColumnName("DESPESA");

        }
    }
}