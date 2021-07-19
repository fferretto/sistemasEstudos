using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_REG_CARGA_CLI_NETCARDMap : IEntityTypeConfiguration<PROC_PAGNET_REG_CARGA_CLI_NETCARD>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_REG_CARGA_CLI_NETCARD> entity)
        {

            entity.HasKey(pc => new { pc.CODEMISSAOBOLETO });

        }
    }
}