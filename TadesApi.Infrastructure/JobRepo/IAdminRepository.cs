
using VideoPortalApi.Core;
using VideoPortalApi.Db.Entities.AppDbContext;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace VideoPortalApi.Infrastructure
{

    public interface IAdminRepository<T> where T : BaseEntity
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableByDb(string connection, AppDbContext dbContext);//Transection için "dbContext" gerekli.
        IQueryable<T> TableNoTracking { get; }
        IQueryable<T> TableNoTrackingByDb(string connection, AppDbContext dbContext); //Transection için "dbContext" gerekli.
        T GetById(object id, bool isDecrypt = false);
        T GetEntity(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = "");
        T GetByIdByDb(object id, string connection, bool isDecrypt = false);//isDecrypt şifreli propertyler var ise "true" atanır ve şifresiz alınır.
        T GetByIdByDbSql(object id, string companyName, bool isDecrypt = false);//DB Dinamic ConnectionString ==>Datbase'den seçiliyor.
        T GetByIdByDbSql(object id, string companyName, AppDbContext dbContext, bool hasEncryptFields = false);//DB Dinamic ConnectionString ==>Datbase'den seçiliyor. DBContext'in dışardan verilen hali.
        void Insert(T entity, bool hasEncryptFields = false);
        //void Insert(IEnumerable<T> entities, bool isEncrypt = false);
        void Insert(IList<T> entities, bool hasEncryptFields = false);
        void Update(T entity, bool isEncrypt = false);
        void Save(T entity, bool isEncrypt = false);

        void Update(IEnumerable<T> entities);
        void UpdateByDbSql(T entity, AppDbContext dbContext);//Database dinamic. dbContext dışardan veriliyor ve DBConnection Database'den alınıyor.
        void UpdateMatchEntity(T updateEntity, T setEntity, bool hasEncryptFields = false);//isEncrypt şifreli propertyler var ise "true" atanır.
        void Delete(T entity);
        void DeleteById(object Id);
        bool DeleteByQuery(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null);


        void Delete(IEnumerable<T> entities);
        IQueryable<T> IncludeMany(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetSql(string sql);
        IEnumerable<T> GetSqlByDb(string sql, AppDbContext dbContext);
        bool ExecuteQuery(string sql);
        public IEnumerable<T> GetSql(string sql, params object[] parameters);
      
        bool ExecuteQuery(string sql, DbTransaction transaction); // toplu kayıtlarda kullanmak üzere transaction eklendi. Ali GUL

        bool ExecuteQuery(string sql, DbTransaction transaction, out string identitiy); // toplu kayıtlarda kullanmak üzere transaction ve identity alınması eklendi. Ali GUL

        bool ExecuteQueryByDb(string sql, string connection);
        bool ExecuteQueryByDbSql(string sql, string companyName);
    }
}
