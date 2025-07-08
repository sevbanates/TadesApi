using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Db.Configuration.AppManagament
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users");

            entity.Property(x => x.Id)
                .HasColumnName("Id")
                //.HasColumnType("int")
                .IsRequired()
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            entity.Property(p => p.GuidId)
                .HasDefaultValueSql("NEWID()");

            entity.Property(e => e.ChangePassCode)
                .HasMaxLength(128)
                .IsUnicode(false);

            entity.Property(e => e.ChangePassDate).HasColumnType("datetime");

            entity.Property(e => e.ChangePassReq)
                .HasMaxLength(1)
                .IsUnicode(false);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(150);


            entity.Property(e => e.LastIpAddress)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LastLoginTime).HasColumnType("datetime");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(1024)
                .IsUnicode(false);

            entity.Property(e => e.PasswordSalt2)
                .HasMaxLength(1024)
                .IsUnicode(false);

            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            //entity.HasMany(s => s.Clients).WithOne(s => s.User).HasForeignKey(s => s.UserId);
        }
    }
}