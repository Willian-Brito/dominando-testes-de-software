﻿namespace Features.Tests.Fixtures;

[Collection(nameof(ClienteCollection))]
public class ClienteTesteInvalido
{
    private readonly ClienteTestsFixture _clienteTestsFixture;

    public ClienteTesteInvalido(ClienteTestsFixture clienteTestsFixture)
    {
        _clienteTestsFixture = clienteTestsFixture;
    }

    [Fact(DisplayName = "Novo Cliente Inválido")]
    [Trait("Categoria", "Cliente Trait Tests")]
    public void Cliente_NovoCliente_DeveEstarInvalido()
    {
        // Arrange
        var cliente = _clienteTestsFixture.GerarClienteInvalido();

        // Act
        var result = cliente.EhValido();

        // Assert 
        Assert.False(result);
        Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
    }
}
