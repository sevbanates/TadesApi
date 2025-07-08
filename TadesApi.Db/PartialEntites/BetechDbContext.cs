using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TadesApi.Db.Entities.AppDbContext;
using TadesApi.Core.Session;
using TadesApi.Db.Extensions;
using TadesApi.Db.PartialEntites;

namespace TadesApi.Db;

public class BtcDbContext : AppDbContext
{
    private ICurrentUser _currentUser;
    public BtcDbContext()
    {
    }

    public BtcDbContext(DbContextOptions<AppDbContext> options, ICurrentUser currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    public string DecryptText(string encStr) => throw new NotSupportedException();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddGlobalFilter(_currentUser);

        modelBuilder.HasDbFunction(typeof(BtcDbContext).GetMethod(nameof(DecryptText), new[] { typeof(string) }), builder => {
            builder.HasName("DecryptText");
            builder.HasParameter("encStr").HasStoreType("nvarchar(max)");
            builder.HasSchema("dbo");
        });

    }
    public override int SaveChanges()
    {
        ChangeTracker.DetectChanges();

        var data = ChangeTracker.Entries().Where(e => e.Entity is IAuditable).ToList();

        foreach (var entry in this.ChangeTracker.Entries().Where(e => e.Entity is IAuditable
        && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
        {
            IAuditable e = (IAuditable)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                if (e.CreUser == 0)
                    e.CreUser = _currentUser.UserId;
                if (e.CreDate == DateTime.MinValue)
                    e.CreDate = DateTime.Now;
            }
            else
            {
                e.ModUser = _currentUser.UserId;
                e.ModDate = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder.UseLoggerFactory(LoggerFactory));
    }
    
    public static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
     {
         builder.AddFilter((string category, LogLevel level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddDebug();
     });  
}
