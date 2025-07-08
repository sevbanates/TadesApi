using System;
using System.Collections.Generic;
using TadesApi.Core.Models.Global;

namespace TadesApi.Core
{
    public class PagedAndSortedResponse<T>
    {
        public bool IsSuccess { get; set; } = true;
        public List<string> ReturnMessage { get; set; } = new();

        //public int PageNumber { get; set; } = 1;
        public int TotalCount { get; set; }

        //public int PageSize { get; set; }
        //public List<SortDescriptor> Sort { get; set; } = new();
        public string Token { get; set; }
        public List<T> EntityList { get; set; }
    }

    [Serializable]
    public class ActionResponse<T>
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ReturnMessage { get; set; } = new();
        public T Entity { get; set; }
        public List<T> EntityList { get; set; }
    }

    public class FunctionResponse<T>
    {
        public T Entity { get; set; }
        public List<T> EntityList { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ReturnMessage { get; set; } = new();
        public string SingleMessage { get; set; }
        public int Count { get; set; } = 0;
    }
}