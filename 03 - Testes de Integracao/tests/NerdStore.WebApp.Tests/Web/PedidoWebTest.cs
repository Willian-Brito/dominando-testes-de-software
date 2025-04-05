using AngleSharp.Html.Parser;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Application.Services;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;

namespace NerdStore.WebApp.Tests.Web;

[Collection(nameof(IntegrationWebTestsFixtureCollection))]
public class PedidoWebTest
{
    private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;
    private readonly IProdutoService _produtoService;
    
    public PedidoWebTest(IntegrationTestsFixture<StartupWebTests> testsFixture)
    {
        _testsFixture = testsFixture;
        _produtoService = _testsFixture.Services.GetRequiredService<IProdutoService>();
    }

    [Fact(DisplayName = "Adicionar item em novo pedido")]
    [Trait("Categoria", "Integração Web - Pedido")]
    public async Task AdicionarItem_NovoPedido_DeveAtualizarValorTotal()
    {
        // Arrange
        var produtoId = new Guid("b21e5a57-89ab-cdef-0123-456789abcdef");
        // var quantidade = 2;
        var produto = await _produtoService.ObterPorId(produtoId);
        
        var initialResponse = await _testsFixture.Client.GetAsync($"/produto-detalhe/{produtoId}");
        initialResponse.EnsureSuccessStatusCode();
        
        var formData = new Dictionary<string, string>
        {
            {"Id", produtoId.ToString()},
            {"quantidade", produto.QuantidadeEstoque.ToString()}
        };

        await _testsFixture.RealizarLoginWeb();
        
        var request = new HttpRequestMessage(HttpMethod.Post, "/meu-carrinho")
        {
            Content = new FormUrlEncodedContent(formData)
        };
        
        // Act
        var response = await _testsFixture.Client.SendAsync(request);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var html = new HtmlParser()
         .ParseDocumentAsync(await response.Content.ReadAsStringAsync())
         .Result
         .All;
        
        var formQuantidade = html?.FirstOrDefault(c => c.Id == "quantidade")?.GetAttribute("value")?.ApenasNumeros();
        var valorUnitario = html?.FirstOrDefault(c => c.Id == "valorUnitario")?.TextContent.Split(".")[0]?.ApenasNumeros();
        var valorTotal = html?.FirstOrDefault(c => c.Id == "valorTotal")?.TextContent.Split(".")[0]?.ApenasNumeros();
        
        Assert.Equal(valorTotal, valorUnitario * formQuantidade);
    }
}