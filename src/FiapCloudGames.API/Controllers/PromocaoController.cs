using FiapCloudGames.API.DTOs;
using FiapCloudGames.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromocaoController : ControllerBase
{
    private readonly IPromocaoService _promocaoService;

    public PromocaoController(IPromocaoService promocaoService)
    {
        _promocaoService = promocaoService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CadastrarPromocao([FromBody] CadastrarPromocaoDto dto)
    {
        var promocao = await _promocaoService.CriarPromocaoAsync(dto.Nome, dto.PercentualDeDesconto, dto.DataInicio, dto.DataFim);
        return CreatedAtAction(nameof(BuscarPorId), new { id = promocao.Id }, promocao);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var promocao = await _promocaoService.ObterPromocaoPorIdAsync(id);
        if (promocao == null) return NotFound();
        return Ok(promocao);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{promocaoId:guid}/jogos/{jogoId:guid}")]
    public async Task<IActionResult> AdicionarJogo(Guid promocaoId, Guid jogoId)
    {
        await _promocaoService.AdicionarJogoAPromocaoAsync(promocaoId, jogoId);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{promocaoId:guid}/jogos/{jogoId:guid}")]
    public async Task<IActionResult> RemoverJogo(Guid promocaoId, Guid jogoId)
    {
        await _promocaoService.RemoverJogoDaPromocaoAsync(promocaoId, jogoId);
        return NoContent();
    }

    [HttpGet("{promocaoId:guid}/jogos")]
    public async Task<IActionResult> BuscarJogosPorPromocao(Guid promocaoId)
    {
        var jogos = await _promocaoService.ListarJogosEmPromocaoAsync(promocaoId);
        return Ok(jogos);
    }

    [HttpGet("ativas")]
    public async Task<IActionResult> BuscarAtivas([FromQuery] DateTime? data = null)
    {
        var dataConsulta = data ?? DateTime.Now;
        var promocoes = await _promocaoService.ListarPromocoesAtivasAsync(dataConsulta);
        return Ok(promocoes);
    }
}