using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;


namespace PagNet.Infra.Data.EntityConfig
{
    public class PAGNET_ARQUIVO_DESCONTOFOLHAMap : IEntityTypeConfiguration<PAGNET_ARQUIVO_DESCONTOFOLHA>
    {
        public void Configure(EntityTypeBuilder<PAGNET_ARQUIVO_DESCONTOFOLHA> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODARQUIVO_CONCILIACAO);

            entity.ToTable("PAGNET_ARQUIVO_DESCONTOFOLHA", "dbo");
            entity.Property(t => t.CODARQUIVO_CONCILIACAO).HasColumnName("CODARQUIVO_CONCILIACAO");
            entity.Property(t => t.CODCLIENTE).HasColumnName("CODCLIENTE");
            entity.Property(t => t.CODFORMAVERIFICACAO).HasColumnName("CODFORMAVERIFICACAO");
            entity.Property(t => t.EXTENSAOARQUI_RET).HasColumnName("EXTENSAOARQUI_RET");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");

            entity.HasOne(t => t.PAGNET_CADCLIENTE)
                .WithMany(t => t.PAGNET_ARQUIVO_DESCONTOFOLHA)
                .HasForeignKey(d => d.CODCLIENTE);


            entity.HasOne(t => t.PAGNET_FORMA_VERIFICACAO_DF)
                    .WithMany(t => t.PAGNET_ARQUIVO_DESCONTOFOLHA)
                    .HasForeignKey(d => d.CODFORMAVERIFICACAO);
        }
    }

    public class PAGNET_PARAM_ARQUIVO_DESCONTOFOLHAMap : IEntityTypeConfiguration<PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA>
    {
        public void Configure(EntityTypeBuilder<PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODPARAM);

            entity.ToTable("PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA", "dbo");
            entity.Property(t => t.CODPARAM).HasColumnName("CODPARAM");
            entity.Property(t => t.CODARQUIVO_CONCILIACAO).HasColumnName("CODARQUIVO_CONCILIACAO");
            entity.Property(t => t.TIPOARQUIVO).HasColumnName("TIPOARQUIVO");
            entity.Property(t => t.CAMPO).HasColumnName("CAMPO");
            entity.Property(t => t.LINHAINICIO).HasColumnName("LINHAINICIO");
            entity.Property(t => t.POSICAO_CSV).HasColumnName("POSICAO_CSV");
            entity.Property(t => t.POSICAOINI_TXT).HasColumnName("POSICAOINI_TXT");
            entity.Property(t => t.POSICAOFIM_TXT).HasColumnName("POSICAOFIM_TXT");


        entity.HasOne(t => t.PAGNET_ARQUIVO_DESCONTOFOLHA)
                .WithMany(t => t.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA)
                .HasForeignKey(d => d.CODARQUIVO_CONCILIACAO);
        }
    }

    public class PAGNET_FORMA_VERIFICACAO_DFMap : IEntityTypeConfiguration<PAGNET_FORMA_VERIFICACAO_DF>
    {
        public void Configure(EntityTypeBuilder<PAGNET_FORMA_VERIFICACAO_DF> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODFORMAVERIFICACAO);

            entity.ToTable("PAGNET_FORMA_VERIFICACAO_DF", "dbo");
            entity.Property(t => t.CODFORMAVERIFICACAO).HasColumnName("CODFORMAVERIFICACAO");
            entity.Property(t => t.DESCRICAO).HasColumnName("DESCRICAO");
        }
    }
}
