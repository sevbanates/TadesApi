using TadesApi.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Configuration.AppManagament
{
    internal class SysControllerActionTotalConfiguration : IEntityTypeConfiguration<SysControllerActionTotal>
    {
        public void Configure(EntityTypeBuilder<SysControllerActionTotal> entity)
        {
            entity.ToTable("SysControllerActionTotal");

            //entity.HasIndex(e => new { e.Controller }).IsUnique();

            entity.Property(e => e.Controller)
                         .IsRequired()
                         .HasMaxLength(255)
                         .IsUnicode(false);


        }
    }
}
