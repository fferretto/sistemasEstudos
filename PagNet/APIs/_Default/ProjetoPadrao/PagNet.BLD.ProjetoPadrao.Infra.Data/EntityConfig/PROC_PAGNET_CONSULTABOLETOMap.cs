using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_CONSULTABOLETOMap : IEntityTypeConfiguration<PROC_PAGNET_CONSULTABOLETO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_CONSULTABOLETO> entity)
        {

            entity.HasKey(pc => new { pc.CODEMISSAOBOLETO });

        }
    }
}