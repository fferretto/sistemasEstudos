using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;


namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class NetCard_UsuarioPreMap : IEntityTypeConfiguration<NETCARD_USUARIOPRE>
    {
        public void Configure(EntityTypeBuilder<NETCARD_USUARIOPRE> entity)
        {
            // Primary Key
            entity.HasKey(t => new { t.CPF, t.NUMDEP, t.CODCLI });


            // Table & Column Mappings
            entity.ToTable("VUSUARIOVA");
        }
        }
}
