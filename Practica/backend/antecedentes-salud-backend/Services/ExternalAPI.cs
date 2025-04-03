using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using antecedentes_salud_backend.Models;
using Microsoft.Extensions.Logging;

public class ExternalAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalAPI> _logger;

    public ExternalAPI(HttpClient httpClient, ILogger<ExternalAPI> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Antecedentes>> GetFichasMedicasAsync()
    {
        try
        {
            _logger.LogInformation("Iniciando solicitud a {Url}", "http://ap2-salud.minenergia.qa/api/FichaMedica");
            var response = await _httpClient.GetAsync("http://ap2-salud.minenergia.qa/api/FichaMedica");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Contenido de la respuesta: {Content}", content);

            var fichasMedicas = JsonSerializer.Deserialize<List<Antecedentes>>(content);
            _logger.LogInformation("Número de fichas médicas recibidas: {Count}", fichasMedicas?.Count ?? 0);

            return fichasMedicas;
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "Error durante la solicitud HTTP");
            throw;
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Error durante la deserialización de JSON");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error inesperado");
            throw;
        }
    }

    public async Task<bool> PingAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://ap2-salud.minenergia.qa/api/FichaMedica");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al hacer ping a la API");
            return false;
        }
    }
}