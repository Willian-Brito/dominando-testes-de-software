namespace Demo.Tests;

public class CalculadoraTests
{
    [Fact(DisplayName = "Deve retornar valor somado")]
    public void Calculadora_Somar_RetornarValorSoma()
    {
        //Arrange
        var calculadora = new Calculadora();

        //Act
        var resultado = calculadora.Somar(2, 2);

        //Assert
        Assert.Equal(4, resultado);
    }

    [Theory(DisplayName = "Deve retonar valores de somas")]
    [InlineData(1,1, 2)]
    [InlineData(0,1, 1)]
    public void Calculadora_Somar_RetornarValoresSomaCorretos(double v1, double v2, double total)
    {
        //Arrange
        var calculadora = new Calculadora();

        //Act
        var resultado = calculadora.Somar(v1, v2);

        //Assert
        Assert.Equal(total, resultado);
    }
}