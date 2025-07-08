using System;

namespace TadesApi.Core;

public class AppConfigs
{
    public AppConfigs()
    {
        PrivateKey = "2909012565820034"; //TODO Db den alınacak 
    }

    public string Files_root { get; set; }
    public string Temp_root { get; set; }

    public string RabbitMqPassword { get; set; }
    public string RedisConnectionString { get; set; }
    public string PrivateKey { get; set; }
    public string AppCode { get; set; }
}

public class FileSettings
{
    public string DefaultFilePath { get; set; }
    public string DefaultTempPath { get; set; }
    public string ExcelFilePath { get; set; }
    public string CompanyLogoFiles { get; set; }
    public string UserLogoFiles { get; set; }
    public string ProductImages { get; set; }
    public string SignedContracts { get; set; }
    public string SignedSupplierOrderItemContracts { get; set; }
    public string TemplatePath { get; set; }

    public string ResourceExcelFolder => $"{Environment.CurrentDirectory}\\Resource\\ExcellFile";
    public string ResourceContractFile => $"{Environment.CurrentDirectory}\\Resource\\Template\\contract.pdf";
    public string ResourcePrimeIconsFile => $"{Environment.CurrentDirectory}\\Resource\\JSON\\prime-icon.json";
    public string ResourcePrimeButtonClassFile => $"{Environment.CurrentDirectory}\\Resource\\JSON\\prime-button-class.json";

    public string UserDefaultImage => $"{Environment.CurrentDirectory}\\Resource\\Images\\userDefaultAvatar.png";
    public string ProductDefaultImage => $"{Environment.CurrentDirectory}\\Resource\\Images\\productDefaultImage.png";
    public string CompanyDefaultImage => $"{Environment.CurrentDirectory}\\Resource\\Images\\companyDefaultImage.png";
}

public class AwsSettings
{
    public string S3AccessKey { get; set; }
    public string S3SecretKey { get; set; }
    public string S3BucketName { get; set; }
    public string S3Region { get; set; }
}