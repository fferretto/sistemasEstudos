using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PagNet_Config_Regra_Bol_LogMap : IEntityTypeConfiguration<PAGNET_CONFIG_REGRA_BOL_LOG>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CONFIG_REGRA_BOL_LOG> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODREGRA_LOG);

            // Table & Column Mappings
            entity.ToTable("PAGNET_CONFIG_REGRA_BOL_LOG");
            entity.Property(t => t.CODREGRA_LOG).HasColumnName("CODREGRA_LOG");
            entity.Property(t => t.CODREGRA).HasColumnName("CODREGRA");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.COBRAJUROS).HasColumnName("COBRAJUROS");
            entity.Property(t => t.VLJUROSDIAATRASO).HasColumnName("VLJUROSDIAATRASO");
            entity.Property(t => t.PERCJUROS).HasColumnName("PERCJUROS");
            entity.Property(t => t.COBRAMULTA).HasColumnName("COBRAMULTA");
            entity.Property(t => t.VLMULTADIAATRASO).HasColumnName("VLMULTADIAATRASO");
            entity.Property(t => t.PERCMULTA).HasColumnName("PERCMULTA");
            entity.Property(t => t.CODPRIMEIRAINSTCOBRA).HasColumnName("CODPRIMEIRAINSTCOBRA");
            entity.Property(t => t.CODSEGUNDAINSTCOBRA).HasColumnName("CODSEGUNDAINSTCOBRA");
            entity.Property(t => t.TAXAEMISSAOBOLETO).HasColumnName("TAXAEMISSAOBOLETO");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.DATINCLOG).HasColumnName("DATINCLOG");
            entity.Property(t => t.DESCLOG).HasColumnName("DESCLOG");

        }
    }
}