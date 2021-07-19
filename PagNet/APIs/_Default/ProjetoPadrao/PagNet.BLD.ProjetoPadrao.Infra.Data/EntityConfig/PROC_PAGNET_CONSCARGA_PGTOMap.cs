using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_CONSCARGA_PGTOMap : IEntityTypeConfiguration<PROC_PAGNET_CONSCARGA_PGTO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_CONSCARGA_PGTO> entity)
        {
            entity.HasKey(pc => new { pc.CODCLI, pc.NUMCARGA, pc.DTAUTORIZ });

            entity.Property(t => t.CODCLI).HasColumnName("CODCLI");
            entity.Property(t => t.CNPJ).HasColumnName("CNPJ");
            entity.Property(t => t.NMCLIENTE).HasColumnName("NMCLIENTE");
            entity.Property(t => t.NUMCARGA).HasColumnName("NUMCARGA");
            entity.Property(t => t.DTAUTORIZ).HasColumnName("DTAUTORIZ");
            entity.Property(t => t.DTCARGA).HasColumnName("DTCARGA");
            entity.Property(t => t.QUANT).HasColumnName("QUANT");
            entity.Property(t => t.VALCARGA).HasColumnName("VALCARGA");
            entity.Property(t => t.VAL2AVIA).HasColumnName("VAL2AVIA");
            entity.Property(t => t.VALTAXA).HasColumnName("VALTAXA");
            entity.Property(t => t.VALTAXACARREG).HasColumnName("VALTAXACARREG");
            entity.Property(t => t.TOTAL).HasColumnName("TOTAL");
            entity.Property(t => t.SALDOCONTAUTIL).HasColumnName("SALDOCONTAUTIL");
            entity.Property(t => t.DESCRICAOOCORRENCIARETBOL).HasColumnName("DESCRICAOOCORRENCIARETBOL");

        }
    }
}
