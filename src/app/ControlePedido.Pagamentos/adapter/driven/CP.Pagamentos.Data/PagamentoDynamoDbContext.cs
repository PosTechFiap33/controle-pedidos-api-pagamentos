using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using CP.Pagamentos.Data.Configuration;
using CP.Pagamentos.Domain.Adapters.Repositories;
using Microsoft.Extensions.Options;

namespace CP.Pagamentos.Data;

public class PagamentoDynamoDbContext : IUnitOfWork
{
    private readonly DynamoDBContext _context;
    private readonly AmazonDynamoDBClient _client;
    private readonly List<TransactWriteItem> _writeOperations;


    public PagamentoDynamoDbContext(IOptions<AWSConfiguration> configuration)
    {
        var awsConfiguration = configuration.Value;

        var credentials = new BasicAWSCredentials(awsConfiguration.AccessKey, awsConfiguration.SecretKey);
        
        var config = new AmazonDynamoDBConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsConfiguration.Region),
            ServiceURL = awsConfiguration.DynamoDb.ServiceUrl,
        };

        _client = new AmazonDynamoDBClient(credentials, config);
        _context = new DynamoDBContext(_client);
        _writeOperations = new List<TransactWriteItem>();
    }

    public async Task<bool> Commit()
    {
        if (_writeOperations.Any())
        {
            var transactRequest = new TransactWriteItemsRequest
            {
                TransactItems = _writeOperations
            };

            var response = await _client.TransactWriteItemsAsync(transactRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        return false;
    }

    public void Add(Dictionary<string, AttributeValue> values, string tableName)
    {
        _writeOperations.Add(new TransactWriteItem
        {
            Put = new Put
            {
                TableName = tableName,
                Item = values
            }
        });
    }

    public void Dispose()
    {
        _writeOperations.Clear();
    }
}
