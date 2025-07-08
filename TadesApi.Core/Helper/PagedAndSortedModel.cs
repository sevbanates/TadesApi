using System.Collections.Generic;

namespace TadesApi.CoreHelper
{
    public class PagedAndSortedModel<T>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "desc";
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }

        public List<T> ResultList { get; set; } = new();
    }

    public class PagingResponseModel<T>
    {
        public T Data;
        public int TotalRows = 0;
        public bool ReturnStatus { get; set; }
    }

    public class JobResponse
    {
        public List<string> Messages { get; set; } = new();
        public List<string> ErrorMessages { get; set; } = new();
        public bool ReturnStatus { get; set; } = true;
    }
}