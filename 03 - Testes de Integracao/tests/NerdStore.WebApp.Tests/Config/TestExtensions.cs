using System.Net.Http.Headers;

namespace NerdStore.WebApp.Tests.Config;

public static class TestExtensions
{
    public static decimal ApenasNumeros(this string value)
    {
        return Convert.ToDecimal(new string(value.Where(char.IsDigit).ToArray()));
    }
    
    public static void AtribuirToken(this HttpClient client, string token)
    {
        client.AtribuirJsonMediaType();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    public static void AtribuirJsonMediaType(this HttpClient client)
    {
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}