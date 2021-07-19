using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class OPERADORAMap : IEntityTypeConfiguration<OPERADORA>
    {
        public void Configure(EntityTypeBuilder<OPERADORA> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODOPE);
            
            entity.Property(t => t.NOME)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(t => t.BD_AUT)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(t => t.BD_NC)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(t => t.FLAG_VA)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            entity.Property(t => t.FLAG_TESTE)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            entity.Property(t => t.SERVIDOR)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(t => t.DESC_PREFEITURA)
                .IsFixedLength()
                .HasMaxLength(1);

            entity.Property(t => t.TIPO_CALC)
                .IsFixedLength()
                .HasMaxLength(1);

            entity.Property(t => t.DESC_CANC)
                .IsFixedLength()
                .HasMaxLength(1);

            entity.Property(t => t.SERVIDOR_AUT)
                .HasMaxLength(30);

            entity.Property(t => t.EXIBE_GRAFICO)
                .IsFixedLength()
                .HasMaxLength(1);

            entity.Property(t => t.OPERADORA1)
                .HasMaxLength(128);

            entity.Property(t => t.SERVIDOR_IIS)
                .HasMaxLength(50);

            entity.Property(t => t.NOMOPERAFIL)
                .HasMaxLength(30);

            entity.Property(t => t.CAMINHOARQUIVO)
                .HasMaxLength(250);

            entity.Property(t => t.CAMINHO_ARQ_FECH_CLI)
                .HasMaxLength(100);

            entity.Property(t => t.CAMINHO_ARQ_FECH_CRE)
                .HasMaxLength(100);

            // Table & Column Mappings
            entity.ToTable("OPERADORA");
            entity.Property(t => t.CODOPE).HasColumnName("CODOPE");
            entity.Property(t => t.NOME).HasColumnName("NOME");
            entity.Property(t => t.BD_AUT).HasColumnName("BD_AUT");
            entity.Property(t => t.BD_NC).HasColumnName("BD_NC");
            entity.Property(t => t.FLAG_VA).HasColumnName("FLAG_VA");
            entity.Property(t => t.FLAG_TESTE).HasColumnName("FLAG_TESTE");
            entity.Property(t => t.SERVIDOR).HasColumnName("SERVIDOR");
            entity.Property(t => t.DESC_PREFEITURA).HasColumnName("DESC_PREFEITURA");
            entity.Property(t => t.REAJUSTE).HasColumnName("REAJUSTE");
            entity.Property(t => t.DIA_VENCIMENTO).HasColumnName("DIA_VENCIMENTO");
            entity.Property(t => t.MINIMO).HasColumnName("MINIMO");
            entity.Property(t => t.TIPO_CALC).HasColumnName("TIPO_CALC");
            entity.Property(t => t.VALOR_UNITARIO).HasColumnName("VALOR_UNITARIO");
            entity.Property(t => t.VALOR_CNP).HasColumnName("VALOR_CNP");
            entity.Property(t => t.DESCNPB).HasColumnName("DESCNPB");
            entity.Property(t => t.DESC_CANC).HasColumnName("DESC_CANC");
            entity.Property(t => t.LICENCA).HasColumnName("LICENCA");
            entity.Property(t => t.SERVIDOR_AUT).HasColumnName("SERVIDOR_AUT");
            entity.Property(t => t.EXIBE_GRAFICO).HasColumnName("EXIBE_GRAFICO");
            entity.Property(t => t.OPERADORA1).HasColumnName("OPERADORA");
            entity.Property(t => t.SERVIDOR_IIS).HasColumnName("SERVIDOR_IIS");
            entity.Property(t => t.NOMOPERAFIL).HasColumnName("NOMOPERAFIL");
            entity.Property(t => t.CAMINHOARQUIVO).HasColumnName("CAMINHOARQUIVO");
            entity.Property(t => t.CAMINHO_ARQ_FECH_CLI).HasColumnName("CAMINHO_ARQ_FECH_CLI");
            entity.Property(t => t.CAMINHO_ARQ_FECH_CRE).HasColumnName("CAMINHO_ARQ_FECH_CRE");
        }
    }
}
