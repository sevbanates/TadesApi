using TadesApi.Db.PartialEntites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TadesApi.Core;
using TadesApi.Core.Session;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Extensions
{
    public static class ModelBuilderExtensions
    {

        public static void AddGlobalFilter(this ModelBuilder modelBuilder,  ICurrentUser _currentUser)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);

                //else if (typeof(ITenant).IsAssignableFrom(type.ClrType))
                //    modelBuilder.SetTenantFilter(type.ClrType, _currentUser);
            }
        }

        //***************************************** Delete *************************************

        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder });
        }

        static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(ModelBuilderExtensions)
                   .GetMethods(BindingFlags.Public | BindingFlags.Static)
                   .Single(t => t.IsGenericMethod && t.Name == "SetSoftDeleteFilter");

        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, ISoftDeletable
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);

        }

     
    }
}
