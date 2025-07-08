using TadesApi.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TadesApi.Db.Entities;


namespace TadesApi.Db.Configuration.AppManagament
{
    public class SysStringResourceConfiguration : IEntityTypeConfiguration<SysStringResource>
    {
        public void Configure(EntityTypeBuilder<SysStringResource> entity)
        {
            entity.ToTable("SysStringResource");


            entity.HasOne(e => e.SysLanguage)
                .WithMany(c => c.SysStringResource)
                .HasForeignKey(s => s.SysLanguageId);


            entity.Property(e => e.Value)
                .HasMaxLength(1024)
                .IsUnicode(true);

            entity.Property(e => e.ResourceName)
            .HasMaxLength(1024)
            .IsUnicode(false);
        }
    }
}
