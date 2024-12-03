using System;
using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Application.UseCases.CriarQrCodePagamento;

public interface ICriarQrCodePagamentoUseCase
{
    Task<CriarQrCodePagamentoUseCaseResult> Executar(CriarQrCodePagamentoUseCase dadosPagamento);

}
