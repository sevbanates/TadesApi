using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TadesApi.Db.Entities;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Entities.AppDbContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<ForgotPassword> ForgotPassword { get; set; }
    public virtual DbSet<SysControllerAction> SysControllerAction { get; set; }
    public virtual DbSet<SysControllerActionRole> SysControllerActionRole { get; set; }
    public virtual DbSet<SysControllerActionTotal> SysControllerActionTotal { get; set; }
    public virtual DbSet<SysLanguage> SysLanguage { get; set; }
    public virtual DbSet<SysRole> SysRole { get; set; }
    public virtual DbSet<SysStringResource> SysStringResource { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<VmenuAction> VmenuAction { get; set; }
    public virtual DbSet<CmmLog> CmmLog { get; set; }
    public virtual DbSet<Countries> Countries { get; set; }
    public virtual DbSet<Cities> Cities { get; set; }
    public virtual DbSet<UserRequest> UserRequests { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<TicketMessage> TicketMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=TadesDb;Persist Security Info=True;User ID=sa;Password=Kinney149;TrustServerCertificate=True;pooling=True;min pool size=0;max pool size=100;MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.InvoiceNumber)
            .IsUnique();




        modelBuilder.Entity<ForgotPassword>(entity =>
        {
            entity.Property(e => e.CreDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GuidId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Token).HasMaxLength(100);
        });

   

        modelBuilder.Entity<SysControllerAction>(entity =>
        {
            entity.Property(e => e.ActionName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ControllerName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GuidId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<SysControllerActionRole>(entity =>
        {
            entity.HasIndex(e => new { e.RoleId, e.ActionName, e.Controller }, "IX_SysControllerActionRole_RoleId_ActionName_Controller").IsUnique();

            entity.Property(e => e.ActionName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Controller)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.CreDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GuidId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<SysControllerActionTotal>(entity =>
        {
            entity.Property(e => e.Controller)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GuidId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<SysLanguage>(entity =>
        {
            entity.HasIndex(e => e.Culture, "IX_SysLanguage_Culture").IsUnique();

            entity.Property(e => e.CreDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Culture)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.GuidId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SysRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SysRole_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GuidId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.RoleDescription).HasMaxLength(255);
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<SysStringResource>(entity =>
        {
            entity.HasIndex(e => e.SysLanguageId, "IX_SysStringResource_SysLanguageId");

            entity.Property(e => e.ResourceName)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasMaxLength(1024);

            entity.HasOne(d => d.SysLanguage).WithMany(p => p.SysStringResource).HasForeignKey(d => d.SysLanguageId);
        });

        modelBuilder.Entity<UserRequest>(entity =>
        {
            entity.Property(e => e.RequesterId).HasColumnName("RequesterID");
            entity.Property(e => e.TargetUserId).HasColumnName("TargetUserID");

            entity.HasOne(ur => ur.Requester)
                .WithMany()
                .HasForeignKey(ur => ur.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ur => ur.TargetUser)
                .WithMany()
                .HasForeignKey(ur => ur.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
