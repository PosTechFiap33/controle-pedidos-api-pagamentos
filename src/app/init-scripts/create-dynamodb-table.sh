#!/bin/bash

# Espera o LocalStack iniciar e estar pronto
echo "Aguardando o LocalStack iniciar..."
until awslocal dynamodb list-tables; do
    sleep 5
done

echo "LocalStack iniciado. Criando a tabela DynamoDB..."

# Cria a tabela no DynamoDB
awslocal dynamodb create-table  --table-name ControlePedidosPagamentos  --attribute-definitions AttributeName=PedidoId,AttributeType=S  --key-schema AttributeName=PedidoId,KeyType=HASH --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5

# Lista dados do dynamodb
awslocal dynamodb scan --table-name ControlePedidosPagamentos

# Cria a queue no SQS
awslocal sqs create-queue --queue-name ControlePedidosPagamentos --attributes MessageRetentionPeriod=345600

# Listar mensagens no SQS
awslocal sqs receive-message --queue-url http://localhost:4566/000000000000/ControlePedidosPagamentos --max-number-of-messages 10

#publicar mensagem no SQS
awslocal sqs send-message --queue-url http://localhost:4566/000000000000/ControlePedidosPagamentos --message-body '{"PedidoId":"7ea82f18-8bdc-48a3-84ed-50d242ca7c38","PagamentoId":"987f6543-e21b-34c5-d678-567812340000"}'

echo "Tabela criada com sucesso!"
