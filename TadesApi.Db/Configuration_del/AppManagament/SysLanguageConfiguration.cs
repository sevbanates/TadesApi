using TadesApi.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Configuration.AppManagament
{
    public class SysLanguageConfiguration : IEntityTypeConfiguration<SysLanguage>
    {
        public void Configure(EntityTypeBuilder<SysLanguage> entity)
        {
            entity.ToTable("SysLanguage");


            entity.HasIndex(p => new { p.Culture }).IsUnique();

            entity.HasMany(c => c.SysStringResource)
                 .WithOne(e => e.SysLanguage)
                  .HasForeignKey(s => s.SysLanguageId);



            entity.Property(e => e.Name)
                 .IsRequired()
                 .HasMaxLength(255)
                 .IsUnicode(false);

            entity.Property(e => e.Culture)
           .IsRequired()
           .HasMaxLength(255)
           .IsUnicode(false);





            //******************************
            //    AdSeedData(entity);

            //}

            //private void AdSeedData(EntityTypeBuilder<SysLanguage> entity)
            //{
            //    //****************************** Set  Data ************************
            //    entity.HasData(
            //       new SysLanguage { Id = 1, Culture = "en_US", Name = "en_US" },
            //       //new SysRole {Id=2, RoleKey = 100, RoleName = "Admin", RoleDescr2 = "Admin" },
            //       new SysLanguage { Id = 2, Culture = "tr-TR", Name = "tr-TR" });
        }
    }
}
