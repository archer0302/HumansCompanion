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

    public async Task<Stream> GetObjectStream(string bucketName, string objectName)
    {
        // Create a GetObject request
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = objectName,
        };

        // Issue request and remember to dispose of the response
        GetObjectResponse response = await client.GetObjectAsync(request);

        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            return response.ResponseStream;
        }
        else
        {
            throw new ArgumentException($"Getting object {objectName} failed.");
        }
    }
}