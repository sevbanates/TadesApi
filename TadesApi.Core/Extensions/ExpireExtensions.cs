using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TadesApi.Core.Extensions
{
    public static class ExpireExtensions
    {
        public static IEnumerable<IExpired> CheckExpire(this IEnumerable<IExpired> entityList,int? expireMinute=5)
        {
            //Kitlenip 5 dakkayı geçen kayıtlar'ın kilidi açılır. Rule'a çevir...
            entityList.Where(re => re.OpenStatusUserId != null && (DateTime.Now - re.ModDate).Value.TotalMinutes >= expireMinute).ToList().ForEach(row =>
            {
                row.OpenStatus = true;
                row.OpenStatusUserId = null;
                row.ModDate = null;
            });
            return entityList;
        }
    }
}
