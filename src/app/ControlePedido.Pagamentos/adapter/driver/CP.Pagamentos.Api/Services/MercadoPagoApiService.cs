using System;
using System.Security.Cryptography;
using System.Text;
using CP.Pagamentos.Infra.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CP.Pagamentos.Api.Services;

public interface IPaymentProviderRequestAuthorization
{
    bool IsValid(HttpRequest request);
}

public class MercadoPagoApiService : IPaymentProviderRequestAuthorization
{
    private readonly MercadoPagoIntegration _mercadoPagoIntegration;

    public MercadoPagoApiService(IOptions<MercadoPagoIntegration> options)
    {
        _mercadoPagoIntegration = options.Value;
    }
    
    public bool IsValid(HttpRequest request)
    {
        request.Headers.TryGetValue("x-signature", out StringValues xSignatureHeader);
        request.Headers.TryGetValue("x-request-id", out StringValues xRequestIdHeader);

        string xSignature = xSignatureHeader.FirstOrDefault();

        if (xSignature == null)
            return false;

        var parts = xSignature.Split(',');
        string ts = null;
        string hash = null;

        foreach (var part in parts)
        {
            var keyValue = part.Split('=');
            if (keyValue.Length == 2)
            {
                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();

                if (key == "ts")
                {
                    ts = value;
                }
                else if (key == "v1")
                {
                    hash = value;
                }
            }
        }

        if (string.IsNullOrEmpty(ts) || string.IsNullOrEmpty(hash))
            return false;

        // Obter o parâmetro de consulta "data.id"
        var queryParams = request.Query;
        string dataId = queryParams["data.id"];

        if (string.IsNullOrEmpty(dataId))
            return false;

        // Gerar o manifest string
        var manifest = $"id:{dataId};request-id:{xRequestIdHeader};ts:{ts};";

        // Gerar a assinatura HMAC
        var computedHash = ComputeHmacSha256(manifest, _mercadoPagoIntegration.SecretWebhook);

        // Validar o hash
        if (computedHash != hash)
            return false;

        return true;
    }

    private string ComputeHmacSha256(string data, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var dataBytes = Encoding.UTF8.GetBytes(data);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hashBytes = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}

