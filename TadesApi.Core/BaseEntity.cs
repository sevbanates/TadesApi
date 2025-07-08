using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TadesApi.CoreHelper;

namespace TadesApi.Core
{
    public abstract class BaseEntity
    {
        [Key] public long Id { get; set; }

        public Guid GuidId { get; private set; } = Guid.NewGuid();
        public DateTime CreDate { get;  set; } = DateTime.Now;

        public BaseEntity()
        {
            Logs = new List<string>();
        }


        [NotMapped]
        public virtual string Key
        {
            get { return this.GetType().GetProperty("Id").GetValue(this).ToString(); }
            private set { }
        }


        [NotMapped]
        public bool IsLoaded
        {
            get { return !(this.Key.IsInitial() || this.Key == "0"); }
            private set { }
        }


        [NotMapped] public List<string> Logs { get; set; }
    }
}