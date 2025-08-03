using FiapCloudGames.API.DTOs.Request;
using FiapCloudGames.API.DTOs.Response;
using FiapCloudGames.Core.Entities;
using FiapCloudGames.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JogoController : ControllerBase
{
    private readonly IJogoService _jogoService;

    public JogoController(IJogoService jogoService)
    {
        _jogoService = jogoService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CadastrarJogo([FromBody] CadastrarJogoDto dto)
    {
        var jogo = new Jogo(dto.Titulo, dto.Descricao, dto.Genero, dto.Preco);
        await _jogoService.AdicionarAsync(jogo);
        return CreatedAtAction(nameof(BuscarPorId), new { id = jogo.Id }, jogo);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var jogo = await _jogoService.BuscarPorIdAsync(id);
        if (jogo == null) return NotFound();
        return Ok(new JogoDTO(jogo));
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
    {
        var jogos = await _jogoService.BuscarTodosAsync();

        if(jogos == null) return NotFound();

        var listaJogosDto = new List<JogoDTO>();
        jogos.ToList().ForEach(j => listaJogosDto.Add(new JogoDTO(j)));

        return Ok(listaJogosDto);
    }
}