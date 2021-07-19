using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;


namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class NetCard_UsuarioPosMap : IEntityTypeConfiguration<NETCARD_USUARIOPOS>
    {
        public void Configure(EntityTypeBuilder<NETCARD_USUARIOPOS> entity)
        {
            // Primary Key
            entity.HasKey(t => new { t.CPF, t.CODCLI, t.NUMDEP });


            // Table & Column Mappings
            entity.ToTable("VUSUARIO");
        }
    }
}
