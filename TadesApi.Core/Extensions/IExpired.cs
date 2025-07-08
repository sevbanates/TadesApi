using System;
using System.Collections.Generic;
using System.Text;

namespace TadesApi.Core.Extensions
{
    //İlgili interfaceden türemiş sınıflar güncellendiği ya da silindiği zaman, bu iterfaceden türeyen sınıfların ilgili kolonları locklama amaçlı doldurulur.
    public interface IExpired
    {
        public DateTime? ModDate { get; set; }        
        public bool? OpenStatus { get; set; }
        public int? OpenStatusUserId { get; set; }
    }
}