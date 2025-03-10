namespace Demo.Tests;

public class AssertingObjectTypesTests
{
    [Fact(DisplayName = "Criar funcionario")]
    public void FuncionarioFactory_Criar_DeveRetornarTipoFuncionario()
    {
        // Arrange & Act
        var funcionario = FuncionarioFactory.Criar("Eduardo", 10000);

        // Assert
        Assert.IsType<Funcionario>(funcionario);
    }

    [Fact(DisplayName = "Criar Pessoa")]
    public void FuncionarioFactory_Criar_DeveRetornarTipoDerivadoPessoa()
    {
        // Arrange & Act
        var funcionario = FuncionarioFactory.Criar("Eduardo", 10000);

        // Assert
        Assert.IsAssignableFrom<Pessoa>(funcionario); // verifica se funcionario herda de pessoa
    }
}
