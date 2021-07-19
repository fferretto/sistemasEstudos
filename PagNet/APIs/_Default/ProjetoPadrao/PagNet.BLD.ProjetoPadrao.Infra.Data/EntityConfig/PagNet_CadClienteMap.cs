using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;


namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_CadClienteMap : IEntityTypeConfiguration<PAGNET_CADCLIENTE>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CADCLIENTE> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODCLIENTE);


            // Table & Column Mappings
            entity.ToTable("PAGNET_CADCLIENTE");
            entity.Property(t => t.CODCLIENTE).HasColumnName("CODCLIENTE");
            entity.Property(t => t.NMCLIENTE).HasColumnName("NMCLIENTE");
            entity.Property(t => t.CPFCNPJ).HasColumnName("CPFCNPJ");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.CEP).HasColumnName("CEP");
            entity.Property(t => t.LOGRADOURO).HasColumnName("LOGRADOURO");
            entity.Property(t => t.NROLOGRADOURO).HasColumnName("NROLOGRADOURO");
            entity.Property(t => t.COMPLEMENTO).HasColumnName("COMPLEMENTO");
            entity.Property(t => t.BAIRRO).HasColumnName("BAIRRO");
            entity.Property(t => t.CIDADE).HasColumnName("CIDADE");
            entity.Property(t => t.UF).HasColumnName("UF");
            entity.Property(t => t.COBRANCADIFERENCIADA).HasColumnName("COBRANCADIFERENCIADA");
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
                .WithMany(t => t.PAGNET_CADCLIENTE)
                .HasForeignKey(d => d.CODEMPRESA);

            entity.HasOne(t => t.PAGNET_FORMAS_FATURAMENTO)
                .WithMany(t => t.PAGNET_CADCLIENTE)
                .HasForeignKey(d => d.CODFORMAFATURAMENTO);
        }
    }
}