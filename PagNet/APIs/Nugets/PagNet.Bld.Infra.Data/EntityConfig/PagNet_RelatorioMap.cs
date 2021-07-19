using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
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

    public class PAGNET_RELATORIO_STATUSMap : IEntityTypeConfiguration<PAGNET_RELATORIO_STATUS>
    {
        public void Configure(EntityTypeBuilder<PAGNET_RELATORIO_STATUS> entity)
        {
            // Primary Key
            entity.HasKey(t => t.COD_STATUS_REL);

            // Table & Column Mappings
            entity.ToTable("PAGNET_RELATORIO_STATUS");
            entity.Property(t => t.COD_STATUS_REL).HasColumnName("COD_STATUS_REL");
            entity.Property(t => t.ID_REL).HasColumnName("ID_REL");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.STATUS).HasColumnName("STATUS");
            entity.Property(t => t.TIPORETORNO).HasColumnName("TIPORETORNO");
            entity.Property(t => t.DATEMISSAO).HasColumnName("DATEMISSAO");
            entity.Property(t => t.ERRO).HasColumnName("ERRO");
            entity.Property(t => t.MSG_ERRO).HasColumnName("MSG_ERRO");


            entity.HasOne(t => t.PAGNET_RELATORIO)
                        .WithMany(t => t.PAGNET_RELATORIO_STATUS)
                        .HasForeignKey(d => d.ID_REL)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PAGNET_RELATORIO_RESULTADOMap : IEntityTypeConfiguration<PAGNET_RELATORIO_RESULTADO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_RELATORIO_RESULTADO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.COD_RESULTADO);

            // Table & Column Mappings
            entity.ToTable("PAGNET_RELATORIO_RESULTADO");
            entity.Property(t => t.COD_STATUS_REL).HasColumnName("COD_STATUS_REL");
            entity.Property(t => t.COD_RESULTADO).HasColumnName("COD_RESULTADO");
            entity.Property(t => t.LINHAIMP).HasColumnName("LINHAIMP");
            entity.Property(t => t.TIP).HasColumnName("TIP");

            entity.HasOne(t => t.PAGNET_RELATORIO_STATUS)
                    .WithMany(t => t.PAGNET_RELATORIO_RESULTADO)
                    .HasForeignKey(d => d.COD_STATUS_REL)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PAGNET_RELATORIO_PARAM_UTILIZADOMap : IEntityTypeConfiguration<PAGNET_RELATORIO_PARAM_UTILIZADO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_RELATORIO_PARAM_UTILIZADO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.COD_PARAM_UTILIZADO);

            // Table & Column Mappings
            entity.ToTable("PAGNET_RELATORIO_PARAM_UTILIZADO");
            entity.Property(t => t.COD_STATUS_REL).HasColumnName("COD_STATUS_REL");
            entity.Property(t => t.COD_PARAM_UTILIZADO).HasColumnName("COD_PARAM_UTILIZADO");
            entity.Property(t => t.NOMPAR).HasColumnName("NOMPAR");
            entity.Property(t => t.CONTEUDO).HasColumnName("CONTEUDO");

            entity.HasOne(t => t.PAGNET_RELATORIO_STATUS)
                                .WithMany(t => t.PAGNET_RELATORIO_PARAM_UTILIZADO)
                                .HasForeignKey(d => d.COD_STATUS_REL)
                                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
