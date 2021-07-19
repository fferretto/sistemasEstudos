using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_Emissao_Titulos_LogMap : IEntityTypeConfiguration<PAGNET_EMISSAO_TITULOS_LOG>
    {
        public void Configure(EntityTypeBuilder<PAGNET_EMISSAO_TITULOS_LOG> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODTITULO_LOG);
            
            // Table & Column Mappings
            entity.ToTable("PAGNET_EMISSAO_TITULOS_LOG");
            entity.Property(t => t.CODTITULO).HasColumnName("CODTITULO");
            entity.Property(t => t.CODTITULOPAI).HasColumnName("CODTITULOPAI");
            entity.Property(t => t.STATUS).HasColumnName("STATUS");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.CODFAVORECIDO).HasColumnName("CODFAVORECIDO");
            entity.Property(t => t.DATEMISSAO).HasColumnName("DATEMISSAO");
            entity.Property(t => t.DATPGTO).HasColumnName("DATPGTO");
            entity.Property(t => t.DATREALPGTO).HasColumnName("DATREALPGTO");
            entity.Property(t => t.VALBRUTO).HasColumnName("VALBRUTO");
            entity.Property(t => t.VALLIQ).HasColumnName("VALLIQ");
            entity.Property(t => t.TIPOTITULO).HasColumnName("TIPOTITULO");
            entity.Property(t => t.ORIGEM).HasColumnName("ORIGEM");
            entity.Property(t => t.SISTEMA).HasColumnName("SISTEMA");
            entity.Property(t => t.LINHADIGITAVEL).HasColumnName("LINHADIGITAVEL");
            entity.Property(t => t.CODBORDERO).HasColumnName("CODBORDERO");
            entity.Property(t => t.VALTOTAL).HasColumnName("VALTOTAL");
            entity.Property(t => t.SEUNUMERO).HasColumnName("SEUNUMERO");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.DATINCLOG).HasColumnName("DATINCLOG");
            entity.Property(t => t.DESCLOG).HasColumnName("DESCLOG");


            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                    .WithMany(t => t.PAGNET_EMISSAO_TITULOS_LOG)
                    .HasForeignKey(d => d.CODEMPRESA);

            entity.HasOne(t => t.PAGNET_CADFAVORECIDO)
                    .WithMany(t => t.PAGNET_EMISSAO_TITULOS_LOG)
                    .HasForeignKey(d => d.CODFAVORECIDO);

            entity.HasOne(t => t.USUARIO_NETCARD)
                    .WithMany(t => t.PAGNET_EMISSAO_TITULOS_LOG)
                    .HasForeignKey(d => d.CODUSUARIO);

        }
    }
}