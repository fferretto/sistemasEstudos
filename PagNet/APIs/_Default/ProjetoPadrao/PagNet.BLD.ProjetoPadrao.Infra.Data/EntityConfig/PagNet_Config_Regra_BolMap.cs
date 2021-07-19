using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_Config_Regra_BolMap : IEntityTypeConfiguration<PAGNET_CONFIG_REGRA_BOL>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CONFIG_REGRA_BOL> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODREGRA);

            // Table & Column Mappings
            entity.ToTable("PAGNET_CONFIG_REGRA_BOL");
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


            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                    .WithMany(t => t.PAGNET_CONFIG_REGRA_BOL)
                    .HasForeignKey(d => d.CODEMPRESA);

        }
    }
}