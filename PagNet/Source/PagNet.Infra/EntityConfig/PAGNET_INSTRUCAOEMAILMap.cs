using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PAGNET_INSTRUCAOEMAILMap : IEntityTypeConfiguration<PAGNET_INSTRUCAOEMAIL>
    {
        public void Configure(EntityTypeBuilder<PAGNET_INSTRUCAOEMAIL> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODINSTRUCAOEMAIL);

            // Hack pois o método ValueGeneratedOnAdd do EF Core ainda não aceita propriedades decimal
            entity.Property(i => i.CODINSTRUCAOEMAIL).HasDefaultValueSql("SELECT MAX(CODINSTRUCAOEMAIL) + 1  FROM PAGNET_INSTRUCAOEMAIL").HasColumnName("CODINSTRUCAOEMAIL");

            entity.Property(f => f.CODINSTRUCAOEMAIL).ValueGeneratedOnAdd();

            // Table & Column Mappings
            entity.ToTable("PAGNET_INSTRUCAOEMAIL");
            entity.Property(t => t.CODINSTRUCAOEMAIL).HasColumnName("CODINSTRUCAOEMAIL");
            entity.Property(t => t.ASSUNTO).HasColumnName("ASSUNTO");
            entity.Property(t => t.MENSAGEM).HasColumnName("MENSAGEM");

        }
    }
}
