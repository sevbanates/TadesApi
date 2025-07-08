using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Data.Common;
using System.Linq.Expressions;
using TadesApi.Db.Entities.AppDbContext;
using TadesApi.Core;
using TadesApi.Core.Session;
using TadesApi.Db.Extensions;

namespace TadesApi.Db.Infrastructure
{
    public class GeneralRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BtcDbContext _context;
        private DbSet<T> _entities;

        private IBetechContextProvider _provider;

        private readonly ICurrentUser _workContext;

        public GeneralRepository(BtcDbContext context, IBetechContextProvider provider, ICurrentUser workContext)
        {
            _context = context;
            _entities = context.Set<T>();
            _provider = provider;
            _workContext = workContext;
        }

        /// <summary>
        ///     Gets a table
        /// </summary>
        public virtual IQueryable<T> Table => Entities;

        public virtual IQueryable<T> TableByDb(string connection, AppDbContext dbContext)
        {
            DbSet<T> entities = dbContext.Set<T>();
            return entities;
        }

        /// <summary>
        ///     Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only
        ///     operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        public virtual IQueryable<T> GetTableNoTracking()
        {
            var entities = Entities.AsNoTracking();

            return entities;
        }

        public virtual IQueryable<T> GetTable()
        {
            var entities = Entities;

            return entities;
        }

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

        //Dinamik Database Method connection => Database'den alınıyor.
        public virtual T GetByIdByDbSql(object id, string companyName)
        {
            using (var dbContext = _provider.GetDbContextByDb(companyName.Trim()))
            {
                DbSet<T> entities = dbContext.Set<T>();
                T entity = entities.Find(id);

                return entity;
            }
        }

        //Dinamik Database Method connection => Database'den alınıyor. Sonrasında Update işlemi olacağı için DBContext dışardan veriliyor.
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
        public virtual void Update(IList<T> entities)
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


            _context.Entry(updateEntity).CurrentValues
                .SetValues(setEntity); //Tüm kayıtlar, kolon eşitlemesine gitmeden bir entity'den diğerine atanır.

            foreach (var property in _context.Entry(setEntity).Properties)
            {
                if (property.CurrentValue == null)
                {
                    _context.Entry(updateEntity).Property(property.Metadata.Name).IsModified = false;
                }
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

        // Tekrar tekrar include dememek için bu methodu kullanabiliriz.
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

        // Toplu işlemlerde kullanılmak üzere DbTransaction özelliği eklendi.  Ali GUL
        public bool ExecuteQuery(string sql, DbTransaction transaction)
        {
            _context.ExecuteQuery(sql, transaction);
            return true;
        }

        public int BulkUpdate(Expression<Func<T, bool>> query, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> expression)
        {
            var resp = _context.Set<T>().Where(query).ExecuteUpdate(expression);
            _context.SaveChanges();

            return resp;
        }


        //  DbTransaction özelliği eklendi. Ali GUL
        //  identitiy isteğe bağlı eğer dönüşü isteniyorsa  komutların sonuna ekleyin SELECT @@IDENTITY AS 'Identity'
        public bool ExecuteQuery(string sql, DbTransaction transaction, out string identity)
        {
            try
            {
                var result = _context.ExecuteQuery(sql, transaction);
                if ((result != null) && (result.Any()))
                {
                    identity = result.FirstOrDefault().Single(x => x.Key == "Identity").Value.ToString();
                }
                else
                {
                    identity = null;
                }

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


        public T GetEntity(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includes = "")
        {
            IQueryable<T> query = Entities.AsNoTracking();

            //DbSet<T> entities = dbContext.Set<T>();
            //return entities.AsNoTracking();

            //IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // If you use the First() method you will get an exception when the result is empty.
            return query?.FirstOrDefault();
        }

        public List<T> GetEntities(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includes = "")
        {
            var query = Entities.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ToList();
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

        public bool ExecuteDelete(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null)
        {
            IQueryable<T> query = Entities.AsQueryable();

            if (filter == null)
                throw new ArgumentNullException("Delete Filter Cannot be null");

            query = query.Where(filter);

            if (orderby != null)
                query = orderby(query);

            query.ExecuteDelete();
            _context.SaveChanges();


            return true;
        }


        /// <summary>
        ///     Entities
        /// </summary>
        protected virtual DbSet<T> Entities => _entities ??= _context.Set<T>();
    }
}