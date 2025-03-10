namespace Demo.Tests;

public class AssertingExceptionsTests
{
    [Fact(DisplayName = "Deve retornar erro de divisão por zero")]
    public void Calculadora_Dividir_DeveRetornarErroDivisaoPorZero()
    {
        // Arrange
        var calculadora = new Calculadora();

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => calculadora.Dividir(10, 0));
    }


    [Fact(DisplayName = "Deve retornar erro de salario inferior permitido")]
    public void Funcionario_Salario_DeveRetornarErroSalarioInferiorPermitido()
    {
        // Arrange & Act & Assert
        var exception =
            Assert.Throws<Exception>(() => FuncionarioFactory.Criar("Eduardo", 250));

        Assert.Equal("Salario inferior ao permitido", exception.Message);
    }
}