using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_LIBERA_DESCONTOFOLHAMap : IEntityTypeConfiguration<PROC_PAGNET_LIBERA_DESCONTOFOLHA>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_LIBERA_DESCONTOFOLHA> entity)
        {

            entity.HasKey(pc => new { pc.CODIGOERRO });

            entity.Property(t => t.CODIGOERRO).HasColumnName("CODIGOERRO");
            entity.Property(t => t.RESULTADO).HasColumnName("RESULTADO");
            entity.Property(t => t.MENSAGEMRETORNO).HasColumnName("MENSAGEMRETORNO");

        }
    }
}