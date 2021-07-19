using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_CadFavorecidosMap : IEntityTypeConfiguration<PAGNET_CADFAVORECIDO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CADFAVORECIDO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODFAVORECIDO);


            // Table & Column Mappings
            entity.ToTable("PAGNET_CADFAVORECIDO");
            entity.Property(t => t.CODFAVORECIDO).HasColumnName("CODFAVORECIDO");
            entity.Property(t => t.NMFAVORECIDO).HasColumnName("NMFAVORECIDO");
            entity.Property(t => t.CPFCNPJ).HasColumnName("CPFCNPJ");
            entity.Property(t => t.CODCEN).HasColumnName("CODCEN");
            entity.Property(t => t.BANCO).HasColumnName("BANCO");
            entity.Property(t => t.AGENCIA).HasColumnName("AGENCIA");
            entity.Property(t => t.DVAGENCIA).HasColumnName("DVAGENCIA");
            entity.Property(t => t.CONTACORRENTE).HasColumnName("CONTACORRENTE");
            entity.Property(t => t.DVCONTACORRENTE).HasColumnName("DVCONTACORRENTE");
            entity.Property(t => t.CEP).HasColumnName("CEP");
            entity.Property(t => t.LOGRADOURO).HasColumnName("LOGRADOURO");
            entity.Property(t => t.NROLOGRADOURO).HasColumnName("NROLOGRADOURO");
            entity.Property(t => t.COMPLEMENTO).HasColumnName("COMPLEMENTO");
            entity.Property(t => t.BAIRRO).HasColumnName("BAIRRO");
            entity.Property(t => t.CIDADE).HasColumnName("CIDADE");
            entity.Property(t => t.UF).HasColumnName("UF");
            entity.Property(t => t.REGRADIFERENCIADA).HasColumnName("REGRADIFERENCIADA");
            entity.Property(t => t.VALMINIMOCC).HasColumnName("VALMINIMOCC");            
            entity.Property(t => t.VALTED).HasColumnName("VALTED");
            entity.Property(t => t.VALMINIMOTED).HasColumnName("VALMINIMOTED");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");
            entity.Property(t => t.CODCONTACORRENTE).HasColumnName("CODCONTACORRENTE");


            entity.HasOne(t => t.PAGNET_CONTACORRENTE)
                .WithMany(t => t.PAGNET_CADFAVORECIDO)
                .HasForeignKey(d => d.CODCONTACORRENTE);

        }
    }
}