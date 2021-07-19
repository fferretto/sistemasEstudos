using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PAGNET_EMISSAOBOLETOMap : IEntityTypeConfiguration<PAGNET_EMISSAOBOLETO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_EMISSAOBOLETO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.codEmissaoBoleto);

            // Properties
            entity.Property(t => t.Status)
                .HasMaxLength(50);


            // Table & Column Mappings
            entity.ToTable("PAGNET_EMISSAOBOLETO");
            entity.Property(t => t.codEmissaoBoleto).HasColumnName("CODEMISSAOBOLETO");
            entity.Property(t => t.Status).HasColumnName("STATUS");
            entity.Property(t => t.CodCliente).HasColumnName("CODCLIENTE");
            entity.Property(t => t.codEmpresa).HasColumnName("CODEMPRESA");
            entity.Property(t => t.dtVencimento).HasColumnName("DATVENCIMENTO");
            entity.Property(t => t.NossoNumero).HasColumnName("NOSSONUMERO");
            entity.Property(t => t.codOcorrencia).HasColumnName("CODOCORRENCIA");
            entity.Property(t => t.SeuNumero).HasColumnName("SEUNUMERO");
            entity.Property(t => t.Valor).HasColumnName("VALOR");
            entity.Property(t => t.dtSolicitacao).HasColumnName("DATSOLICITACAO");
            entity.Property(t => t.dtReferencia).HasColumnName("DATREFERENCIA");
            entity.Property(t => t.dtSegundoDesconto).HasColumnName("DATSEGUNDODESCONTO");
            entity.Property(t => t.vlDesconto).HasColumnName("VLDESCONTO");
            entity.Property(t => t.vlSegundoDesconto).HasColumnName("VLSEGUNDODESCONTO");
            entity.Property(t => t.MensagemArquivoRemessa).HasColumnName("MENSAGEMARQUIVOREMESSA");
            entity.Property(t => t.MensagemInstrucoesCaixa).HasColumnName("MENSAGEMINSTRUCOESCAIXA");
            entity.Property(t => t.numControle).HasColumnName("NUMCONTROLE");
            entity.Property(t => t.OcorrenciaRetBol).HasColumnName("OCORRENCIARETBOL");
            entity.Property(t => t.nmBoletoGerado).HasColumnName("NMBOLETOGERADO");
            entity.Property(t => t.DescricaoOcorrenciaRetBol).HasColumnName("DESCRICAOOCORRENCIARETBOL");




        // Relationships

            entity.HasOne(t => t.PAGNET_CADCLIENTE)
                .WithMany(t => t.PAGNET_EMISSAOBOLETO)
                .HasForeignKey(p =>  p.CodCliente);

            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                .WithMany(t => t.PAGNET_EMISSAOBOLETO)
                .HasForeignKey(d => d.codEmpresa);

            entity.HasOne(t => t.PAGNET_CONTACORRENTE)
                .WithMany(t => t.PAGNET_EMISSAOBOLETO)
                .HasForeignKey(d => d.codContaCorrente);
        }
    }
}