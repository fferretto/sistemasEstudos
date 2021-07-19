using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;


namespace PagNet.Infra.Data.EntityConfig
{
    public class PagNet_CadCliente_LogMap : IEntityTypeConfiguration<PAGNET_CADCLIENTE_LOG>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CADCLIENTE_LOG> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODCLIENTE_LOG);


            // Table & Column Mappings
            entity.ToTable("PAGNET_CADCLIENTE_LOG");
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
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.DATINCLOG).HasColumnName("DATINCLOG");
            entity.Property(t => t.DESCLOG).HasColumnName("DESCLOG");
        }
    }
}