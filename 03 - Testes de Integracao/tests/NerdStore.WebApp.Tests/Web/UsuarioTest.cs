using Features.Tests;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;

namespace NerdStore.WebApp.Tests.Web;

[TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
[Collection(nameof(IntegrationWebTestsFixtureCollection))]
public class UsuarioTest
{
    private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

    public UsuarioTest(IntegrationTestsFixture<StartupWebTests> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Realizar cadastro com sucesso"), TestPriority(1)]
    [Trait("Categoria", "Integração Web - Usuário")]
    public async Task Usuario_RealizarCadastro_DeveExecutarComSucesso()
    {
        // Arrange
        var url = "/Identity/Account/Register";
        var initialResponse = await _testsFixture.Client.GetAsync(url);
        initialResponse.EnsureSuccessStatusCode();
        
        var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());
        _testsFixture.GerarUserSenha();
        
        var formData = new Dictionary<string, string>
        {
            { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
            {"Input.Email", _testsFixture.UsuarioEmail },
            {"Input.Password", _testsFixture.UsuarioSenha },
            {"Input.ConfirmPassword", _testsFixture.UsuarioSenha }
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(formData)
        };
        
        // Act
        var response = await _testsFixture.Client.SendAsync(request);
        
        // Assert
        var responseString = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);
    }

    [Fact(DisplayName = "Realizar login com sucesso"), TestPriority(2)]
    [Trait("Categoria", "Integração Web - Usuário")]
    public async Task Usuario_RealizarLogin_DeveExecutarComSucesso()
    {
        // Arrange
        var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Login");
        initialResponse.EnsureSuccessStatusCode();
        
        var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

        var formData = new Dictionary<string, string>
        {
            {_testsFixture.AntiForgeryFieldName, antiForgeryToken},
            {"Input.Email", _testsFixture.UsuarioEmail},
            {"Input.Password", _testsFixture.UsuarioSenha}
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
        {
            Content = new FormUrlEncodedContent(formData)
        };
        
        // Act
        var response = await _testsFixture.Client.SendAsync(request);
        
        // Assert
        var responseString = await response.Content.ReadAsStringAsync();
        
        response.EnsureSuccessStatusCode();
        Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);
    }
    
    [Fact(DisplayName = "Realizar cadastro com senha fraca"), TestPriority(3)]
    [Trait("Categoria", "Integração Web - Usuário")]
    public async Task Usuario_RealizarCadastroComSenhaFraca_DeveRetornarMensagemDeErro()
    {
        // Arrange
        var url = "/Identity/Account/Register";
        var initialResponse = await _testsFixture.Client.GetAsync(url);
        initialResponse.EnsureSuccessStatusCode();
        
        var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());
        _testsFixture.GerarUserSenha();
        const string senhaFraca = "123456";
        
        var formData = new Dictionary<string, string>
        {
            { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
            {"Input.Email", _testsFixture.UsuarioEmail },
            {"Input.Password", senhaFraca },
            {"Input.ConfirmPassword", senhaFraca }
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(formData)
        };
        
        // Act
        var response = await _testsFixture.Client.SendAsync(request);
        
        // Assert
        var responseString = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        Assert.Contains("Passwords must have at least one non alphanumeric character.", responseString);
        Assert.Contains("Passwords must have at least one lowercase (&#x27;a&#x27;-&#x27;z&#x27;).", responseString);
        Assert.Contains("Passwords must have at least one uppercase (&#x27;A&#x27;-&#x27;Z&#x27;).", responseString);
    }
}
