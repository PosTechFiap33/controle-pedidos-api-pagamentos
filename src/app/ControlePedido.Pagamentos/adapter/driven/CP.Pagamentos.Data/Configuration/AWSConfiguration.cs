namespace CP.Pagamentos.Data.Configuration;

public class AWSConfiguration
{
    public string Region { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public DynamoDb DynamoDb { get; set; }
}

public class DynamoDb{
    public string ServiceUrl { get; set; }
}
