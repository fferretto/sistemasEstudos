using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PagNet_RelatorioMap : IEntityTypeConfiguration<PAGNET_RELATORIO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_RELATORIO> entity)
        {

            // Primary Key
            entity.HasKey(t => t.ID_REL);


            // Table & Column Mappings
            entity.ToTable("PAGNET_RELATORIO");
            entity.Property(t => t.ID_REL).HasColumnName("ID_REL");
            entity.Property(t => t.DESCRICAO).HasColumnName("DESCRICAO");
            entity.Property(t => t.NOMREL).HasColumnName("NOMREL");
            entity.Property(t => t.TIPREL).HasColumnName("TIPREL");
            entity.Property(t => t.NOMPROC).HasColumnName("NOMPROC");


        }
    }
}
