using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using CP.Pagamentos.Data.Mappings;
using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.DomainObjects;
using CP.Pagamentos.Domain.Entities;
using Microsoft.Extensions.Options;
using CP.Pagamentos.CrpssCutting.Configuration;
using CP.Pagamentos.CrossCutting.Factories;
using System.Diagnostics.CodeAnalysis;

namespace CP.Pagamentos.Data;

[ExcludeFromCodeCoverage]
public class PagamentoDynamoDbContext : IUnitOfWork
{
    private readonly AmazonDynamoDBClient _client;
    private readonly Dictionary<TransactWriteItem, IDomainNotification> _writeOperations;
    private readonly IDomainNotificationEmmiter _notificationHandler;
    // private readonly ILogger<PagamentoDynamoDbContext> _logger;

    public PagamentoDynamoDbContext(IOptions<AWSConfiguration> configuration,
                                    IDomainNotificationEmmiter domainNotificationHandler/*,
                                    ILogger<PagamentoDynamoDbContext> logger*/)
    {
        // _logger = logger;

        _notificationHandler = domainNotificationHandler;

        var awsConfiguration = configuration.Value;

        var config = new AmazonDynamoDBConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsConfiguration.Region),
        };

        _client = new AmazonDynamoDBClient(config.CreateCredentials(awsConfiguration), config);

        _writeOperations = new Dictionary<TransactWriteItem, IDomainNotification>();
    }

    public async Task<bool> Commit()
    {
        if (!_writeOperations.Any())
            return true;

        var transactRequest = new TransactWriteItemsRequest
        {
            TransactItems = _writeOperations.Keys.ToList()
        };

        var response = await _client.TransactWriteItemsAsync(transactRequest);

        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            //    _logger.LogError("Transação do banco falhou com status {StatusCode}", response.HttpStatusCode);
            return false;
        }

        var notificationTasks = _writeOperations.Values
            .Select(notification => _notificationHandler.SendNotification(notification))
            .ToArray();

        await Task.WhenAll(notificationTasks);

        return true;
    }

    public void Add<T>(IDynamoEntity<T> dynamoEntity, string tableName) where T : Entity, IAggregateRoot
    {
        _writeOperations.Add(new TransactWriteItem
        {
            Put = new Put
            {
                TableName = tableName,
                Item = dynamoEntity.MapToDynamo()
            }
        }, dynamoEntity.Entity.Notification);
    }

    public void Dispose()
    {
        _writeOperations.Clear();
    }
}
