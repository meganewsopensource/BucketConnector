using Amazon;
using Amazon.Runtime;
using Amazon.S3;

namespace BucketConnector;

public class CloudConnection 
{
    public static IAmazonS3 CloudConnectionCreate(string serviceUrl,  string accessKey, string secretAccessKey)
    {
        AWSConfigsS3.UseSignatureVersion4 = true;

        var amazonS3Config = new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            UseHttp = false,
            Timeout = TimeSpan.FromMinutes(10),
            ForcePathStyle = true,
            SignatureVersion = "4"
        };

        var basicAwsCredentials = new BasicAWSCredentials(accessKey, secretAccessKey);

        return new AmazonS3Client(basicAwsCredentials, amazonS3Config);
    }
}