using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.CoreHelper
{
    public static class ExcelImport
    {
        public static string CheckImportValidate<T>(string[] colnames)
        {

            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var attribute = CommonFunctions.GetExcellFieldAttrib(prop);

                if (attribute != null)
                    if (attribute.ExtraColumn)
                        continue;

                var ind = Array.IndexOf(colnames, prop.Name);

                if (ind < 0)
                    return "Excell " + prop.Name + "Alanı bulunamadı";

            }

            return "";
        }
        public static object GetExcelCellValue(string[] colvals, string[] colnames, string cellName)
        {
            return colvals[Array.IndexOf(colnames, cellName)];

        }

    }

}
