Feature: Receber pagamento via Mercado Pago
  Como um usuario eu quero receber um pagamento via mercado pago, para que eu consiga realizar meu pedido.

  Scenario: Receber um pagamento com sucesso
    Given que criado os dados de pagamento
    When for feita uma requisicao para a rota de webhook
    Then o status code da requisicao deve ser 201