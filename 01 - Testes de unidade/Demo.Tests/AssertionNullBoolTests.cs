namespace Demo.Tests;

public class AssertNullBoolTests
{
    [Fact(DisplayName = "Nome do funcionario não deve ser vazio")]
    public void Funcionario_Nome_NaoDeveSerNuloOuVazio()
    {
        // Arrange & Act
        var funcionario = new Funcionario("", 1000);

        // Assert
        Assert.False(string.IsNullOrEmpty(funcionario.Nome));
    }

    [Fact(DisplayName = "Apelido do funcioario não deve ser vazio")]
    public void Funcionario_Apelido_NaoDeveTerApelido()
    {
        // Arrange & Act
        var funcionario = new Funcionario("Eduardo", 1000);

        // Assert
        Assert.Null(funcionario.Apelido);

        // Assert Bool
        Assert.True(string.IsNullOrEmpty(funcionario.Apelido));
        Assert.False(funcionario.Apelido?.Length > 0);
    }
}
