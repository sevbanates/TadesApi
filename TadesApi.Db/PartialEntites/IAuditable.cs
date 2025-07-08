using System;
using System.Collections.Generic;
using System.Text;

namespace TadesApi.Db.PartialEntites
{
    public interface IAuditable
    {
        long CreUser { get; set; }
        DateTime CreDate { get; set; }
        long? ModUser { get; set; }
        DateTime ?ModDate { get; set; }
    }
}
