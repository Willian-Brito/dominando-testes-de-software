Feature: Usuário - Cadastro
  Como um visitante da loja
  Eu desejo me cadastrar como usuário
  Para que eu possa realizar compras na loja
  
  Scenario: Cadastro de usuário com sucesso
    Given Que o visitante está acessando o site da loja
    When Ele clicar em registrar
    And Preencher os dados do formulario
      | Dados                |
      | E-mail               |
      | Senha                |
      | Confirmação de senha |
    And Clicar no botão registrar
    Then Ele será redirecionado para a vitrine
    And Uma saudação com seu e-mail será exibida no menu superior
    
  Scenario: Cadastro com senha sem maiusculas
    Given Que o visitante está acessando o site da loja
    When Ele clicar em registrar
    And Preencher os dados do formulario com uma senha sem maiusculas
      | Dados                |
      | E-mail               |
      | Senha                |
      | Confirmação de senha |
    And Clicar no botão registrar
    Then Ele receberá uma mensagem de erro que a senha precisa conter uma letra maiuscula
    
  Scenario: Cadastro com senha sem caractere especial
    Given Que o visitante está acessando o site da loja
    When Ele clicar em registrar
    And Preencher os dados do formulario com uma senha sem caractere especial
      | Dados                |
      | E-mail               |
      | Senha                |
      | Confirmação de senha |
    And Clicar no botão registrar
    Then Ele receberá uma mensagem de erro que a senha precisa conter um caractere especial