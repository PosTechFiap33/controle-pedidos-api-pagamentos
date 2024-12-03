using System;

namespace CP.Pagamentos.Domain.Adapters.MessageBus;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, string topicOrQueue);
}
