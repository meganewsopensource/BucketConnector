namespace BucketConnector;

public interface IBucketCreator
{
    void CreateBucket(string bucketName);

    bool BucketExists(string bucketName);
}