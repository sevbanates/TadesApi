using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core;

namespace TadesApi.BusinessService.Common.Interfaces
{
    public interface IFileHelper
    {
        string GetExcellFilePath(int month, int year, long companyId);
        byte[] GetFileByteArray(string pdfPath);
        void CreateFile(string filePath, byte[] byteArray);
        byte[] ConvertStringToByteArray(string fileDataStr);

    }
}
