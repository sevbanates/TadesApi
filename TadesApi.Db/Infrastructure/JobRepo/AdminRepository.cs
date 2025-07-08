
using TadesApi.Core;
using TadesApi.Db;
using TadesApi.Db.Extensions;
using TadesApi.Db.PartialEntites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using TadesApi.Db.Entities.AppDbContext;

namespace TadesApi.Db.Infrastructure
{

    public class AdminRepository<T> : IAdminRepository<T> where T : BaseEntity
    {
        private readonly AdminDbContext _context;
        private DbSet<T> _entities;

        private IBetechContextProvider _provider;

        public AdminRepository(AdminDbContext context, IBetechContextProvider provider)
        {
            _context = context;
            _provider = provider;
            _entities = context.Set<T>();
        }

        public virtual IQueryable<T> Table => Entities;
        public virtual IQueryable<T> TableByDb(string connection, AppDbContext dbContext)
        {
            DbSet<T> entities = dbContext.Set<T>();
            return entities;
        }
        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        public virtual IQueryable<T> TableNoTrackingByDb(string connection, AppDbContext dbContext)
        {
            DbSet<T> entities = dbContext.Set<T>();
            return entities.AsNoTracking();
        }

        public virtual T GetById(object id)
        {
            T entity = Entities.Find(id);

            return entity;
        }
        

        
        public virtual T GetByIdByDb(object id, string connection)
        {
            using (var dbContext = _provider.GetDbContext(connection))
            {
                DbSet<T> entities = dbContext.Set<T>();
                T entity = entities.Find(id);

                return entity;
            }
        }
        public virtual T GetByIdByDbSql(object id, string companyName)
        {
            using (var dbContext = _provider.GetDbContextByDb(companyName.Trim()))
            {
                DbSet<T> entities = dbContext.Set<T>();
                T entity = entities.Find(id);

                return entity;
            }
        }
        public virtual T GetByIdByDbSql(object id, string companyName, AppDbContext dbContext)
        {
            DbSet<T> entities = dbContext.Set<T>();
            T entity = entities.Find(id);
            
            return entity;
        }

        /// <summary>
        ///     Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Add(entity);
            _context.SaveChanges();
        }
        

        public virtual void Insert(IList<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            
            Entities.AddRange(entities);
            _context.SaveChanges();
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Update(entity);
            _context.SaveChanges();
        }
        /// <summary>
        ///     Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            Entities.UpdateRange(entities);

            _context.SaveChanges();
        }

        public virtual void UpdateByDbSql(T entity, AppDbContext dbContext)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbContext.SaveChanges();
        }

        //Index'e atma işlemi birçok yerde olabileceği için Extract edilmiştir. AuditLogModel tipinin JsonModel propertysine, ilgili sınıfın önceki hali serialize edilip Elastiğin, "audit_log" indexi'ine atılır.
        public void InsertElastic(T updateEntity, string operatioName, string elasticIndex)
        {
            if (updateEntity is IAuditable)
            {
                //Insert ElasticSearch for AuditLog
                //string jsonString;
                //jsonString = JsonConvert.SerializeObject(updateEntity, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                //AuditLogModel logModel = new(_workContext.CurrentUserId, jsonString, updateEntity.GetType().Name, operatioName, DateTime.Now, _workContext.IsMobile, _workContext.UniquedeviceID);
                ////AuditLogModel logModel = new AuditLogModel();
                ////logModel.PostDate = DateTime.Now;
                ////logModel.UserID = _workContext.CurrentUserId;
                ////logModel.JsonModel = jsonString;
                ////logModel.Operation = operatioName;
                ////logModel.ClassName = updateEntity.GetType().Name;

                //_elasticAuditLogService.CheckExistsAndInsertLog(logModel, elasticIndex);
            }
        }

        /// <summary>
        ///     Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void UpdateMatchEntity(T updateEntity, T setEntity)
        {
            //updateEntity: Varolan hali, setEntity: Güncellenmiş hali
            if (setEntity == null)
                throw new ArgumentNullException(nameof(setEntity));

            if (updateEntity == null)
                throw new ArgumentNullException(nameof(updateEntity));
            
            _context.Entry(updateEntity).CurrentValues.SetValues(setEntity);//Tüm kayıtlar, kolon eşitlemesine gitmeden bir entity'den diğerine atanır.

            foreach (var property in _context.Entry(setEntity).Properties)
            {
                if (property.CurrentValue == null) { _context.Entry(updateEntity).Property(property.Metadata.Name).IsModified = false; }
            }

            _context.SaveChanges();
        }

        /// <summary>
        ///     Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.Remove(entity);
            _context.SaveChanges();
        }

        /// <summary>
        ///     Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            Entities.RemoveRange(entities);

            _context.SaveChanges();
        }
        public IQueryable<T> IncludeMany(params Expression<Func<T, object>>[] includes)
        {
            return _entities.IncludeMultiple(includes);
        }

        public IEnumerable<T> GetSql(string sql)
        {
            return Entities.FromSqlRaw(sql).AsNoTracking();
        }
        public IEnumerable<T> GetSqlByDb(string sql, AppDbContext dbContext)
        {
            DbSet<T> entities = dbContext.Set<T>();
            return entities.FromSqlRaw(sql).AsNoTracking();
        }
        public IEnumerable<T> GetSql(string sql, params object[] parameters)
        {
            return Entities.FromSqlRaw(sql, parameters).AsNoTracking();
        }

        public bool ExecuteQuery(string sql)
        {
            _context.ExecuteQuery(sql);
            return true;
        }
        public bool ExecuteQuery(string sql, DbTransaction transaction)
        {
            _context.ExecuteQuery(sql, transaction);
            return true;
        }
        public bool ExecuteQuery(string sql, DbTransaction transaction, out string identity)
        {
            try
            {
                var result = _context.ExecuteQuery(sql, transaction);
                if ((result != null) && (result.Count() > 0))
                {
                    identity = result.FirstOrDefault().Single(x => x.Key == "Identity").Value.ToString();
                }
                else { identity = null; }
                return true;
            }
            catch
            {
                identity = null;
                return false;
            }
        }

        public bool ExecuteQueryByDb(string sql, string connection)
        {
            using (var dbContext = _provider.GetDbContext(connection))
            {
                try
                {
                    dbContext.ExecuteQuery(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool ExecuteQueryByDbSql(string sql, string companyName)
        {
            using (var dbContext = _provider.GetDbContextByDb(companyName))
            {
                try
                {
                    dbContext.ExecuteQuery(sql);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        public T GetEntity(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = "")
        {
            IQueryable<T> query = Entities.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderby != null)
            {
                query = orderby(query);
            }

            return query?.FirstOrDefault();
        }

        public void Save(T entity)
        {
            if (entity.IsLoaded)
            {
                Update(entity);
            }
            else
            {
                Insert(entity);
            }
        }

        public void DeleteById(object Id)
        {
            T entity = GetById(Id);

            if (entity == null)
                throw new ArgumentNullException("Delete Entity Error");

            Delete(entity);
        }

        public bool DeleteByQuery(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null)
        {
            IQueryable<T> query = Entities.AsQueryable();

            if (filter == null)
                throw new ArgumentNullException("Delete Filter Cannot be null");

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderby != null)
            {
                query = orderby(query);
            }

            var entites = query.ToList();

            if (entites.Any())
                Delete(entites);

            return true;
        }


        /// <summary>
        ///     Entities
        /// </summary>
        protected virtual DbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());
    }
}
