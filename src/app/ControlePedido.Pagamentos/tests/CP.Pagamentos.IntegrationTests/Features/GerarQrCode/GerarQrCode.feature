Feature: Gerar QR Code para pagamento via Mercado Pago
  Como um usuario eu quero gerar um QR Code para pagamento via Mercado Pago
  e entao completar o processo de pagamento para comprar um produto.

  Scenario: Gerar um QR Code com sucesso
    Given que seja criada uma lista de itens para um pedido
      And que a lista contenha um item com os seguintes dados:
        | Nome   | Descrição           | Preço  | Quantidade |
        | Teste  | Segue uma descrição | 10.00  | 1          |
    When uma requisição for enviada para a API de geração de QR Code
    Then o status da resposta deverá ser 201 (Created)
      And a resposta deverá conter o QR Code gerado
