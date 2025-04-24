using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace BucketConnector;

public class BucketCreator : IBucketCreator
{
    private readonly IAmazonS3 _s3Client;

    private readonly ILogger<BucketCreator> _logger;

    public BucketCreator(IAmazonS3 s3Client, ILogger<BucketCreator> logger)
    {
        _s3Client = s3Client;
        _logger = logger;
    }

    public void CreateBucket(string bucketName)
    {
        try
        {
            var request = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            var response = _s3Client.PutBucketAsync(request).WaitAsync(CancellationToken.None);

            _logger.LogInformation($"Bucket '{bucketName}' criado com sucesso na região {response.Result}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao criar o bucket: {ex.Message}");
            throw;
        }
    }

    public bool BucketExists(string bucketName)
    {
        try
        {
            var response = _s3Client.ListBucketsAsync().WaitAsync(CancellationToken.None).Result;
            return response.Buckets.Any(b => b.BucketName.Equals(bucketName, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao verificar o bucket: {ex.Message}");
            throw;
        }
    }
}