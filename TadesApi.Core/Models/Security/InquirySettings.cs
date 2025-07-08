using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.Security
{
    [Serializable]
    public class InquirySettings
    {
        public InquirySettings()
        {
            DisplayColumns = new List<ColumnInfo>();
            IsFastSearch = false;
            searchParams = new List<string>();
            rowActions = new List<CustomAction>();
            toolbarAction = new List<InquiryAction>();
        }
        //*** General Ingfo

        public bool isDialogMaintanance { get; set; } = false;
        public string GridTitle { get; set; }

        //**** Actions *****
        public bool isAdd { get; set; }
        public string eacId { get; set; }
        public bool hasAdd { get; set; } = true;
        public bool searchByDefaultInquiry { get; set; } = true;
        public bool isEdit { get; set; } = true;
        public bool hasEdit { get; set; } = true;
        public string IdentifierField { get; set; }
        public bool isDelete { get; set; } = true;
        public bool hasDelete { get; set; } = true;


        //**** Columns ************
        public bool IsFastSearch { get; set; }
        public List<string> searchParams { get; set; }
        public List<ColumnInfo> DisplayColumns { get; set; }
        public List<CustomAction> rowActions { get; set; }
        public List<InquiryAction> toolbarAction { get; set; }
        public List<string> localAction { get; set; }


    }
    public class ColumnInfo
    {
        public string FieldName { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string Title { get; set; }
        public int width { get; set; }
        public bool sort { get; set; } = true;



    }
    public class FieldType
    {
        public const string Text = "string";
        public const string Number = "number";
        public const string Decimal = "decimal";
        public const string Date = "date";
        public const string Boolean = "bool";

    }
    public class PageType
    {
        public const string Dialog = "Dialog";
        public const string NewPage = "NewPage";

    }
    public class ContextAction
    {
        public string ActionName { get; set; }
        public bool Status { get; set; }

    }
}
