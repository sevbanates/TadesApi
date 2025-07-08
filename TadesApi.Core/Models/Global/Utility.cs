using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TadesApi.Core.Models.Global
{
    public class BaseModel
    {
        //public string Key { get; set; }
        //public bool IsEditState { get; set; } = true;
        //public bool IsLoaded { get; set; } = false;
        public long Id { get; set; }

        //public List<CustomAction> AvailableActions { get; set; } = new List<CustomAction>();
    }

    public interface IBaseUpdateModel
    {
        public Guid GuidId { get; set; }
    }
    

    public class PagedAndSortedInput
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string SortExpression { get; set; } = "Id:desc";
        public string SortBy => SortExpression.Split(":").Length > 0 ? SortExpression.Split(":")[0] : "Id";
        public string SortDirection => SortExpression.Split(":").Length > 1 ? SortExpression.Split(":")[1] : "desc";
    }

    public class PagedAndSortedSearchInput : PagedAndSortedInput
    {
        public string Search { get; set; }
    }
    

    public class RecordState
    {
        public RecordState()
        {
            //AvailableActions = new List<CustomAction>();
            ActionStates = new List<string>();
            LocalStates = new List<string>();
        }

        public bool IsEditState { get; set; } = true;

        //public List<CustomAction> AvailableActions { get; set; }
        public List<string> ActionStates { get; set; }
        public List<string> LocalStates { get; set; }
    }

    public class TextIntValueDto
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

    public class SortDescriptor
    {
        public string Field { get; set; } = "Id";
        public string Dir { get; set; } = "desc";
    }

    public static class SortDirection
    {
        public const string Asc = "asc";
        public const string Desc = "desc";
    }

    public class TextValueDto
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}