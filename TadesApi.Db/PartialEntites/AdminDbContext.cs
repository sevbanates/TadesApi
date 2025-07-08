
using TadesApi.Core;
using TadesApi.Db.Entities;
using TadesApi.Db.Extensions;
using TadesApi.Db.PartialEntites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TadesApi.Db.Entities.AppDbContext;

namespace TadesApi.Db
{
    public class AdminDbContext : AppDbContext
    {
        public AdminDbContext()
        {
        }

        public AdminDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public override int SaveChanges()
        {
            //ChangeTracker.DetectChanges();

            //foreach (var entry in this.ChangeTracker.Entries().Where(e => e.Entity is ITenant && (e.State == EntityState.Added) || (e.State == EntityState.Modified)))
            //{
            //    ITenant e = (ITenant)entry.Entity;
            //    e.TenantId = _currentUser.TenantId;
            //}

            //foreach (var entry in this.ChangeTracker.Entries().Where(e => e.Entity is IAuditable && (e.State == EntityState.Added) || (e.State == EntityState.Modified)))
            //{
            //    IAuditable e = (IAuditable)entry.Entity;
            //    if (entry.State == EntityState.Added)
            //    {
            //        if (e.CreUser == 0)
            //            e.CreUser = _currentUser.UserId;
            //        if (e.CreDate == DateTime.MinValue)
            //            e.CreDate = DateTime.Now;
            //    }
            //    else
            //    {
            //        e.ModUser = _currentUser.UserId;
            //        e.ModDate = DateTime.Now;
            //    }
            //}

            return base.SaveChanges();

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            //base.OnConfiguring(optionsBuilder.UseLoggerFactory(VbtLoggerFactory));
            base.OnConfiguring(optionsBuilder.UseLoggerFactory(LoggerFactory)
                //.AddInterceptors(new DynamicTableInterceptor(_currentUser))

                );
#endif
#if RELEASE
                base.OnConfiguring(optionsBuilder);
#endif
        }

        /* 
            Yazdığımız entity`lerin log'unun yazılması. 
            Linq `ların debug moddayken sql execution plan scriptini görmemizi sağlıyor. 
        */
        public static readonly ILoggerFactory LoggerFactory
         = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
         {
             builder
                 .AddFilter((string category, LogLevel level) =>
                     category == DbLoggerCategory.Database.Command.Name
                     && level == LogLevel.Information)
                 .AddDebug();
         });



      
    }
}
