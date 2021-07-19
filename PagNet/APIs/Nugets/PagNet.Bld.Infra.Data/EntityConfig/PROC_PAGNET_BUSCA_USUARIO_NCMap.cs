using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_BUSCA_USUARIO_NCMap : IEntityTypeConfiguration<PROC_PAGNET_BUSCA_USUARIO_NC>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_BUSCA_USUARIO_NC> entity)
        {
            entity.HasKey(pc => new { pc.CPF });

            entity.Property(t => t.CPF).HasColumnName("CPF");
            entity.Property(t => t.NOMEUSUARIO).HasColumnName("NOMEUSUARIO");
            entity.Property(t => t.BANCO).HasColumnName("BANCO");
            entity.Property(t => t.OPE).HasColumnName("OPE");
            entity.Property(t => t.DVCONTACORRENTE).HasColumnName("DVCONTACORRENTE");
            entity.Property(t => t.DVAGENCIA).HasColumnName("DVAGENCIA");
            entity.Property(t => t.AGENCIA).HasColumnName("AGENCIA");
            entity.Property(t => t.CONTACORRENTE).HasColumnName("CONTACORRENTE");
            entity.Property(t => t.CEP).HasColumnName("CEP");
            entity.Property(t => t.LOGRADOURO).HasColumnName("LOGRADOURO");

            entity.Property(t => t.NROLOGRADOURO).HasColumnName("NROLOGRADOURO");
            entity.Property(t => t.COMPLEMENTO).HasColumnName("COMPLEMENTO");
            entity.Property(t => t.BAIRRO).HasColumnName("BAIRRO");
            entity.Property(t => t.CIDADE).HasColumnName("CIDADE");
            entity.Property(t => t.UF).HasColumnName("UF");

        }
    }
}