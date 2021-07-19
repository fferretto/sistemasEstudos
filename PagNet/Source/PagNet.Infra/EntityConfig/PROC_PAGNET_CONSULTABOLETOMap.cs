using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_CONSULTABOLETOMap : IEntityTypeConfiguration<PROC_PAGNET_CONSULTABOLETO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_CONSULTABOLETO> entity)
        {

            entity.HasKey(pc => new { pc.CODEMISSAOBOLETO });

        }
    }
}