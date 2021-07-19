using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PagNet_Titulos_PagosMap : IEntityTypeConfiguration<PAGNET_TITULOS_PAGOS>
    {
        public void Configure(EntityTypeBuilder<PAGNET_TITULOS_PAGOS> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODTITULOPAGO);


            // Table & Column Mappings
            entity.ToTable("PAGNET_TITULOS_PAGOS");
            entity.Property(t => t.CODTITULOPAGO).HasColumnName("CODTITULOPAGO");
            entity.Property(t => t.STATUS).HasColumnName("STATUS");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.CODCONTACORRENTE).HasColumnName("CODCONTACORRENTE");
            entity.Property(t => t.CODBORDERO).HasColumnName("CODBORDERO");
            entity.Property(t => t.CODFAVORECIDO).HasColumnName("CODFAVORECIDO");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.TIPOSERVICO).HasColumnName("TIPOSERVICO");
            entity.Property(t => t.CODFORMALANCAMENTO).HasColumnName("CODFORMALANCAMENTO");
            entity.Property(t => t.SEUNUMERO).HasColumnName("SEUNUMERO");
            entity.Property(t => t.NOSSONUMERO).HasColumnName("NOSSONUMERO");
            entity.Property(t => t.DTPAGAMENTO).HasColumnName("DTPAGAMENTO");
            entity.Property(t => t.DTREALPAGAMENTO).HasColumnName("DTREALPAGAMENTO");
            entity.Property(t => t.DTVENCIMENTO).HasColumnName("DTVENCIMENTO");
            entity.Property(t => t.VALOR).HasColumnName("VALOR");
            entity.Property(t => t.CODARQUIVO).HasColumnName("CODARQUIVO");
            entity.Property(t => t.OCORRENCIARETORNO).HasColumnName("OCORRENCIARETORNO");



            entity.HasOne(t => t.PAGNET_BORDERO_PAGAMENTO)
                    .WithMany(t => t.PAGNET_TITULOS_PAGOS)
                    .HasForeignKey(d => d.CODBORDERO);

            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                    .WithMany(t => t.PAGNET_TITULOS_PAGOS)
                    .HasForeignKey(d => d.CODEMPRESA);

            entity.HasOne(t => t.PAGNET_CADFAVORECIDO)
                    .WithMany(t => t.PAGNET_TITULOS_PAGOS)
                    .HasForeignKey(d => d.CODFAVORECIDO);

            entity.HasOne(t => t.PAGNET_CONTACORRENTE)
                    .WithMany(t => t.PAGNET_TITULOS_PAGOS)
                    .HasForeignKey(d => d.CODCONTACORRENTE);

            entity.HasOne(t => t.PAGNET_ARQUIVO)
                    .WithMany(t => t.PAGNET_TITULOS_PAGOS)
                    .HasForeignKey(d => d.CODARQUIVO);

        }
    }
}
