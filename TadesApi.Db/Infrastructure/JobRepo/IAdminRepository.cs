
using TadesApi.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using TadesApi.Db.Entities.AppDbContext;

namespace TadesApi.Db.Infrastructure
{

    public interface IAdminRepository<T> where T : BaseEntity
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableByDb(string connection, AppDbContext dbContext);//Transection için "dbContext" gerekli.
        IQueryable<T> TableNoTracking { get; }
        IQueryable<T> TableNoTrackingByDb(string connection, AppDbContext dbContext); //Transection için "dbContext" gerekli.
        T GetById(object id);
        T GetEntity(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeProperties = "");
        T GetByIdByDb(object id, string connection);//isDecrypt şifreli propertyler var ise "true" atanır ve şifresiz alınır.
        T GetByIdByDbSql(object id, string companyName);//DB Dinamic ConnectionString ==>Datbase'den seçiliyor.
        T GetByIdByDbSql(object id, string companyName, AppDbContext dbContext);//DB Dinamic ConnectionString ==>Datbase'den seçiliyor. DBContext'in dışardan verilen hali.
        void Insert(T entity);

        void Insert(IList<T> entities);
        void Update(T entity);
        void Save(T entity);

        void Update(IEnumerable<T> entities);
        void UpdateByDbSql(T entity, AppDbContext dbContext);//Database dinamic. dbContext dışardan veriliyor ve DBConnection Database'den alınıyor.
        void UpdateMatchEntity(T updateEntity, T setEntity);//isEncrypt şifreli propertyler var ise "true" atanır.
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

        bool ExecuteQuery(string sql, DbTransaction transaction, out string identity); // toplu kayıtlarda kullanmak üzere transaction ve identity alınması eklendi. Ali GUL

        bool ExecuteQueryByDb(string sql, string connection);
        bool ExecuteQueryByDbSql(string sql, string companyName);
    }
}
