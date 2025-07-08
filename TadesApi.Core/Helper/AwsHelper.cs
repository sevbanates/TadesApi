using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;

namespace TadesApi.Core.Helper;

public static class AwsHelper
{
    private static readonly RegionEndpoint _region = RegionEndpoint.USEast1;
    private static readonly string _accessKey = "AKIA47CRX6SSXGZTDSDF";
    private static readonly string _secretKey = "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";
    private static readonly string _bucketName = "gses";

    public static void UploadFileToS3(IFormFile file, string key)
    {
        using var client = new AmazonS3Client(_accessKey, _secretKey, _region);
        using var newMemoryStream = new MemoryStream();
        file.CopyTo(newMemoryStream);
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = key,
            BucketName = _bucketName,
            ContentType = file.ContentType,
        };
        var fileTransferUtility = new TransferUtility(client);
        fileTransferUtility.Upload(uploadRequest);
    }

    public static void DeleteFileFromS3(string key)
    {
        using var client = new AmazonS3Client(_accessKey, _secretKey, _region);
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };
        client.DeleteObjectAsync(request);
    }


    public static string GetFileUrl(string key)
    {
        using var client = new AmazonS3Client(_accessKey, _secretKey, _region);
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.Now.AddMinutes(10)
        };
        return client.GetPreSignedURL(request);
    }
}