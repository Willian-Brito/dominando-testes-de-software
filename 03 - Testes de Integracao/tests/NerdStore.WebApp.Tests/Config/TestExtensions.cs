namespace NerdStore.WebApp.Tests.Config;

public static class TestExtensions
{
    public static decimal ApenasNumeros(this string value)
    {
        return Convert.ToDecimal(new string(value.Where(char.IsDigit).ToArray()));
    }
}