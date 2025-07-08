using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core;
using TadesApi.Core.Session;

namespace TadesApi.Db.PartialEntites
{
    public class DynamicTableInterceptor : DbCommandInterceptor
    {
        private ICurrentUser _workContext;

        public DynamicTableInterceptor(ICurrentUser currentUser)
        {
            _workContext = currentUser;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            //command.CommandText = ReplaceTableName(command);
            return result;
        }
        //private string ReplaceTableName(DbCommand command)
        //{
        //    if (command.CommandText.Contains(tenantKey()))
        //    {
        //        string text = command.CommandText.Replace(tenantKey(), addTenantKey());
        //        return text;
        //    }

        //    return command.CommandText; ;
        //}
        //private string tenantKey()
        //{
        //    return "[TenantId] = 0";
        //}
        //private string addTenantKey()
        //{
        //    return $"[TenantId] = {_workContext.TenantId}";
        //}

    }
}
