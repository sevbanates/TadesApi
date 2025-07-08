using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Configuration.AppManagament
{
    public class LibraryConfiguration : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            builder.ToTable(nameof(Library));
            builder.HasKey(x => x.Id);
            builder.Property(s => s.GuidId).IsRequired();
            builder.Property(s => s.Category).IsRequired();
            builder.Property(s => s.Title).HasMaxLength(250);
            builder.Property(s => s.Description).HasMaxLength(2000);
            builder.Property(s => s.FileName).HasMaxLength(250);
            builder.Property(s => s.VideoUrl).IsRequired().HasMaxLength(1024);
            //builder.Property(s => s.ThumbImg).HasConversion<byte[]>();
            builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        }
    }
}
