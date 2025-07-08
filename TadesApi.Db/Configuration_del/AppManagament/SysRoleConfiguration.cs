using TadesApi.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Configuration.AppManagament
{
    public class SysRoleConfiguration : IEntityTypeConfiguration<SysRole>
    {
        public void Configure(EntityTypeBuilder<SysRole> entity)
        {

            entity.ToTable("SysRole");
            entity.HasKey(p => new { p.Id });

            entity.Property(e => e.RoleDescription).HasMaxLength(255);

            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(255);


        }
    }
}
