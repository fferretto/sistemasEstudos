using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
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

            entity.Property(t => t.CODCONVENIOBOL)
                .HasMaxLength(20);

            entity.Property(t => t.CODCONVENIOPAG)
                .HasMaxLength(20);

            entity.Property(t => t.CODTRASMISSAO240)
                .HasMaxLength(15);

            entity.Property(t => t.CODTRANSMISSAO400)
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
            entity.Property(t => t.CODCONVENIOBOL).HasColumnName("CODCONVENIOBOL");
            entity.Property(t => t.CODCONVENIOPAG).HasColumnName("CODCONVENIOPAG");
            entity.Property(t => t.CODTRASMISSAO240).HasColumnName("CODTRASMISSAO240");
            entity.Property(t => t.CODTRANSMISSAO400).HasColumnName("CODTRANSMISSAO400");
            entity.Property(t => t.CARTEIRAREMESSA).HasColumnName("CARTEIRAREMESSA");
            entity.Property(t => t.CARTEIRABOL).HasColumnName("CARTEIRABOL");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");
            entity.Property(t => t.VALTED).HasColumnName("VALTED");            

            entity.Property(t => t.TRANSMITIRARQAUTO).HasColumnName("TRANSMITIRARQAUTO");
            entity.Property(t => t.CODOPERACAOCC).HasColumnName("CODOPERACAOCC");

        }
    }
}
