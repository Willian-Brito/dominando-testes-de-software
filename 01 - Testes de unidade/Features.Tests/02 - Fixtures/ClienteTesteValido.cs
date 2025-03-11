﻿namespace Features.Tests.Fixtures;

[Collection(nameof(ClienteCollection))]
public class ClienteTesteValido
{
    private readonly ClienteTestsFixture _clienteTestsFixture;

    public ClienteTesteValido(ClienteTestsFixture clienteTestsFixture)
    {
        _clienteTestsFixture = clienteTestsFixture;
    }

    [Fact(DisplayName = "Novo Cliente Válido")]
    [Trait("Categoria", "Cliente Trait Tests")]
    public void Cliente_NovoCliente_DeveEstarValido()
    {
        // Arrange
        var cliente = _clienteTestsFixture.GerarClienteValido();

        // Act
        var result = cliente.EhValido();

        // Assert 
        Assert.True(result);
        Assert.Equal(0, cliente.ValidationResult.Errors.Count);
    }
}