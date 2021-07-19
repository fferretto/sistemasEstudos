
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_INC_FAVORECIDO_USUARIO_NCMap : IEntityTypeConfiguration<PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC> entity)
        {
            entity.HasKey(pc => new { pc.RETORNO });

        }
    }
}
