using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Discord;

namespace HumansCompanion.Services;

public class AWSS3BucketService 
{
    private readonly AmazonS3Client client;

    public AWSS3BucketService() 
    {
        client = new AmazonS3Client(RegionEndpoint.USEast2);
    }

    public async Task<bool> UploadAttachment(IAttachment attachment, string bucketName, string objectName)
    {
        try
        {
            using HttpClient httpClient = new();
            byte[] imageData = await httpClient.GetByteArrayAsync(attachment.Url);

            using MemoryStream memoryStream = new(imageData);
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = memoryStream,
                CannedACL = S3CannedACL.PublicRead
            };

            var response = await client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Successfully uploaded {objectName} to {bucketName}.");
                return true;
            }
            else
            {
                Console.WriteLine($"Could not upload {objectName} to {bucketName}.");
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
            return false;
        }
    }
}