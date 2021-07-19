using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;


namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_Arquivo_ConciliacaoMap : IEntityTypeConfiguration<PAGNET_ARQUIVO_CONCILIACAO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_ARQUIVO_CONCILIACAO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODARQUIVO_CONCILIACAO);

            entity.ToTable("PAGNET_ARQUIVO_CONCILIACAO", "dbo");
            entity.Property(t => t.CODARQUIVO_CONCILIACAO).HasColumnName("CODARQUIVO_CONCILIACAO");
            entity.Property(t => t.CODCLIENTE).HasColumnName("CODCLIENTE");
            entity.Property(t => t.CODFORMAVERIFICACAO).HasColumnName("CODFORMAVERIFICACAO");
            entity.Property(t => t.EXTENSAOARQUI_RET).HasColumnName("EXTENSAOARQUI_RET");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");

            entity.HasOne(t => t.PAGNET_CADCLIENTE)
                .WithMany(t => t.PAGNET_ARQUIVO_CONCILIACAO)
                .HasForeignKey(d => d.CODCLIENTE);


            entity.HasOne(t => t.PAGNET_FORMA_VERIFICACAO_ARQUIVO)
                    .WithMany(t => t.PAGNET_ARQUIVO_CONCILIACAO)
                    .HasForeignKey(d => d.CODFORMAVERIFICACAO);
        }
    }

    public class PagNet_Param_Arquivo_ConciliacaoMap : IEntityTypeConfiguration<PAGNET_PARAM_ARQUIVO_CONCILIACAO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_PARAM_ARQUIVO_CONCILIACAO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODPARAM);

            entity.ToTable("PAGNET_PARAM_ARQUIVO_CONCILIACAO", "dbo");
            entity.Property(t => t.CODPARAM).HasColumnName("CODPARAM");
            entity.Property(t => t.CODARQUIVO_CONCILIACAO).HasColumnName("CODARQUIVO_CONCILIACAO");
            entity.Property(t => t.TIPOARQUIVO).HasColumnName("TIPOARQUIVO");
            entity.Property(t => t.CAMPO).HasColumnName("CAMPO");
            entity.Property(t => t.LINHAINICIO).HasColumnName("LINHAINICIO");
            entity.Property(t => t.POSICAO_CSV).HasColumnName("POSICAO_CSV");
            entity.Property(t => t.POSICAOINI_TXT).HasColumnName("POSICAOINI_TXT");
            entity.Property(t => t.POSICAOFIM_TXT).HasColumnName("POSICAOFIM_TXT");


        entity.HasOne(t => t.PAGNET_ARQUIVO_CONCILIACAO)
                .WithMany(t => t.PAGNET_PARAM_ARQUIVO_CONCILIACAO)
                .HasForeignKey(d => d.CODARQUIVO_CONCILIACAO);
        }
    }

    public class PagNet_Forma_Verificacao_ArquivoMap : IEntityTypeConfiguration<PAGNET_FORMA_VERIFICACAO_ARQUIVO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_FORMA_VERIFICACAO_ARQUIVO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODFORMAVERIFICACAO);

            entity.ToTable("PAGNET_FORMA_VERIFICACAO_ARQUIVO", "dbo");
            entity.Property(t => t.CODFORMAVERIFICACAO).HasColumnName("CODFORMAVERIFICACAO");
            entity.Property(t => t.DESCRICAO).HasColumnName("DESCRICAO");
        }
    }
}
