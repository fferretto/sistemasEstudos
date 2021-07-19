using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PAGNET_INSTRUCAOCOBRANCAMap : IEntityTypeConfiguration<PAGNET_INSTRUCAOCOBRANCA>
    {
        public void Configure(EntityTypeBuilder<PAGNET_INSTRUCAOCOBRANCA> entity)
        {
            // Primary Key
            entity.HasKey(t => t.codInstrucaoCobranca);

            // Properties
            entity.Property(t => t.nmInstrucaoCobranca)
                .IsFixedLength()
                .HasMaxLength(100);

            // Table & Column Mappings
            entity.ToTable("PAGNET_INSTRUCAOCOBRANCA");
            entity.Property(t => t.codInstrucaoCobranca).HasColumnName("codInstrucaoCobranca");
            entity.Property(t => t.nmInstrucaoCobranca).HasColumnName("nmInstrucaoCobranca");
            entity.Property(t => t.bAtivo).HasColumnName("bAtivo");
        }
    }
}
