using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;


namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_EXTRATO_BANCARIOMap : IEntityTypeConfiguration<PROC_PAGNET_EXTRATO_BANCARIO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_EXTRATO_BANCARIO> entity)
        {
            entity.HasKey(pc => new { pc.DATA, pc.DESCRICAO });
        }
    }
}