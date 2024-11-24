using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CP.Pagamentos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PagamentoManualController : MainController
{
    public PagamentoManualController(ILogger<PagamentoManualController> logger) : base(logger)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromServices] IPagamentoRepository repository)
    {
        repository.Criar(new Pagamento(Guid.NewGuid(), 20, Guid.NewGuid().ToString(), "MANUAL", DateTime.UtcNow));
        await repository.UnitOfWork.Commit();
        return Ok();
    }
}
