# 🧪 Dominando Testes de Software

Este projeto reúne exemplos práticos e organizados de como implementar diversos tipos de testes em aplicações .NET, cobrindo desde testes unitários até testes de carga. O objetivo deste projeto é ensinar, demonstrar e praticar testes de software de forma abrangente, cobrindo diferentes camadas e técnicas essenciais para garantir a qualidade em projetos reais.

## 📌 Conteúdo

### ✅ Testes de Unidade

- `Traits` – Agrupamento de testes por categoria.
- `Fixtures` – Compartilhamento de contexto entre testes.
- `Order` – Definição da ordem de execução.
- `Bogus` – Geração de dados aleatórios para cenários de teste.
- `Mocks` – Simulação de dependências com Moq e AutoMock.
- `FluentAssertions` – Verificações legíveis e fluentes.
- `Skips` – Ignorar testes específicos sob certas condições.
- `TDD` – Desenvolvimento orientado a testes (Test Driven Development).


### 🔗 Testes de Integração

- `Appsettings` por ambiente – Configuração de ambiente de teste.
- Testes em aplicações Web com `HTML Parser` (AngleSharp).
- Testes de API – Verificação de endpoints e contratos.
- Testes com banco de dados – Testes que interagem com persistência de dados.

### 🧭 Testes End-to-End (E2E)

- Web Drivers para navegadores.
- Configuração do Selenium.
- `POM` – Page Object Model para modularização de páginas.
- `BDD` – Testes com comportamento orientado ao negócio.

### 🔥 Testes de Carga

- **Tipos de Teste:**
  - Desempenho
  - Carga
  - Stress
- **Ferramentas:**
  - Apache JMeter
- **APDEX** – Índice de Satisfação de Usuário.
- Boas práticas sobre como estruturar e executar testes de carga.


## 🧰 Bibliotecas Utilizadas

| Categoria | Biblioteca |
|----------|-------------|
| Teste | `xunit` |
| Asserts | `FluentAssertions` |
| Mocking | `Moq`, `Moq.AutoMock` |
| Dados Fake | `Bogus` |
| HTML Parsing | `AngleSharp` |
| E2E | `Selenium.WebDriver` |


## 🧠 Conceitos Importantes

Aqui estão explicações essenciais para compreender melhor os fundamentos por trás das práticas de teste utilizadas no projeto.

### 🛠️ TDD

**TDD** (Test Driven Development) é uma prática de desenvolvimento onde os testes são escritos **antes** do código de produção. O ciclo básico é:

1. Escrever um teste que falha.
2. Escrever o mínimo de código necessário para passar o teste.
3. Refatorar o código garantindo que o teste continue passando.

Essa abordagem garante um design de software mais limpo, melhora a confiabilidade do sistema e reduz 
o retrabalho.

<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/TDD.png" width="400" height="300"/>
</div>

### 🎯 BDD

**BDD** (Behavior Driven Development) é uma evolução do TDD que enfatiza a descrição do comportamento do sistema do ponto de vista do usuário. Em BDD, escrevemos especificações legíveis tanto por técnicos quanto por não técnicos (como Product Owners).

Para os testes end-to-end deste projeto, adotei o **BDD** para estruturar os cenários, focando em capturar o comportamento esperado de cada funcionalidade, simulando a experiência real de uso.

<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/BDD.png" width="700" height="350"/>
</div>

#### 📋 Exemplo de Cenário BDD

```gherkin
Funcionalidade: Tela de login
  Como um usuário registrado
  Quero acessar minha conta
  Para visualizar minhas informações pessoais

  Cenário: Login bem-sucedido com credenciais válidas
    Dado que estou na página de login
    Quando eu preencho o usuário e senha corretamente
    E clico no botão "Entrar"
    Então devo ser redirecionado para a página inicial
    E devo ver a mensagem "Bem-vindo de volta!"
```

### 📄 POM (Page Object Model)
O **POM (Page Object Model)** é um padrão de design utilizado para organizar o código de testes de interface gráfica, especialmente quando usamos ferramentas como Selenium.

<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/POM%20-%20Page%20Object%20Model.png"/>
</div>

**No POM:**

 - Cada página ou tela da aplicação é representada por uma classe.
 - Cada elemento da página é mapeado como um atributo da classe.
 - As ações realizadas na página (ex: clicar em botão, preencher formulário) são métodos da classe.

**Benefícios:**

- Reduz duplicação de código.
- Facilita a manutenção (uma mudança na interface afeta apenas a classe correspondente).
- Deixa os testes mais legíveis e organizados.


🔹 **Exemplo de Classe POM (Selenium):**

```
public class LoginPage
{
    private readonly IWebDriver _driver;

    public LoginPage(IWebDriver driver)
    {
        _driver = driver;
    }

    private IWebElement UsernameInput => _driver.FindElement(By.Id("username"));
    private IWebElement PasswordInput => _driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => _driver.FindElement(By.Id("login-button"));

    public void EnterUsername(string username) => UsernameInput.SendKeys(username);
    public void EnterPassword(string password) => PasswordInput.SendKeys(password);
    public void ClickLogin() => LoginButton.Click();
}
```

🔹 **Uso no teste:**
```
var loginPage = new LoginPage(driver);
loginPage.EnterUsername("usuario@teste.com");
loginPage.EnterPassword("senha123");
loginPage.ClickLogin();
```

### 🏋️ Testes de Carga

Os testes de carga têm como objetivo avaliar como a aplicação se comporta sob diferentes níveis de demanda e pressão. Eles ajudam a garantir que a aplicação mantenha a performance esperada mesmo em situações adversas.

#### ⚡ Desempenho

**Sua aplicação possui 5000 usuários. Será que ela é capaz de suportar 1/3 desses usuários simultaneamente?**

O teste de desempenho (*performance*) é o tipo de teste que valida um cenário real. É necessário garantir que a aplicação responda bem a um número de conexões esperadas.

Neste tipo de teste, a preocupação é realizar uma **regressão de performance** para garantir que a aplicação não perdeu desempenho após uma mudança e que ela vai conseguir atender à demanda esperada com um tempo de resposta satisfatório.

**Exemplo:** Responder em menos de **500ms** no *response time*.

#### 📈 Carga

**Quantas transações serão suportadas se o número de usuários aumentar em 50%?**

O objetivo principal é encontrar o **limite de capacidade** da aplicação e identificar qual o gargalo (banco de dados, hardware, cache). Não é o objetivo do teste de carga encontrar problemas funcionais.

Esse tipo de teste é essencial para aplicações que passam por **picos sazonais** como promoções, Natal ou Black Friday.

**Também é utilizado para:**
- Validar se melhorias de performance produziram resultados (comparando testes antes e depois da implementação).
- Confirmar se a **escala automática** (horizontal scaling) está funcionando corretamente dentro do tempo esperado.

#### 💥 Stress

**Como a aplicação irá se comportar se um número não previsto de requisições for disparado simultaneamente?**

O teste de stress é baseado no disparo de **massivas quantidades de requests** simultâneos durante alguns minutos. O objetivo é entender o comportamento da aplicação nesse tipo de cenário extremo.

**Os pontos analisados são:**
- Que tipo de **crash** pode ocorrer.
- Qual o **tempo de recuperação** da aplicação após a sobrecarga.
- Se a aplicação retorna ao **estado normal sem intervenção manual**.

Muitas vezes o teste é disparado contra uma funcionalidade específica para observar como o restante da aplicação é afetado.

#### 📋 Resumo dos Tipos de Teste de Carga

| Tipo de Teste | Propósito | Exemplo |
| :--- | :--- | :--- |
| ⚡ **Desempenho** | Validar se a aplicação responde bem ao número esperado de usuários e garantir que não houve regressão de performance. | Confirmar que o tempo de resposta é inferior a 500ms. |
| 📈 **Carga** | Identificar o limite de capacidade da aplicação e possíveis gargalos ao aumentar o número de usuários. | Avaliar o comportamento durante picos como Black Friday. |
| 💥 **Stress** | Analisar como a aplicação se comporta sob uma quantidade extrema e inesperada de requisições. | Observar se a aplicação recupera sem intervenção após crash. |


### 📈 APDEX

**APDEX (Application Performance Index)** é uma métrica padrão usada para medir a satisfação dos usuários em relação à performance de uma aplicação. O APDEX é representado como um valor entre 0 e 1, onde 1 significa que todos os usuários estão satisfeitos.

**Formula:**
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/Formula%20APDEX.png" width=700 height=300/>
</div>

**Critérios de Avaliação:**

Essa métrica é essencial para avaliar a qualidade percebida de sistemas em testes de carga.
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/APDEX%20Score.png" width=500 height=250/>
</div>

**Resultados:**
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/APDEX.png" />
</div>

<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/response%20time%20overview.png" />
</div>

## 🔧 Instalação
1. **Pré-requisitos**: 
   - Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:
    [Git](https://git-scm.com), [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0), [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads). 
    Além disto é bom ter um editor para trabalhar com o código como [VSCode](https://code.visualstudio.com/).

2. **Rodando Banco de Dados pelo Docker**:

    ```bash
      # Iniciando Docker
      $ sudo systemctl start docker

      # Rodando SQL Server
      $ sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=sql@2019' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
    ```

3. **Instalando as Dependências**:
    ```bash
    $ dotnet restore
    ```

### 🚀 Rodando o projeto
  ```bash
    # Entrando na pasta de execução do projeto
    $ cd 03\ -\ Testes\ de\ Integracao/src/NerdStore.WebApp.MVC/

    # Executando Projeto
    $ dotnet run
  ```

### ✅ Executando os testes
```bash
 $  dotnet test
```

**Categorias de teste:**
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/testes.png" />
</div>

## 🎨 Testes End-to-End (E2E)

### 🖥️ Adicionar item com sucesso a um novo pedido
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/Adicionar%20item%20com%20sucesso%20a%20um%20novo%20pedido.gif" />
</div>

### 🖥️ Adicionar item já existente no carrinho
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/Adicionar%20item%20j%C3%A1%20existente%20no%20carrinho.gif" />
</div>

### 🖥️ Adicionar itens acima do limite
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/Adicionar%20itens%20acima%20do%20limite.gif" />
</div>

### 🖥️ Cadastro de usuário com sucesso
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/Cadastro%20de%20usu%C3%A1rio%20com%20sucesso.gif" />
</div>

### 🖥️ Realizar login com sucesso
<div align="center">
  <img src="https://raw.githubusercontent.com/Willian-Brito/dominando-testes-de-software/refs/heads/main/03%20-%20Testes%20de%20Integracao/src/NerdStore.WebApp.MVC/wwwroot/prints/Realizar%20login%20com%20sucesso.gif" />
</div>

## 📝 Licença

Este projeto esta sobe a licença [MIT](https://github.com/Willian-Brito/dominando-testes-de-software/blob/main/LICENSE).

Feito com ❤️ por Willian Brito 👋🏽 [Entre em contato!](https://www.linkedin.com/in/willian-ferreira-brito/)