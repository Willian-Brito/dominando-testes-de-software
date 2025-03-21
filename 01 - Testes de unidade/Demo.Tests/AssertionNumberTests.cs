﻿namespace Demo.Tests;

public class AssertNumbersTests
{
    [Fact(DisplayName = "Soma deve ser igual")]
    public void Calculadora_Somar_DeveSerIgual()
    {
        // Arrange
        var calculadora = new Calculadora();

        // Act
        var result = calculadora.Somar(1, 2);

        // Assert
        Assert.Equal(3, result);
    }

    [Fact(DisplayName = "Soma não de ve ser igual")]
    public void Calculadora_Somar_NaoDeveSerIgual()
    {
        // Arrange
        var calculadora = new Calculadora();

        // Act
        var result = calculadora.Somar(1.13123123123, 2.2312313123);

        // Assert
        Assert.NotEqual(3.3, result, 1);
    }
}