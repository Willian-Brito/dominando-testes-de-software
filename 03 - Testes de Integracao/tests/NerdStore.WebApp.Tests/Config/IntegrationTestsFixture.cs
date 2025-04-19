using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.MVC.Models;

namespace NerdStore.WebApp.Tests.Config;

[CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupWebTests>> { }

[CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupApiTests>> { }

public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
{
    public string AntiForgeryFieldName = "__RequestVerificationToken";
    public string UsuarioEmail;
    public string UsuarioSenha;
    public string UsuarioToken;
    
    public readonly LojaAppFactory<TStartup> Factory;
    public HttpClient Client;
    public IServiceProvider Services;

    public IntegrationTestsFixture()
    {
        var clientOptions = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            BaseAddress = new Uri("http://localhost"),
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        };

        Factory = new LojaAppFactory<TStartup>();
        Client = Factory.CreateClient(clientOptions);
        Services = Factory.Services;
    }
    
    public void GerarUserSenha()
    {
        var faker = new Faker("pt_BR");
        UsuarioEmail = faker.Internet.Email().ToLower();
        UsuarioSenha = faker.Internet.Password(8, false, "", "@1Ab_");
    }

    public async Task RealizarLoginApi()
    {
        await CriarUsuario();
        await LoginAPI();
    }
    
    public async Task RealizarLoginWeb()
    {
        await CriarUsuario();
        await LoginWeb();
    }

    private async Task LoginAPI()
    {
        var userData = new LoginViewModel
        {
            Email = UsuarioEmail,
            Senha = UsuarioSenha
        };
        
        // Recriando o client para evitar configurações de Web
        Client = Factory.CreateClient();
        
        var response = await Client.PostAsJsonAsync("api/login", userData);
        response.EnsureSuccessStatusCode();
        UsuarioToken = await response.Content.ReadAsStringAsync();
    }
    
    private async Task LoginWeb()
    {
        var url = "/Identity/Account/Login";
        var formData = new Dictionary<string, string>
        {
            {"Input.Email", UsuarioEmail},
            {"Input.Password", UsuarioSenha}
        };

        await GerarRequisicao(url, HttpMethod.Post, formData);
    }

    private async Task CriarUsuario()
    {
        UsuarioEmail = "teste@teste.com";
        UsuarioSenha = "#Teste@123";
        
        var userManager = Services.GetRequiredService<UserManager<IdentityUser>>();
        
        if (userManager.FindByEmailAsync(UsuarioEmail).Result == null)
        {
            var url = "/Identity/Account/Register";
                
            var formData = new Dictionary<string, string>
            {
                {"Input.Email", UsuarioEmail},
                {"Input.Password", UsuarioSenha},
                {"Input.ConfirmPassword", UsuarioSenha}
            };

            await GerarRequisicao(url, HttpMethod.Post, formData);
        }
    }

    private async Task GerarRequisicao(string url, HttpMethod method, Dictionary<string, string> formData)
    {
        var initialResponse = await Client.GetAsync(url);
        initialResponse.EnsureSuccessStatusCode();
        
        var antiForgeryToken = ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());
        
        formData.Add(AntiForgeryFieldName, antiForgeryToken);
        
        var request = new HttpRequestMessage(method, url)
        {
            Content = new FormUrlEncodedContent(formData)
        };
        
        var response = await Client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public string ObterAntiForgeryToken(string htmlBody)
    {
        var requestVerificationTokenMatch =
            Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

        if (requestVerificationTokenMatch.Success)
            return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
        
        throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' não encontrado no HTML", nameof(htmlBody));
    }
    
    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}