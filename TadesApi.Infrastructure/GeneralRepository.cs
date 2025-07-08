
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using VideoPortalApi.Core;
using VideoPortalApi.Core.Session;
using VideoPortalApi.Db;
using VideoPortalApi.Db.Entities.AppDbContext;
using VideoPortalApi.Db.Extensions;
using VideoPortalApi.Db.PartialEntites;

namespace VideoPortalApi.Infrastructure
{

    public class GeneralRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly BtcDbContext _context;
        private DbSet<T> _entities;
        private readonly IEncryption _encryption;

        private IBetechContextProvider _provider;

        //private readonly IElasticSearchService<AuditLogModel> _elasticAuditLogService;
        //Microsoft.Extensions.Options.IOptions<ElasticConnectionSettings> _elasticConfig;

        private readonly ICurrentUser _workContext;
        public GeneralRepository(IEncryption encryption, BtcDbContext context, IBetechContextProvider provider, ICurrentUser workContext)
        {
            _context = context;
            _entities = context.Set<T>();
            _encryption = encryption;
            _provider = provider;
            //_elasticAuditLogService = elasticAuditLogService;
            //_elasticConfig = elasticConfig;
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
        public virtual IQueryable<T> GetTableNoTracking(bool hasEncryptFields = false)
        {
            var entities = Entities.AsNoTracking();

            if (hasEncryptFields)
            {
                entities = DecryptEntityFields(entities, _context);

            }

            return entities;
        }
        public virtual IQueryable<T> GetTable(bool hasEncryptFields = false)
        {
            var entities = Entities;

            if (hasEncryptFields)
            {
                entities = DecryptEntityFields(entities);

            }

            return entities;
        }

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

        //Eğer Güncellenecek "CryptoData" Alan var ise ve değişmiş ise tekrar şifrelenir.
        public virtual T UpdateEncryptedEntityFieldIfChange(T entity)
        {
            MetadataTypeAttribute[] metadataTypes = entity.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().ToArray();
            foreach (MetadataTypeAttribute metadata in metadataTypes)
            {
                System.Reflection.PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                //Metadata atanmış entity'nin tüm propertyleri tek tek alınır.
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    //Eğer ilgili property ait CryptoData flag'i var ise ilgili deger encrypt edilir.
                    if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.CryptoData)))
                    {
                        //int id = ((CryptoData)pi.GetCustomAttributes(true)[0]).id;
                        //Eğer şifreli property gerçekten değişmiş ise tekrardan şifrelenir. Önceki şifreli hali, yeni şifresiz hali şifrelenerek bakılır.
                        if (_context.Entry(entity).Property(pi.Name).OriginalValue.ToString() != _encryption.EncryptText(_context.Entry(entity).Property(pi.Name).CurrentValue.ToString()))
                        {
                            _context.Entry(entity).Property(pi.Name).CurrentValue = _encryption.EncryptText(_context.Entry(entity).Property(pi.Name).CurrentValue.ToString());
                        }
                        else
                        {
                            //Değişmediği için IsModified false atanır. Şifresiz hali geldiği için hiç güncellememek gerekir.
                            _context.Entry(entity).Property(pi.Name).IsModified = false;
                        }
                    }
                    else if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.HashData))) //User PasswordHash Tek yönlü şifreleme.
                    {
                        //Eğer PasswordHash kolonu gelmiş ise, Password alanının değişip değişmediğine "ConvertPassword" ile bakılır.
                        //PasswordHash her zaman değişir. Çünkü DB'de şifreli hali ama kendisine UI'dan atanan şifresiz(password) halidir.
                        //if (_context.Entry(entity).Property(pi.Name.ConvertPassword()).IsModified == false) 
                        //{ 
                        //    _context.Entry(entity).Property(pi.Name).IsModified = false; 
                        //}

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

        //*** Decrypt Entity Fields Methods
        public virtual T DecryptEntityFields(T entity, AppDbContext _dbcontext)
        {
            MetadataTypeAttribute[] metadataTypes = entity.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().ToArray();
            foreach (MetadataTypeAttribute metadata in metadataTypes)
            {
                System.Reflection.PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                //Metadata atanmış entity'nin tüm propertyleri tek tek alınır.
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    //Eğer ilgili property ait CryptoData flag'i var ise ilgili deger Decrypt edilir.
                    if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.CryptoData)))
                    {
                        //int id = ((CryptoData)pi.GetCustomAttributes(true)[0]).id;
                        _dbcontext.Entry(entity).Property(pi.Name).CurrentValue = _encryption.DecryptText(_dbcontext.Entry(entity).Property(pi.Name).CurrentValue.ToString());
                    }
                }
            }
            return entity;
        }
        public virtual IQueryable<T> DecryptEntityFields(IQueryable<T> entities, AppDbContext _dbcontext)
        {

            //var props = typeof(T).GetProperties();

            //var entity = entities.First();
            //foreach (var prop in props)
            //{
            //    if (Attribute.IsDefined(prop, typeof(Db.PartialEntites.CryptoData)))
            //    {
            //        var value = prop.GetValue(entity, null);

            //        prop.SetValue(entity, value);

            //    }

            //}
            T entityExamp = entities.FirstOrDefault();
            MetadataTypeAttribute[] metadataTypes = entityExamp.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().ToArray();
            foreach (MetadataTypeAttribute metadata in metadataTypes)
            {
                System.Reflection.PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                //Metadata atanmış entity'nin tüm propertyleri tek tek alınır.
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    //Eğer ilgili property ait CryptoData flag'i var ise ilgili deger Decrypt edilir.
                    if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.CryptoData)))
                    {
                        foreach (var entity in entities)
                        {
                            var value = _encryption.DecryptText(pi.GetValue(entity, null).ToString());

                            pi.SetValue(entity, value);
                            //_dbcontext.Entry(entity).Property(pi.Name).CurrentValue = _encryption.DecryptText(_dbcontext.Entry(entity).Property(pi.Name).CurrentValue.ToString());
                        }
                    }
                }
            }
            return entities;
        }
        public virtual DbSet<T> DecryptEntityFields(DbSet<T> entities)
        {
            T entityExamp = entities.FirstOrDefault();
            MetadataTypeAttribute[] metadataTypes = entityExamp.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().ToArray();
            foreach (MetadataTypeAttribute metadata in metadataTypes)
            {
                System.Reflection.PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                //Metadata atanmış entity'nin tüm propertyleri tek tek alınır.
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    //Eğer ilgili property ait CryptoData flag'i var ise ilgili deger Decrypt edilir.
                    if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.CryptoData)))
                    {
                        foreach (var entity in entities)
                        {
                            _context.Entry(entity).Property(pi.Name).CurrentValue = _encryption.DecryptText(_context.Entry(entity).Property(pi.Name).CurrentValue.ToString());
                        }
                    }
                }
            }
            return entities;
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
        //Dinamik Database Method connection => Database'den alınıyor.
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

        //Dinamik Database Method connection => Database'den alınıyor. Sonrasında Update işlemi olacağı için DBContext dışardan veriliyor.
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
            //Set Default UsedTime Parameter ==> UsedTime BaseEntity Property.

            //Buradaki _context = T yani örneğin Users. Entry ise property, column vb o entity`e ait herşeyine ulaşmamızı sağlayan kütüphane.
            //Entity'ye ait MetaData varsa bulunur
            if (hasEncryptFields)
            {
                entity = EncryptEntityFields(entity, _context);
            }
            //_context.Entry(entity).Property("UsedTime").CurrentValue = DateTime.Now; //UsedTime Entity'de görülmez [NotMapped] olduğu için.
            //---------------

            _entities.Add(entity);
            _context.SaveChanges();
        }

        //Buradaki _context = T yani örneğin Users. Entry ise property, column vb o entity`e ait herşeyine ulaşmamızı sağlayan kütüphane.
        //Entity'ye ait MetaData varsa bulunur
        public virtual T EncryptEntityFields(T entity, AppDbContext dbContext)
        {
            MetadataTypeAttribute[] metadataTypes = entity.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().ToArray();
            foreach (MetadataTypeAttribute metadata in metadataTypes)
            {
                System.Reflection.PropertyInfo[] properties = metadata.MetadataClassType.GetProperties();
                //Metadata atanmış entity'nin tüm propertyleri tek tek alınır.
                foreach (System.Reflection.PropertyInfo pi in properties)
                {
                    //Eğer ilgili property ait CryptoData flag'i var ise ilgili deger encrypt edilir.
                    if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.CryptoData)))
                    {
                        int id = ((CryptoData)pi.GetCustomAttributes(true)[0]).id;
                        dbContext.Entry(entity).Property(pi.Name).CurrentValue = _encryption.EncryptText(dbContext.Entry(entity).Property(pi.Name).CurrentValue.ToString());
                    }
                    else if (Attribute.IsDefined(pi, typeof(Db.PartialEntites.HashData))) //User PasswordHash Tek yönlü şifreleme.
                    {
                        string salt = _encryption.GenerateSalt();
                        string hashData = _encryption.HashCreate(dbContext.Entry(entity).Property(pi.Name).CurrentValue.ToString(), salt);
                        //dbContext.Entry(entity).Property(pi.Name).CurrentValue = hashData + "æ" + salt;
                        dbContext.Entry(entity).Property(pi.Name).CurrentValue = hashData;
                    }
                }
            }
            return entity;
        }

        /// <summary>
        ///     Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        //public virtual void Insert(IEnumerable<T> entities, bool isEncrypt = false)
        //{
        //    //int a=5;
        //    //int b=0;
        //    //int c=a/b;

        //    if (entities == null)
        //        throw new ArgumentNullException(nameof(entities));

        //    foreach (var entity in entities)
        //    {
        //        if (isEncrypt)
        //        {
        //            var _entity = EncryptEntityFields(entity, _context);
        //            Entities.Add(_entity);
        //        }
        //        else
        //        {
        //            Entities.Add(entity);
        //        }
        //    }

        //    _context.SaveChanges();
        //}
        public virtual void Insert(IList<T> entities, bool hasEncryptFields = false)
        {
            //int a=5;
            //int b=0;
            //int c=a/b;

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
            //Audit Eğer güncelenen sınıf IAuditable'dan türemiş ise ElasticSearc'ün "audit_log" indexine kaydedilir.
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
        public virtual void Update(IList<T> entities, bool hasEncryptFields = false)
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


            //Audit Eğer güncelenen sınıf IAuditable'dan türemiş ise ElasticSearc'ün "audit_log" indexine kaydedilir.
            //InsertElastic(updateEntity, "Update", _elasticConfig.Value.ElasticAuditIndex);


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
            //Silinmeden önceki ilgili sınıfın değeri "IAuditable" interface'inden türetildi ise, AuditLogModel tipinin JsonModel propertysine, serialize edilip Elastiğin, "audit_log" indexi'ine atılır.
            //InsertElastic(entity, "Delete", _elasticConfig.Value.ElasticAuditIndex);

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

        //  DbTransaction özelliği eklendi. Ali GUL
        //  identitiy isteğe bağlı eğer dönüşü isteniyorsa  komutların sonuna ekleyin SELECT @@IDENTITY AS 'Identity'
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

            //DbSet<T> entities = dbContext.Set<T>();
            //return entities.AsNoTracking();

            //IQueryable<T> query = dbSet;
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

            // If you use the First() method you will get an exception when the result is empty.
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
