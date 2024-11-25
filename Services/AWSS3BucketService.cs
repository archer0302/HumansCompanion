using Amazon.S3;
using Amazon.S3.Model;
using Discord;

namespace HumansCompanion.Services;

public class AWSS3BucketService 
{
    IAmazonS3 client;

    public AWSS3BucketService() 
    {
        client = new AmazonS3Client();
    }

    public async Task<bool> UploadAttachment(IAttachment attachment, string bucketName, string objectName)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectName,
        };
        return true;
    }
}