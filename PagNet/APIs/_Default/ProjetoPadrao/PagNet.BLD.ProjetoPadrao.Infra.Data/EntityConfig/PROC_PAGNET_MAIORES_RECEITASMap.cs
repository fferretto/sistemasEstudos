using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;


namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_MAIORES_RECEITASMap : IEntityTypeConfiguration<PROC_PAGNET_MAIORES_RECEITAS>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_MAIORES_RECEITAS> entity)
        {
            entity.HasKey(pc => new { pc.NOME, pc.ORIGEM });
        }
    }
}