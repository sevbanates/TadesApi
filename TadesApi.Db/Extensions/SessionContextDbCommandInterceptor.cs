using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Db.Extensions
{
    internal class SessionContextDbCommandInterceptor: DbConnectionInterceptor
    {
        public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC sp_set_session_context 'enc_key', '2909012565820034';";
                    command.ExecuteNonQuery();
                }
            }
            
            base.ConnectionOpened(connection, eventData);
        }

        public override Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC sp_set_session_context 'enc_key', '2909012565820034';";
                    command.ExecuteNonQuery();
                }
            }

            return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        }

    }
}
