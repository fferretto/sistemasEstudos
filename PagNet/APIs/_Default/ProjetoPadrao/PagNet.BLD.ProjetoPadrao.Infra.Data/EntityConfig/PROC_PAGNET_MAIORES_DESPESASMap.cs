using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;


namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_MAIORES_DESPESASMap : IEntityTypeConfiguration<PROC_PAGNET_MAIORES_DESPESAS>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_MAIORES_DESPESAS> entity)
        {
            entity.HasKey(pc => new { pc.NOME, pc.ORIGEM });
        }
    }
}