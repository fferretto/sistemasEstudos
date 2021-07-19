using PagNet.Bld.AntecipPGTO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.EntityConfig
{
    public class PAGNET_CONFIG_REGRA_PAGMAP : IEntityTypeConfiguration<PAGNET_CONFIG_REGRA_PAG>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CONFIG_REGRA_PAG> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODREGRA);

            // Table & Column Mappings
            entity.ToTable("PAGNET_CONFIG_REGRA_PAG");
            entity.Property(t => t.CODREGRA).HasColumnName("CODREGRA");
            entity.Property(t => t.COBRATAXAANTECIPACAO).HasColumnName("COBRATAXAANTECIPACAO");
            entity.Property(t => t.PERCTAXAANTECIPACAO).HasColumnName("PERCTAXAANTECIPACAO");
            entity.Property(t => t.VLTAXAANTECIPACAO).HasColumnName("VLTAXAANTECIPACAO");

            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");


        }
    }

    public class PAGNET_CONFIG_REGRA_PAG_LOGMAP : IEntityTypeConfiguration<PAGNET_CONFIG_REGRA_PAG_LOG>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CONFIG_REGRA_PAG_LOG> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODREGRA_LOG);

            // Table & Column Mappings
            entity.ToTable("PAGNET_CONFIG_REGRA_PAG_LOG");
            entity.Property(t => t.CODREGRA_LOG).HasColumnName("CODREGRA_LOG");
            entity.Property(t => t.CODREGRA).HasColumnName("CODREGRA");
            entity.Property(t => t.COBRATAXAANTECIPACAO).HasColumnName("COBRATAXAANTECIPACAO");
            entity.Property(t => t.PERCTAXAANTECIPACAO).HasColumnName("PERCTAXAANTECIPACAO");
            entity.Property(t => t.VLTAXAANTECIPACAO).HasColumnName("VLTAXAANTECIPACAO");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.DATINCLOG).HasColumnName("DATINCLOG");
            entity.Property(t => t.DESCLOG).HasColumnName("DESCLOG");



        }
    }
}