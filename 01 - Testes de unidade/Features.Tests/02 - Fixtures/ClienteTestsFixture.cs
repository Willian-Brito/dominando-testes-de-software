using Features.Clientes;

namespace Features.Tests.Fixtures;

[CollectionDefinition(nameof(ClienteCollection))]
public class ClienteCollection : ICollectionFixture<ClienteTestsFixture> { }

public class ClienteTestsFixture : IDisposable
{
    public Cliente GerarClienteValido()
    {
        return new Cliente(
            Guid.NewGuid(),
            "Willian",
            "Brito",
            DateTime.Now.AddYears(-30),
            "will@edu.com",
            true,
            DateTime.Now);
    }

    public Cliente GerarClienteInvalido()
    {
        return new Cliente(
            Guid.NewGuid(),
            "",
            "",
            DateTime.Now,
            "edu2edu.com",
            true,
            DateTime.Now);
    }
 
    public void Dispose() { }
}
