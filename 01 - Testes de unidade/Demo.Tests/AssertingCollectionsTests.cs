namespace Demo.Tests;

public class AssertingCollectionsTests
{
    [Fact(DisplayName = "Funcionario não deve possuir habilidades vazias")]
    public void Funcionario_Habilidades_NaoDevePossuirHabilidadesVazias()
    {
        // Arrange & Act
        var funcionario = FuncionarioFactory.Criar("Eduardo", 10000);

        // Assert
        // Testa toda a coleção de habilidades, item a item
        Assert.All(funcionario.Habilidades, habilidade => Assert.False(string.IsNullOrWhiteSpace(habilidade)));
    }

    [Fact(DisplayName = "Funcinario junior deve possuir habilidades básicas")]
    public void Funcionario_Habilidades_JuniorDevePossuirHabilidadeBasica()
    {
        // Arrange & Act
        var funcionario = FuncionarioFactory.Criar("Eduardo", 1000);

        // Assert
        Assert.Contains("OOP", funcionario.Habilidades);
    }


    [Fact(DisplayName = "Funcionario junior não deve possuir habilidades avançadas")]
    public void Funcionario_Habilidades_JuniorNaoDevePossuirHabilidadeAvancada()
    {
        // Arrange & Act
        var funcionario = FuncionarioFactory.Criar("Eduardo", 1000);

        // Assert
        Assert.DoesNotContain("Microservices", funcionario.Habilidades);
    }


    [Fact(DisplayName = "Funcionario senior deve possuir todas habilidades")]
    public void Funcionario_Habilidades_SeniorDevePossuirTodasHabilidades()
    {
        // Arrange & Act
        var funcionario = FuncionarioFactory.Criar("Eduardo", 15000);

        var habilidadesBasicas = new[]
        {
            "Lógica de Programação",
            "OOP",
            "Testes",
            "Microservices"
        };

        // Assert
        Assert.Equal(habilidadesBasicas, funcionario.Habilidades);
    }
}
