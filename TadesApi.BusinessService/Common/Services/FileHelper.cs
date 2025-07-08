using System;
using System.IO;
using Microsoft.Extensions.Options;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.Core;

namespace TadesApi.BusinessService.Common.Services;

public class FileHelper : IFileHelper
{
    private readonly IOptions<FileSettings> _fileSetting;

    public FileHelper(IOptions<FileSettings> fileSetting)
    {
        _fileSetting = fileSetting;
    }

    public byte[] GetFileByteArray(string pdfPath)
    {
        byte[] fileByte;
        using (var ms = new MemoryStream(File.ReadAllBytes(pdfPath)))
        {
            fileByte = ms.ToArray();
        }

        return fileByte;
    }

    public void CreateFile(string filePath, byte[] byteArray)
    {
        using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        fs.Write(byteArray, 0, byteArray.Length);
    }

    public byte[] ConvertStringToByteArray(string fileDataStr)
    {
        var data = fileDataStr.Split("base64,");
        var lastLata = data[1];
        var byteData = Convert.FromBase64String(lastLata);
        return byteData;
    }

    public string GetExcellFilePath(int month, int year, long companyId)
    {
        var baseFilePath = _fileSetting.Value.ExcelFilePath;

        if (!Directory.Exists(baseFilePath))
            Directory.CreateDirectory(baseFilePath);

        var pathCompany = Path.Combine(baseFilePath, companyId.ToString());

        if (!Directory.Exists(pathCompany))
            Directory.CreateDirectory(pathCompany);

        var pathYear = Path.Combine(pathCompany, year.ToString());

        if (!Directory.Exists(pathYear))
            Directory.CreateDirectory(pathYear);

        var pathMonth = Path.Combine(pathYear, month.ToString());

        if (!Directory.Exists(pathMonth))
            Directory.CreateDirectory(pathMonth);

        return pathMonth;
    }
}