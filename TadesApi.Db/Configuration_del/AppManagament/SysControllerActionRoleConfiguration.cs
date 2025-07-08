using TadesApi.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Configuration.AppManagament
{
    public class SysControllerActionRoleConfiguration : IEntityTypeConfiguration<SysControllerActionRole>
    {
        public void Configure(EntityTypeBuilder<SysControllerActionRole> entity)
        {
            entity.ToTable("SysControllerActionRole");

            entity.HasIndex(e => new { e.RoleId, e.ActionName, e.Controller }).IsUnique();
            entity.Property(e => e.ActionName)
                        .HasMaxLength(255)
                        .IsRequired();
            entity.Property(e => e.Controller)
                   .HasMaxLength(255)
                   .IsRequired();



        }
    }
}
