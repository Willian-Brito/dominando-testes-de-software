namespace Demo.Tests;

public class AssertStringsTests
{
    [Fact(DisplayName = "Deve retornar nome completo")]
    public void StringsTools_UnirNomes_RetornarNomeCompleto()
    {
        // Arrange
        var sut = new StringsTools();

        // Act
        var nomeCompleto = sut.Unir("Willian", "Brito");

        // Assert
        Assert.Equal("Willian Brito", nomeCompleto);
    }



    [Fact(DisplayName = "Deve ignorar case")]
    public void StringsTools_UnirNomes_DeveIgnorarCase()
    {
        // Arrange
        var sut = new StringsTools();

        // Act
        var nomeCompleto = sut.Unir("Willian", "Brito");

        // Assert
        Assert.Equal("Willian Brito", nomeCompleto, true);
    }



    [Fact(DisplayName = "Deve conter trecho de string")]
    public void StringsTools_UnirNomes_DeveConterTrecho()
    {
        // Arrange
        var sut = new StringsTools();

        // Act
        var nomeCompleto = sut.Unir("Willian", "Brito");

        // Assert
        Assert.Contains("lian", nomeCompleto);
    }


    [Fact(DisplayName = "Deve começar com")]
    public void StringsTools_UnirNomes_DeveComecarCom()
    {
        // Arrange
        var sut = new StringsTools();

        // Act
        var nomeCompleto = sut.Unir("Willian", "Brito");

        // Assert
        Assert.StartsWith("Will", nomeCompleto);
    }


    [Fact(DisplayName = "Deve acabar com")]
    public void StringsTools_UnirNomes_DeveAcabarCom()
    {
        // Arrange
        var sut = new StringsTools();

        // Act
        var nomeCompleto = sut.Unir("Willian", "Brito");

        // Assert
        Assert.EndsWith("ito", nomeCompleto);
    }


    [Fact(DisplayName = "Validar expressão regular")]
    public void StringsTools_UnirNomes_ValidarExpressaoRegular()
    {
        // Arrange
        var sut = new StringsTools();

        // Act
        var nomeCompleto = sut.Unir("Willian", "Brito");

        // Assert
        Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", nomeCompleto);
    }
}
