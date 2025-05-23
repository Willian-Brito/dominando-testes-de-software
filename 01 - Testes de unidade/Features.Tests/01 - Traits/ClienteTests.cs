﻿using Features.Clientes;

namespace Features.Tests.Traits;

public class ClienteTests
{
    [Fact(DisplayName = "Novo Cliente Válido")]
    [Trait("Categoria", "Cliente Trait Tests")]
    public void Cliente_NovoCliente_DeveEstarValido()
    {
        // Arrange
        var cliente = new Cliente(
            Guid.NewGuid(),
            "Willian",
            "Brito",
            DateTime.Now.AddYears(-30),
            "will@edu.com",
            true,
            DateTime.Now
        );

        // Act
        var result = cliente.EhValido();

        // Assert 
        Assert.True(result);
        Assert.Equal(0, cliente.ValidationResult.Errors.Count);
    }

    [Fact(DisplayName = "Novo Cliente Inválido")]
    [Trait("Categoria", "Cliente Trait Tests")]
    public void Cliente_NovoCliente_DeveEstarInvalido()
    {
        // Arrange
        var cliente = new Cliente(
            Guid.NewGuid(),
            "",
            "",
            DateTime.Now,
            "will2edu.com",
            true,
            DateTime.Now
        );

        // Act
        var result = cliente.EhValido();

        // Assert 
        Assert.False(result);
        Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
    }
}
