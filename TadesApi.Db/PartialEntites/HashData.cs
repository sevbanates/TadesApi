using System;
using System.Collections.Generic;
using System.Text;

namespace TadesApi.Db.PartialEntites
{
    [AttributeUsage(AttributeTargets.All)]
    public class HashData : System.Attribute
    {
        public HashData()
        {
        }
        public int id { get; set; }
    }
}
