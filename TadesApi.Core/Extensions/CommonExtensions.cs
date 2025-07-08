using System;
using System.Collections.Generic;
using System.Text;

namespace TadesApi.Core.Extensions
{
    public static class CommonExtensions
    {
        public static long GetTotalMilliSeconds(this DateTime time)
        {
            return Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds);
        }
        public static void Merge<T>(this ActionResponse<T> controllerResponse, ActionResponse<T> ActionResponse)
        {
            //controllerResponse.Entity = ActionResponse.Entity;
            //controllerResponse.List = ActionResponse.List;
            //controllerResponse.ReturnStatus = ActionResponse.ReturnStatus;
            //controllerResponse.ReturnMessage = ActionResponse.ReturnMessage;
            //controllerResponse.HasExceptionError = ActionResponse.HasExceptionError;
            //controllerResponse.TotalRows = ActionResponse.TotalRows;
            //controllerResponse.ExceptionMessage = ActionResponse.ExceptionMessage;
        }
    }
}
