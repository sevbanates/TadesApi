using System;
using System.Collections.Generic;
using System.Text;

namespace TadesApi.CoreHelper
{
    public enum FieldValueType
    {
        Number = 0,
        Text = 1,
        Currency = 2,
        Boolean = 3,
        Date = 4,
        Guid = 5,
        //Child = 6
    }
    public class FrontAttrAttribute : Attribute
    {
        public string DisplayTr;
        public string DisplayEn;
        public int TabIndex;
        public bool IsContraint;

        public bool IsReadOnly;


    }
    public class MaintenanceTableAttr
    {
        public string TableName;
        public bool IsGotoList;
        public bool DefaultEditMode;
        public string KeyField;


    }
    public class TrfAttrAttribute : Attribute
    {
        public string TableName;
        public bool IsGotoList;
        public bool IsDilaog;
        public string KeyField;

        public bool DefaultEditMode;

    }

    public class FieldAttrAttribute : Attribute
    {
        public bool IsKey;
        public bool AutoNumber;
        public int KeyOrder;

        public string TxtSql;
        public string Name;
        public string Format;

        public string JoinTable;
        public string JoinColumn;
        public bool IsChild;

        public FieldValueType FieldValueType;
    }
    public class ExcellFldAttrAttribute : Attribute
    {
        public bool ExtraColumn;
        public FieldValueType FieldValueType;
    }

}
