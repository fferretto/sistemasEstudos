using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_CONSULTA_TITULOSMap : IEntityTypeConfiguration<PROC_PAGNET_CONSULTA_TITULOS>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_CONSULTA_TITULOS> entity)
        {
            entity.HasKey(pc => new { pc.CODTITULO });

            entity.Property(t => t.CODTITULO).HasColumnName("CODTITULO");
            entity.Property(t => t.CODFAVORECIDO).HasColumnName("CODFAVORECIDO");
            entity.Property(t => t.NMFAVORECIDO).HasColumnName("NMFAVORECIDO");
            entity.Property(t => t.STATUS).HasColumnName("STATUS");
            entity.Property(t => t.CPFCNPJ).HasColumnName("CPFCNPJ");
            entity.Property(t => t.DATEMISSAO).HasColumnName("DATEMISSAO");
            entity.Property(t => t.DATPGTO).HasColumnName("DATPGTO");
            entity.Property(t => t.DATREALPGTO).HasColumnName("DATREALPGTO");
            entity.Property(t => t.BANCO).HasColumnName("BANCO");
            entity.Property(t => t.AGENCIA).HasColumnName("AGENCIA");
            entity.Property(t => t.CONTACORRENTE).HasColumnName("CONTACORRENTE");
            entity.Property(t => t.VALBRUTO).HasColumnName("VALBRUTO");
            entity.Property(t => t.VALTAXA).HasColumnName("VALTAXA");
            entity.Property(t => t.VALLIQ).HasColumnName("VALLIQ");
            entity.Property(t => t.VALTOTAL).HasColumnName("VALTOTAL");
            entity.Property(t => t.TIPCARTAO).HasColumnName("TIPCARTAO");
            entity.Property(t => t.TIPOTITULO).HasColumnName("TIPOTITULO");
            entity.Property(t => t.MSGRETORNO).HasColumnName("MSGRETORNO");
            
        }
    }
}