﻿namespace Features.Tests.Order;

/// <summary>
/// Essa ordenação de testes e definição de quais serão executados primeiro,
/// deve ser feita somente se necessário e é mais utilizada em testes de
/// integração. Exemplo, você quer validar um login e possui 3 testes de unidade,
/// uma para verificar se o email foi preenchido, outra para verificar a senha e 
/// outra para fazer o login. Nesse caso o teste de login precisa ser o ultimo a 
/// ser executado, então é possivel fazer isso utilizando o exemplo abaixo
/// </summary>
[TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
public class OrdemTestes
{
    public static bool Teste1Chamado;
    public static bool Teste2Chamado;
    public static bool Teste3Chamado;
    public static bool Teste4Chamado;

    [Fact(DisplayName = "Teste 04"), TestPriority(3)]
    [Trait("Categoria", "Ordenacao Testes")]
    public void Teste04()
    {
        Teste4Chamado = true;

        Assert.True(Teste3Chamado);
        Assert.True(Teste1Chamado);
        Assert.False(Teste2Chamado);
    }

    [Fact(DisplayName = "Teste 01"), TestPriority(2)]
    [Trait("Categoria", "Ordenacao Testes")]
    public void Teste01()
    {
        Teste1Chamado = true;

        Assert.True(Teste3Chamado);
        Assert.False(Teste4Chamado);
        Assert.False(Teste2Chamado);
    }

    [Fact(DisplayName = "Teste 03"), TestPriority(1)]
    [Trait("Categoria", "Ordenacao Testes")]
    public void Teste03()
    {
        Teste3Chamado = true;

        Assert.False(Teste1Chamado);
        Assert.False(Teste2Chamado);
        Assert.False(Teste4Chamado);
    }

    [Fact(DisplayName = "Teste 02"), TestPriority(4)]
    [Trait("Categoria", "Ordenacao Testes")]
    public void Teste02()
    {
        Teste2Chamado = true;

        Assert.True(Teste3Chamado);
        Assert.True(Teste4Chamado);
        Assert.True(Teste1Chamado);
    }
}
