
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_INC_FAVORECIDO_USUARIO_NCMap : IEntityTypeConfiguration<PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC> entity)
        {
            entity.HasKey(pc => new { pc.RETORNO });

        }
    }
}
