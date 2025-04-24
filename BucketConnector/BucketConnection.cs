using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace BucketConnector;

public class BucketConnection : IBucketConnection
{
    private readonly IAmazonS3 _s3Client;

    private readonly string _bucketName;

    private readonly ILogger<BucketConnection> _logger;

    public BucketConnection(IAmazonS3 s3Client, string bucketName, ILogger<BucketConnection> logger)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
        _logger = logger;
    }

    public Task<Stream> Download(string fileName)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName 
            };
            
            return Task.FromResult(_s3Client.GetObjectAsync(request).Result.ResponseStream);
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Erro ao acessar o arquivo no S3: {ex.Message}");
            throw new AmazonS3Exception($"Erro ao acessar o arquivo no S3: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro interno: {ex.Message}");
            throw new Exception($"Erro interno: {ex.Message}");
        }
    }
    
    public void Upload(string caminhoArquivo, string nomeArquivo)
    {
        try
        {
            Stream fileStream = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read);
            var request = new PutObjectRequest()
            {
                BucketName = _bucketName,
                Key = nomeArquivo,
                InputStream = fileStream,
                UseChunkEncoding = false
            };

            var objectAsync = _s3Client.PutObjectAsync(request);
            
            objectAsync.Wait();
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Erro ao acessar o arquivo no S3: {ex.Message}");
            throw new AmazonS3Exception($"Erro ao acessar o arquivo no S3: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro interno: {ex.Message}");
            throw new Exception($"Erro interno: {ex.Message}");
        }
    }
    
    public List<FileInformation> ListFiles()
    {
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName
            };

            ListObjectsV2Response response;

            List<FileInformation> list = new List<FileInformation>();
            do
            {
                response = _s3Client.ListObjectsV2Async(request).Result;
                
                response.S3Objects
                    .ForEach(obj => 
                        list.Add(new FileInformation(obj.Key, obj.LastModified, obj.Size))); 
                
                request.ContinuationToken = response.NextContinuationToken;
            }
            while (response.IsTruncated);

            return list;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Ocorreu um erri. Message:'{ex.Message}' ao buscar a lista de objetos.");
            throw;
        }
    }

    public void Delete(string key)
    {
        try
        {
            var request = new DeleteObjectRequest()
            {
                BucketName = _bucketName,
                Key = key
            };
            var objectAsync =_s3Client.DeleteObjectAsync(request);
            objectAsync.Wait();    
        } catch (AmazonS3Exception ex)
        {
            _logger.LogError($"Error encountered on server. Message:'{ex.Message}' deleting a object.");
            throw;
        }
    }
}