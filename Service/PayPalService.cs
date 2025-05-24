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
        // Crear el contexto de la API de PayPal
        var apiContext = new APIContext(await GetAccessTokenAsync())
        {
            Config = new Dictionary<string, string>
            {
                { "mode", _configuration["PayPal:Mode"] } // "sandbox" o "live"
            }
        };

        // Configurar el objeto de pago con la moneda especificada
        var payment = new Payment
        {
            intent = "sale",
            payer = new Payer { payment_method = "paypal" },
            transactions = new List<Transaction>
            {
                new Transaction
                {
                    amount = new Amount
                    {
                        currency = currency, // Usar la moneda especificada (PEN)
                        total = amount.ToString("F2") // Formatear el monto con dos decimales
                    },
                    description = "SweetNela"
                }
            },
            redirect_urls = new RedirectUrls
            {
                return_url = returnUrl,
                cancel_url = cancelUrl
            }
        };

        // Crear el pago en PayPal
        var createdPayment = payment.Create(apiContext);

        // Obtener la URL de aprobación
        var approvalUrl = createdPayment.links.FirstOrDefault(link => link.rel == "approval_url")?.href;

        return approvalUrl ?? throw new Exception("No se pudo generar la URL de aprobación.");
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