using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TadesApi.Db.PartialEntites
{
    public class CustomSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            Console.WriteLine($"Saving changes for {eventData.Context.Database.GetConnectionString()}");
            Console.WriteLine($"Saving changes for { eventData.Context.ChangeTracker.DebugView.LongView}");
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            Console.WriteLine($"Saving changes asynchronously for {eventData.Context.Database.GetConnectionString()}");

            return new ValueTask<InterceptionResult<int>>(result);
        }
    }

}
