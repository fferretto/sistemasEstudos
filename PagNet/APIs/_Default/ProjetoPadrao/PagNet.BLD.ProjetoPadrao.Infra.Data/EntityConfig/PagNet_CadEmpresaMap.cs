using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_CadEmpresaMap : IEntityTypeConfiguration<PAGNET_CADEMPRESA>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CADEMPRESA> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODEMPRESA);


            // Table & Column Mappings
            entity.ToTable("PAGNET_CADEMPRESA");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.RAZAOSOCIAL).HasColumnName("RAZAOSOCIAL");
            entity.Property(t => t.NMFANTASIA).HasColumnName("NMFANTASIA");
            entity.Property(t => t.CNPJ).HasColumnName("CNPJ");
            entity.Property(t => t.CEP).HasColumnName("CEP");
            entity.Property(t => t.LOGRADOURO).HasColumnName("LOGRADOURO");
            entity.Property(t => t.NROLOGRADOURO).HasColumnName("NROLOGRADOURO");
            entity.Property(t => t.COMPLEMENTO).HasColumnName("COMPLEMENTO");
            entity.Property(t => t.BAIRRO).HasColumnName("BAIRRO");
            entity.Property(t => t.CIDADE).HasColumnName("CIDADE");
            entity.Property(t => t.UF).HasColumnName("UF");
            entity.Property(t => t.UTILIZANETCARD).HasColumnName("UTILIZANETCARD");
            entity.Property(t => t.CODSUBREDE).HasColumnName("CODSUBREDE");
            entity.Property(t => t.NMLOGIN).HasColumnName("NMLOGIN");

        }
    }
}