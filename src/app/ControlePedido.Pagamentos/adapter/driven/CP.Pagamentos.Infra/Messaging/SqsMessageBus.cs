using System.Diagnostics.CodeAnalysis;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using CP.Pagamentos.CrossCutting.Factories;
using CP.Pagamentos.CrpssCutting.Configuration;
using CP.Pagamentos.Domain.Adapters.MessageBus;
using Microsoft.Extensions.Options;

namespace CP.Pagamentos.Infra.Messaging;

[ExcludeFromCodeCoverage]
public class SqsMessageBus : IMessageBus
{
    private readonly AWSConfiguration _configuration;

    public SqsMessageBus(IOptions<AWSConfiguration> aWSConfiguration)
    {
        _configuration = aWSConfiguration.Value;
    }

    public async Task PublishAsync<T>(T message, string queueUrl)
    {
        var config = new AmazonSQSConfig();

        if (!string.IsNullOrEmpty(_configuration.ServiceUrl))
            config.ServiceURL = _configuration.ServiceUrl;

        var client = new AmazonSQSClient(config.CreateCredentials(_configuration), config);

        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = message is string ? message.ToString() : System.Text.Json.JsonSerializer.Serialize(message)
        };

        await client.SendMessageAsync(sendMessageRequest);
    }
}
