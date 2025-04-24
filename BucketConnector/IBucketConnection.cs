namespace BucketConnector;

public interface IBucketConnection
{
    Task<Stream> Download(string fileName);
    void Upload(string caminhoArquivo, string nomeArquivo);

    List<FileInformation> ListFiles();

    void Delete(string key);
}