﻿using Features.Clientes;
using MediatR;
using Moq;
using static Features.Tests.DadosHumanos.ClienteBogusTestsFixture;

namespace Features.Tests.Mock;

[Collection(nameof(ClienteBogusCollection))]
public class ClienteServiceTests
{
    readonly ClienteTestsBogusFixture _clienteTestsBogus;

    public ClienteServiceTests(ClienteTestsBogusFixture clienteTestsFixture)
    {
        _clienteTestsBogus = clienteTestsFixture;
    }

    [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
    [Trait("Categoria", "Cliente Service Mock Tests")]
    public void ClienteService_Adicionar_DeveExecutarComSucesso()
    {
        // Arrange
        var cliente = _clienteTestsBogus.GerarClienteValido();
        var clienteRepository = new Mock<IClienteRepository>();
        var mediator = new Mock<IMediator>();

        var clienteService = new ClienteService(clienteRepository.Object, mediator.Object);

        // Act
        clienteService.Adicionar(cliente);

        // Assert
        Assert.True(cliente.EhValido());
        clienteRepository.Verify(r => r.Adicionar(cliente), Times.Once);
        mediator.Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
    }
    [Fact(DisplayName = "Adicionar Cliente com Falha")]
    [Trait("Categoria", "Cliente Service Mock Tests")]
    public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
    {
        // Arrange
        var cliente = _clienteTestsBogus.GerarClienteInvalido();
        var clienteRepo = new Mock<IClienteRepository>();
        var mediatr = new Mock<IMediator>();

        var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

        // Act
        clienteService.Adicionar(cliente);

        // Assert
        Assert.False(cliente.EhValido());
        clienteRepo.Verify(r => r.Adicionar(cliente), Times.Never);
        mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
    }

    [Fact(DisplayName = "Obter Clientes Ativos")]
    [Trait("Categoria", "Cliente Service Mock Tests")]
    public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
    {
        // Arrange
        var clienteRepo = new Mock<IClienteRepository>();
        var mediatr = new Mock<IMediator>();

        clienteRepo.Setup(c => c.ObterTodos())
            .Returns(_clienteTestsBogus.ObterClientesVariados());

        var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

        // Act
        var clientes = clienteService.ObterTodosAtivos();

        // Assert 
        clienteRepo.Verify(r => r.ObterTodos(), Times.Once);
        Assert.True(clientes.Any());
        Assert.False(clientes.Count(c => !c.Ativo) > 0);
    }
}