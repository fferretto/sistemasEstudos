using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PAGNET_FORMAS_FATURAMENTOMap : IEntityTypeConfiguration<PAGNET_FORMAS_FATURAMENTO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_FORMAS_FATURAMENTO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODFORMAFATURAMENTO);

            // Table & Column Mappings
            entity.ToTable("PAGNET_FORMAS_FATURAMENTO");

        }
    }

}