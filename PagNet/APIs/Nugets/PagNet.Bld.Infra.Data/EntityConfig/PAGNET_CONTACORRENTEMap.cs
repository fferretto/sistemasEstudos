using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PAGNET_CONTACORRENTEMap : IEntityTypeConfiguration<PAGNET_CONTACORRENTE>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CONTACORRENTE> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODCONTACORRENTE);

            // Properties
            entity.Property(t => t.NMCONTACORRENTE)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(t => t.CODBANCO)
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(t => t.NROCONTACORRENTE)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(t => t.DIGITOCC)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            entity.Property(t => t.CONTAMOVIEMNTO)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(t => t.AGENCIA)
                .IsRequired()
                .HasMaxLength(6);

            entity.Property(t => t.DIGITOAGENCIA)
                .IsRequired()
                .HasMaxLength(1);

            entity.Property(t => t.CODCEDENTE)
                .HasMaxLength(20);

            entity.Property(t => t.CODCONVENIOPAG)
                .HasMaxLength(20);


            entity.Property(t => t.NMEMPRESA)
                .HasMaxLength(150);

            entity.Property(t => t.CPFCNPJ)
                .HasMaxLength(14);

            entity.Property(t => t.ATIVO)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            entity.ToTable("PAGNET_CONTACORRENTE");
            entity.Property(t => t.CODCONTACORRENTE).HasColumnName("CODCONTACORRENTE");
            entity.Property(t => t.NMCONTACORRENTE).HasColumnName("NMCONTACORRENTE");
            entity.Property(t => t.NMEMPRESA).HasColumnName("NMEMPRESA");
            entity.Property(t => t.CPFCNPJ).HasColumnName("CPFCNPJ");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.NROCONTACORRENTE).HasColumnName("NROCONTACORRENTE");
            entity.Property(t => t.DIGITOCC).HasColumnName("DIGITOCC");
            entity.Property(t => t.CONTAMOVIEMNTO).HasColumnName("CONTAMOVIEMNTO");
            entity.Property(t => t.AGENCIA).HasColumnName("AGENCIA");
            entity.Property(t => t.DIGITOAGENCIA).HasColumnName("DIGITOAGENCIA");
            entity.Property(t => t.CODCEDENTE).HasColumnName("CODCEDENTE");
            entity.Property(t => t.CODCONVENIOPAG).HasColumnName("CODCONVENIOPAG");
            entity.Property(t => t.CODTRANSMISSAO).HasColumnName("CODTRANSMISSAO");
            entity.Property(t => t.CARTEIRAREMESSA).HasColumnName("CARTEIRAREMESSA");
            entity.Property(t => t.VARIACAOCARTEIRA).HasColumnName("VARIACAOCARTEIRA");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");
            entity.Property(t => t.VALTED).HasColumnName("VALTED");
            entity.Property(t => t.COBRAJUROS).HasColumnName("COBRAJUROS");
            entity.Property(t => t.VLJUROSDIAATRASO).HasColumnName("VLJUROSDIAATRASO");
            entity.Property(t => t.PERCJUROS).HasColumnName("PERCJUROS");
            entity.Property(t => t.COBRAMULTA).HasColumnName("COBRAMULTA");
            entity.Property(t => t.VLMULTADIAATRASO).HasColumnName("VLMULTADIAATRASO");
            entity.Property(t => t.PERCMULTA).HasColumnName("PERCMULTA");
            entity.Property(t => t.CODPRIMEIRAINSTCOBRA).HasColumnName("CODPRIMEIRAINSTCOBRA");
            entity.Property(t => t.CODSEGUNDAINSTCOBRA).HasColumnName("CODSEGUNDAINSTCOBRA");
            entity.Property(t => t.TAXAEMISSAOBOLETO).HasColumnName("TAXAEMISSAOBOLETO");
            entity.Property(t => t.QTPOSICAOARQPGTO).HasColumnName("QTPOSICAOARQPGTO");
            entity.Property(t => t.QTPOSICAOARQBOL).HasColumnName("QTPOSICAOARQBOL");

            entity.Property(t => t.CODOPERACAOCC).HasColumnName("CODOPERACAOCC");


            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                .WithMany(t => t.PAGNET_CONTACORRENTE)
                .HasForeignKey(d => d.CODEMPRESA);

        }
    }
}
