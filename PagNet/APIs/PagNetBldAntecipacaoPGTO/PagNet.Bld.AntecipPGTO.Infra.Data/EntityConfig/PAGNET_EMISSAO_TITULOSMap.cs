using PagNet.Bld.AntecipPGTO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.EntityConfig
{
    public class PAGNET_EMISSAO_TITULOSMAP : IEntityTypeConfiguration<PAGNET_EMISSAO_TITULOS>
    {
        public void Configure(EntityTypeBuilder<PAGNET_EMISSAO_TITULOS> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODTITULO);

            // Table & Column Mappings
            entity.ToTable("PAGNET_EMISSAO_TITULOS");
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


            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                    .WithMany(t => t.PAGNET_EMISSAO_TITULOS)
                    .HasForeignKey(d => d.CODEMPRESA);

            entity.HasOne(t => t.PAGNET_CADFAVORECIDO)
                    .WithMany(t => t.PAGNET_EMISSAO_TITULOS)
                    .HasForeignKey(d => d.CODFAVORECIDO);

        }
    }

    public class PAGNET_EMISSAO_TITULOS_LOGMAP : IEntityTypeConfiguration<PAGNET_EMISSAO_TITULOS_LOG>
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

            entity.HasOne(t => t.PAGNET_USUARIO)
                    .WithMany(t => t.PAGNET_EMISSAO_TITULOS_LOG)
                    .HasForeignKey(d => d.CODUSUARIO);

        }
    }
}