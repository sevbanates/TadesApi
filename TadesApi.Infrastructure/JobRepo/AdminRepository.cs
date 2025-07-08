
using VideoPortalApi.Core;
using VideoPortalApi.Db;
using VideoPortalApi.Db.Entities.AppDbContext;
using VideoPortalApi.Db.Extensions;
using VideoPortalApi.Db.PartialEntites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace VideoPortalApi.Infrastructure
{

    public class AdminRepository<T> : IAdminRepository<T> where T : BaseEntity
    {
        private readonly AdminDbContext _context;
        private DbSet<T> _entities;
        private readonly IEncryption _encryption;

        private IBetechContextProvider _provider;

        public AdminRepository(IEncryption encryption, AdminDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
            _encryption = encryption;
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

        public virtual T GetById(object id, bool isDecrypt = false)
        {
            T entity = Entities.Find(id);
            //Locklama işlemi Repository katmanında yapılır ise...
            /*if (entity is Core.Extensions.IExpired && _context.Database.CurrentTransaction == null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var model = (Core.Extensions.IExpired)entity;
                        if (model.OpenStatus == true || (model.OpenStatus == false && model.OpenStatusUserId == _workContext.CurrentUserId))
                        {
                            //!!!Tam bu noktada başka bir client üstüne alır ise üzerine ezilmiş olunur....        
                            model.OpenStatus = false;
                            model.OpenStatusUserId = _workContext.CurrentUserId;
                            model.ModDate = DateTime.Now;
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }*/
            if (isDecrypt)
            {
                return DecryptEntityFields(entity, _context);
            }
            return entity;
        }

        public virtual T UpdateEncryptedEntityFieldIfChange(T entity)
        {
            MetadataTypeAttribute[] metadataTypes = entity.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().ToArray();
            foreach (MetadataTypeAttribute metadata in metadataTypes)
            {
                System.Reflection.PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.CryptoData)))
                    {
                        if (_context.Entry(entity).Property(pi.Name).OriginalValue.ToString() != _encryption.EncryptText(_context.Entry(entity).Property(pi.Name).CurrentValue.ToString()))
                        {
                            _context.Entry(entity).Property(pi.Name).CurrentValue = _encryption.EncryptText(_context.Entry(entity).Property(pi.Name).CurrentValue.ToString());
                        }
                        else
                        {
                            _context.Entry(entity).Property(pi.Name).IsModified = false;
                        }
                    }
                    else if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.HashData))) //User PasswordHash Tek yönlü şifreleme.
                    {

                        if (_context.Entry(entity).Property(pi.Name).IsModified)
                        {
                            string salt = _encryption.GenerateSalt();
                            string hashData = _encryption.HashCreate(_context.Entry(entity).Property(pi.Name).CurrentValue.ToString(), salt);
                            _context.Entry(entity).Property(pi.Name).CurrentValue = hashData;
                        }
                    }
                }
            }
            return entity;
        }

        public virtual T DecryptEntityFields(T entity, AppDbContext _dbcontext)
        {
            MetadataTypeAttribute[] metadataTypes = entity.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().ToArray();
            foreach (MetadataTypeAttribute metadata in metadataTypes)
            {
                System.Reflection.PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.CryptoData)))
                    {
                        _dbcontext.Entry(entity).Property(pi.Name).CurrentValue = _encryption.DecryptText(_dbcontext.Entry(entity).Property(pi.Name).CurrentValue.ToString());
                    }
                }
            }
            return entity;
        }
        public virtual T GetByIdByDb(object id, string connection, bool isDecrypt = false)
        {
            using (var dbContext = _provider.GetDbContext(connection))
            {
                DbSet<T> entities = dbContext.Set<T>();
                T entity = entities.Find(id);
                if (isDecrypt)
                {
                    return DecryptEntityFields(entity, dbContext);
                }
                return entity;
            }
        }
        public virtual T GetByIdByDbSql(object id, string companyName, bool isDecrypt = false)
        {
            using (var dbContext = _provider.GetDbContextByDb(companyName.Trim()))
            {
                DbSet<T> entities = dbContext.Set<T>();
                T entity = entities.Find(id);
                if (isDecrypt)
                {
                    return DecryptEntityFields(entity, dbContext);
                }

                return entity;
            }
        }
        public virtual T GetByIdByDbSql(object id, string companyName, AppDbContext dbContext, bool isDecrypt = false)
        {
            DbSet<T> entities = dbContext.Set<T>();
            T entity = entities.Find(id);
            if (isDecrypt)
            {
                return DecryptEntityFields(entity, dbContext);
            }

            return entity;
        }

        /// <summary>
        ///     Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Insert(T entity, bool hasEncryptFields = false)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (hasEncryptFields)
            {
                entity = EncryptEntityFields(entity, _context);
            }

            _entities.Add(entity);
            _context.SaveChanges();
        }
        public virtual T EncryptEntityFields(T entity, AppDbContext dbContext)
        {
            MetadataTypeAttribute[] metadataTypes = entity.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().ToArray();
            foreach (MetadataTypeAttribute metadata in metadataTypes)
            {
                System.Reflection.PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.CryptoData)))
                    {
                        int id = ((CryptoData)pi.GetCustomAttributes(true)[0]).id;
                        dbContext.Entry(entity).Property(pi.Name).CurrentValue = _encryption.EncryptText(dbContext.Entry(entity).Property(pi.Name).CurrentValue?.ToString());
                    }
                    else if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.HashData))) //User PasswordHash Tek yönlü şifreleme.
                    {
                        string salt = _encryption.GenerateSalt();
                        string hashData = _encryption.HashCreate(dbContext.Entry(entity).Property(pi.Name).CurrentValue.ToString(), salt);
                        dbContext.Entry(entity).Property(pi.Name).CurrentValue = hashData;
                    }
                }
            }
            return entity;
        }

        public virtual void Insert(IList<T> entities, bool hasEncryptFields = false)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (hasEncryptFields)
            {
                for (var i = 0; i < entities.Count(); i++)
                {
                    entities[i] = EncryptEntityFields(entities[i], _context);
                }
            }
            Entities.AddRange(entities);
            _context.SaveChanges();
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(T entity, bool hasEncryptFields = false)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (hasEncryptFields)
            {
                entity = UpdateEncryptedEntityFieldIfChange(entity);
            }
            if (entity is IAuditable)
            {
                //InsertElastic(entity, "Update", _elasticConfig.Value.ElasticAuditIndex);
            }

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
        public virtual void UpdateMatchEntity(T updateEntity, T setEntity, bool hasEncryptFields = false)
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
            if (hasEncryptFields)
            {
                updateEntity = UpdateEncryptedEntityFieldIfChange(updateEntity);
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
        public bool ExecuteQuery(string sql, DbTransaction transaction, out string identitiy)
        {
            try
            {
                var result = _context.ExecuteQuery(sql, transaction);
                if ((result != null) && (result.Count() > 0))
                {
                    identitiy = result.FirstOrDefault().Single(x => x.Key == "Identity").Value.ToString();
                }
                else { identitiy = null; }
                return true;
            }
            catch
            {
                identitiy = null;
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

        public void Save(T entity, bool isEncrypt = false)
        {
            if (entity.IsLoaded)
            {
                Update(entity, isEncrypt);
            }
            else
            {
                Insert(entity, isEncrypt);
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
