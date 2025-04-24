namespace BucketConnector;

public class FileInformation
{
    private readonly string _key;

    private readonly DateTime _dataEnvio;

    private readonly long _tamanho;
    
    public FileInformation(string key, DateTime dataEnvio, long tamanho)
    {
        _key = key;
        _dataEnvio = dataEnvio;
        _tamanho = tamanho;
    }

    public override string ToString()
    {
        return _key;
    }
    
    public FileInformationDTO ParaDto()
    {
        return new FileInformationDTO()
        {
            dataEnvio = _dataEnvio,
            key = _key,
            tamanho = _tamanho
        };
    }
}