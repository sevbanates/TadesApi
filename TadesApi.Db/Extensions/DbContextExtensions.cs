using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Common;

namespace TadesApi.Db.Extensions
{
    public static class DbContextExtensions
    {
        public static IEnumerable<Dictionary<string, object>> ExecuteQuery(this DbContext context, string query, params object[] parameters)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;

                if (parameters.Any())
                    command.Parameters.AddRange(parameters);

                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                using (var dataReader = command.ExecuteReader())
                {
                    var dataRow = ReadData(dataReader);

                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();

                    return dataRow;
                }
            }
        }

        public static async Task<IEnumerable<Dictionary<string, object>>> ExecuteQueryAsync(this DbContext context, string query, params object[] parameters)
        {
            await using (var command = context.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = query;

                if (parameters.Any())
                    command.Parameters.AddRange(parameters);

                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                await using (var dataReader = await command.ExecuteReaderAsync())
                {

                    var dataRow = ReadData(dataReader);

                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();

                    return dataRow;
                }
            }
        }

        private static IEnumerable<Dictionary<string, object>> ReadData(IEnumerable reader)
        {
            var dataList = new List<Dictionary<string, object>>();

            foreach (var item in reader)
            {
                IDictionary<string, object> expando = new ExpandoObject();

                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(item))
                {
                    var obj = propertyDescriptor.GetValue(item);
                    expando.Add(propertyDescriptor.Name, obj);
                }

                dataList.Add(new Dictionary<string, object>(expando));
            }

            return dataList;
        }


        // toplu kayıtlarda kullanmak üzere transaction ve identity alınması eklendi. Ali GUL
        public static IEnumerable<Dictionary<string, object>> ExecuteQuery(this DbContext context, string query, DbTransaction transaction, params object[] parameters)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = transaction;
                if (parameters.Any())
                    command.Parameters.AddRange(parameters);

                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                using (var dataReader = command.ExecuteReader())
                {
                    var dataRow = ReadData(dataReader);

                    //if (command.Connection.State == ConnectionState.Open)
                    //    command.Connection.Close();
                    //_command = command;
                    return dataRow;
                }
            }
        }

    }
}
