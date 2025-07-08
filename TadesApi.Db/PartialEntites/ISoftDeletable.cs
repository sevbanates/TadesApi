using System;
using System.Collections.Generic;
using System.Text;

namespace TadesApi.Db.PartialEntites
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}