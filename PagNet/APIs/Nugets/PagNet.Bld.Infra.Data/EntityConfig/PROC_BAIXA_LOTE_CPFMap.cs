using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PROC_BAIXA_LOTE_CPFMap : IEntityTypeConfiguration<PROC_BAIXA_LOTE_CPF>
    {
        public void Configure(EntityTypeBuilder<PROC_BAIXA_LOTE_CPF> entity)
        {
            entity.HasKey(pc => new { pc.CODRET });

        }
    }
}
