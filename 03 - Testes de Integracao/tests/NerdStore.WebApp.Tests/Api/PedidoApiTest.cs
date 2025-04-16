using System.Net.Http.Json;
using Features.Tests;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Application.Services;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.MVC.Models;
using NerdStore.WebApp.Tests.Config;

namespace NerdStore.WebApp.Tests.Api;

[TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class PedidoApiTest
{
    private readonly IntegrationTestsFixture<StartupApiTests> _testsFixture;

    public PedidoApiTest(IntegrationTestsFixture<StartupApiTests> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Adicionar item em novo pedido"), TestPriority(1)]
    [Trait("Categoria", "Integração API - Pedido")]
    public async Task AdicionarItem_NovoPedido_DeveRetornarComSucesso()
    {
        // Arrange
        var item = new ItemViewModel
        {
            Id = new Guid("b21e5a57-89ab-cdef-0123-456789abcdef"),
            Quantidade = 2
        };
        
        await _testsFixture.RealizarLoginApi();
        _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);
        
        // Act 
        var response = await _testsFixture.Client.PostAsJsonAsync("api/carrinho", item);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Remover item em pedido existente"), TestPriority(2)]
    [Trait("Categoria", "Integração API - Pedido")]
    public async Task RemoverItem_PedidoExistente_DeveRetornarComSucesso()
    {
        // Arrange
        var productId = new Guid("b21e5a57-89ab-cdef-0123-456789abcdef");
        await _testsFixture.RealizarLoginApi(); 
        _testsFixture.Client.AtribuirToken(_testsFixture.UsuarioToken);
                
        // Act
        var response = await _testsFixture.Client.DeleteAsync($"api/carrinho/{productId}");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}