using System.Text.Json;

namespace FiapCloudGames.API.Middlewares;

public class LogDeRequisicaoMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogDeRequisicaoMiddleware> _logger;

    public LogDeRequisicaoMiddleware(RequestDelegate proximo, ILogger<LogDeRequisicaoMiddleware> logger)
    {
        _next = proximo;
        _logger = logger;
    }

    public async Task Invoke(HttpContext contexto)
    {
        contexto.Request.EnableBuffering();
        var corpoRequisicao = await new StreamReader(contexto.Request.Body).ReadToEndAsync();
        contexto.Request.Body.Position = 0;

        var nomeEndpoint = contexto.GetEndpoint()?.DisplayName ?? contexto.Request.Path;
        var corpoSanitizado = MascararCamposSensiveis(corpoRequisicao);

        _logger.LogInformation("Requisição: {Metodo} {Caminho}\nCorpo: {Corpo}",
            contexto.Request.Method,
            nomeEndpoint,
            string.IsNullOrWhiteSpace(corpoSanitizado) ? "" : corpoSanitizado);

        var corpoOriginal = contexto.Response.Body;
        using var corpoMemoria = new MemoryStream();
        contexto.Response.Body = corpoMemoria;

        await _next(contexto);

        contexto.Response.Body.Seek(0, SeekOrigin.Begin);
        var corpoResposta = await new StreamReader(contexto.Response.Body).ReadToEndAsync();
        contexto.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation("Resposta: {StatusCode}\nCorpo: {Corpo}",
            contexto.Response.StatusCode,
            string.IsNullOrWhiteSpace(corpoResposta) ? "" : corpoResposta);

        await corpoMemoria.CopyToAsync(corpoOriginal);
    }

    private string MascararCamposSensiveis(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return json;

        try
        {
            var documento = JsonDocument.Parse(json);
            var raiz = documento.RootElement;

            var campos = raiz.EnumerateObject()
                .ToDictionary(
                    prop => prop.Name,
                    prop => CamposSensiveis.Contains(prop.Name.ToLower())
                        ? "*****"
                        : prop.Value.ToString()
                );

            return JsonSerializer.Serialize(campos);
        }
        catch
        {
            return json;
        }
    }

    private static readonly HashSet<string> CamposSensiveis = new(StringComparer.OrdinalIgnoreCase)
    {
        "Password",
        "accessToken"
    };
}