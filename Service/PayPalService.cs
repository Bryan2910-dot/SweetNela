using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PayPal.Api;

namespace SweetNela.Service;

public class PayPalService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PayPalService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var clientId = _configuration["PayPal:ClientId"];
        var secret = _configuration["PayPal:Secret"];
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        var response = await _httpClient.PostAsync($"{_configuration["PayPal:BaseUrl"]}/v1/oauth2/token",
            new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") }));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al obtener el token de acceso: {response.StatusCode} - {error}");
        }

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(json);
        return data.GetProperty("access_token").GetString();
    }
    
    public async Task<string> CreatePaymentAsync(decimal amount, string currency, string returnUrl, string cancelUrl)
    {
        // Obtén el access token usando tu método actual
        var accessToken = await GetAccessTokenAsync();

        // Configura el HttpClient usando el token Bearer
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Crea el objeto de solicitud de pago según la API REST de PayPal
        var paymentRequest = new
        {
            intent = "sale",
            redirect_urls = new
            {
                return_url = returnUrl,
                cancel_url = cancelUrl
            },
            payer = new
            {
                payment_method = "paypal"
            },
            transactions = new[]
            {
                new
                {
                    amount = new
                    {
                        total = amount.ToString("F2"),
                        currency = currency
                    },
                    description = "Pago de SweetNela"
                }
            }
        };

        var requestJson = JsonSerializer.Serialize(paymentRequest);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        // Llama al endpoint para crear el pago
        var response = await _httpClient.PostAsync($"{_configuration["PayPal:BaseUrl"]}/v1/payments/payment", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al crear el pago: {response.StatusCode} - {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(jsonResponse);
        var root = document.RootElement;

        // En el objeto de respuesta "links" se encuentra la URL de aprobación
        if (root.TryGetProperty("links", out JsonElement links))
        {
            foreach (var link in links.EnumerateArray())
            {
                if (link.GetProperty("rel").GetString() == "approval_url")
                {
                    return link.GetProperty("href").GetString();
                }
            }
        }

        throw new Exception("No se pudo generar la URL de aprobación.");
    }

    public async Task<string> TestPayPalCredentialsAsync()
    {
        try
        {
            var token = await GetAccessTokenAsync();
            return $"Token obtenido exitosamente: {token}";
        }
        catch (Exception ex)
        {
            return $"Error al obtener el token: {ex.Message}";
        }
    }
}