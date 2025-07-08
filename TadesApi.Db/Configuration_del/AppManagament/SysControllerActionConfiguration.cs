using TadesApi.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Configuration.AppManagament
{
    public class SysControllerActionConfiguration : IEntityTypeConfiguration<SysControllerAction>
    {
        public void Configure(EntityTypeBuilder<SysControllerAction> entity)
        {

            entity.ToTable("SysControllerAction");


            //entity.HasIndex(p => new { p.ActionNo, p.ActionName }).IsUnique();

            entity.Property(e => e.ActionNo)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.Property(e => e.ControllerName)
              .IsRequired()
              .HasMaxLength(255)
              .IsUnicode(false);


            entity.Property(e => e.ActionName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);


        }
    }


}
