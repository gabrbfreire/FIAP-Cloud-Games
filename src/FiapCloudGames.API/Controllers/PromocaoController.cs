using FiapCloudGames.API.DTOs;
using FiapCloudGames.API.DTOs.Request;
using FiapCloudGames.API.DTOs.Response;
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
        return Ok(new PromocaoDTO(promocao));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("adicionar-jogo")]
    public async Task<IActionResult> AdicionarJogo([FromBody] AdicionarRemoverJogoPromocaoDto dto)
    {
        await _promocaoService.AdicionarJogoAPromocaoAsync(dto.PromocaoId, dto.JogoId);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("remover-jogo")]
    public async Task<IActionResult> RemoverJogo([FromBody] AdicionarRemoverJogoPromocaoDto dto)
    {
        await _promocaoService.RemoverJogoDaPromocaoAsync(dto.PromocaoId, dto.JogoId);
        return NoContent();
    }

    [HttpGet("{promocaoId:guid}/jogos")]
    public async Task<IActionResult> BuscarJogosPorPromocao(Guid promocaoId)
    {
        var jogos = await _promocaoService.ListarJogosEmPromocaoAsync(promocaoId);

        if (jogos == null) return NotFound();

        var listaJogosDto = new List<JogoDTO>();
        jogos.ToList().ForEach(j => listaJogosDto.Add(new JogoDTO(j)));

        return Ok(listaJogosDto);
    }

    [HttpGet("ativas")]
    public async Task<IActionResult> BuscarAtivas([FromQuery] DateTime? data = null)
    {
        var dataConsulta = data ?? DateTime.Now;
        var promocoes = await _promocaoService.ListarPromocoesAtivasAsync(dataConsulta);

        if (promocoes == null) return NotFound();

        var listaPromocoesDto = new List<PromocaoDTO>();
        promocoes.ToList().ForEach(p => listaPromocoesDto.Add(new PromocaoDTO(p)));

        return Ok(listaPromocoesDto);
    }
}