# 🚀 Roteiro de Testes de Unidade para um CRUD

## 📜 1. Configuração Inicial  
Antes de iniciar os testes, configurar:  
- Um banco de dados em memória ou um mock do repositório.  
- Mocks para dependências, como serviços auxiliares e validações.  
- Um framework de testes, como xUnit, NUnit ou MSTest (para C#).  


## 📄 2. Padrão de Nomenclatura dos Testes  

Os métodos de teste seguem o padrão **"Método_UmComportamento_Esperado"**, estruturado assim:  

### **📌 Estrutura do Nome do Método**  
**`[Action]_[ExpectedBehavior]_[Condition]`**  

#### ➡️ **1. [Action] → O que está sendo testado?**  
O primeiro termo descreve a ação ou método do CRUD que está sendo testado. Exemplos:  
- `Create` → Criar um registro.  
- `GetById` → Buscar um registro pelo ID.  
- `GetAll` → Buscar todos os registros.  
- `Update` → Atualizar um registro.  
- `Delete` → Remover um registro.  

#### ➡️ **2. [ExpectedBehavior] → Qual o resultado esperado?**  
O segundo termo indica o comportamento esperado ao executar a ação. Exemplos:  
- `ShouldAddRecord` → Deve adicionar um registro.  
- `ShouldReturnRecord` → Deve retornar um registro.  
- `ShouldModifyRecord` → Deve modificar um registro.  
- `ShouldRemoveRecord` → Deve remover um registro.  
- `ShouldThrowException` → Deve lançar uma exceção.  

#### ➡️ **3. [Condition] → Em que situação ocorre?**  
O terceiro termo especifica a condição ou contexto em que o teste ocorre. Exemplos:  
- `WhenValidDataIsProvided` → Quando dados válidos são fornecidos.  
- `WhenRecordExists` → Quando o registro existe.  
- `WhenRecordDoesNotExist` → Quando o registro não existe.  
- `WhenIdIsInvalid` → Quando o ID fornecido é inválido.  
- `WhenRecordHasDependencies` → Quando o registro tem dependências.  

### **📌 Exemplo de Nomeação**  
**`Create_ShouldThrowValidationException_WhenRequiredFieldsAreMissing`**  
- `Create` → O método testado é o de criação.  
- `ShouldThrowValidationException` → O comportamento esperado é o lançamento de uma exceção de validação.  
- `WhenRequiredFieldsAreMissing` → Ocorre quando campos obrigatórios não são fornecidos.  

### **✅ Vantagens desse Padrão**  
- **Legibilidade** → O nome do teste explica exatamente o que ele faz.  
- **Padronização** → Facilita a organização e manutenção dos testes.  
- **Facilidade na depuração** → Quando um teste falha, o nome do método já indica o possível problema.  
- **Boa prática** → Esse padrão é amplamente adotado, facilitando a colaboração entre desenvolvedores.  


## *️⃣ 3. Testes de Campos Obrigatórios  

### ❌ Cenários de Falha
- `Create_ShouldThrowRequiredFieldException_WhenRequiredFieldsAreMissing`  
  - **Exception:** `RequiredFieldException`  
  - Deve lançar uma exceção se um campo obrigatório não for preenchido.  
- `Update_ShouldThrowRequiredFieldException_WhenRequiredFieldsAreMissing`  
  - **Exception:** `RequiredFieldException`  
  - Deve lançar uma exceção se um campo obrigatório não for preenchido ao atualizar um registro.  
- `Create_ShouldThrowFieldExceedsMaxLengthException_WhenFieldExceedsMaxLength`  
  - **Exception:** `FieldExceedsMaxLengthException`  
  - Deve lançar uma exceção se um campo ultrapassar o tamanho máximo permitido.  
- `Create_ShouldThrowInvalidFieldException_WhenFieldHasInvalidFormat`  
  - **Exception:** `InvalidFieldException`  
  - Deve lançar uma exceção se um campo estiver em um formato inválido.


## 💾 4. Testes para Create (Adicionar Registro)  

### ✅ Cenários de Sucesso 
- `Create_ShouldAddRecord_WhenValidDataIsProvided`  
  - Deve criar um registro válido com sucesso.  
- `Create_ShouldSetRequiredFieldsCorrectly`  
  - Deve preencher corretamente os campos obrigatórios.  

### ❌ Cenários de Falha  
- `Create_ShouldThrowValidationException_WhenRequiredFieldsAreMissing`  
  - Deve lançar uma exceção ao tentar criar um registro com dados inválidos.  
  - Exemplo: **ValidationException** quando um campo obrigatório está ausente.
- `Create_ShouldThrowDuplicateRecordException_WhenRecordAlreadyExists`  
  - Deve lançar uma exceção ao tentar criar um registro duplicado.  
  - Exemplo: **DuplicateRecordException** ao tentar inserir um item com uma chave única já existente.
- `Create_ShouldThrowBusinessRuleException_WhenDataIsInconsistent`  
  - Deve lançar uma exceção ao tentar criar um registro com valores inconsistentes.  
  - Exemplo: **BusinessRuleException** ao cadastrar um produto com estoque negativo


## 📖 5. Testes para Read (Consultar Registro)  

### ✅ Cenários de Sucesso 
- `GetById_ShouldReturnRecord_WhenRecordExists`  
  - Deve retornar um registro existente pelo ID.  
- `GetAll_ShouldReturnList_WhenRecordsExist`  
  - Deve retornar uma lista de registros quando houver dados na base.  
- `GetAll_ShouldReturnEmptyList_WhenNoRecordsExist`  
  - Deve retornar uma lista vazia quando não houver registros na base.  

### ❌ Cenários de Falha  
- `GetById_ShouldThrowNotFoundException_WhenRecordDoesNotExist`  
  - Deve lançar uma exceção ao buscar um registro inexistente pelo ID.  
  - Exemplo: **NotFoundException** quando o ID não existe na base.
- `GetById_ShouldThrowArgumentException_WhenIdIsInvalid`  
  - Deve lançar uma exceção ao passar um ID inválido.  
  - Exemplo: **ArgumentException** quando o ID é nulo ou menor que zero.


## 📝 6. Testes para Update (Atualizar Registro)  

### ✅ Cenários de Sucesso 
- `Update_ShouldModifyRecord_WhenValidDataIsProvided`  
  - Deve atualizar um registro existente corretamente.  
- `Update_ShouldNotModifyUnchangedFields_WhenUpdatingRecord`  
  - Deve manter inalterados os campos que não forem atualizados.  

### ❌ Cenários de Falha  
- `Update_ShouldThrowNotFoundException_WhenRecordDoesNotExist`  
  - Deve lançar uma exceção ao tentar atualizar um registro inexistente.  
  - Exemplo: **NotFoundException** quando o ID não existe na base.
- `Update_ShouldThrowValidationException_WhenRequiredFieldsAreMissing`  
  - Deve lançar uma exceção ao tentar atualizar um registro com dados inválidos.  
  - Exemplo: **ValidationException** quando um campo obrigatório está ausente ou em formato incorreto.
- `Update_ShouldThrowArgumentException_WhenIdIsInvalid`  
  - Deve lançar uma exceção ao tentar atualizar um registro com um ID inválido.  
  - Exemplo: **ArgumentException** ao passar um ID nulo ou menor que zero.
- `Update_ShouldThrowBusinessRuleException_WhenDataIsInconsistent`  
  - Deve lançar uma exceção ao tentar atualizar um registro para um estado inconsistente.  
  - Exemplo: **BusinessRuleException** ao tentar marcar um pedido como "entregue" sem ter sido "enviado".


## 🗑 7. Testes para Delete (Remover Registro)  

### ✅ Cenários de Sucesso 
- `Delete_ShouldRemoveRecord_WhenRecordExists`  
  - Deve remover um registro existente corretamente.  
- `Delete_ShouldPerformSoftDelete_WhenFeatureIsEnabled`  
  - Deve permitir a remoção lógica (soft delete) se aplicável.  

### ❌ Cenários de Falha  
- `Delete_ShouldThrowNotFoundException_WhenRecordDoesNotExist`  
  - Deve lançar uma exceção ao tentar remover um registro inexistente.  
  - Exemplo: **NotFoundException** quando o ID não existe na base.
- `Delete_ShouldThrowDependencyException_WhenRecordHasDependencies`  
  - Deve lançar uma exceção ao tentar remover um registro associado a outras entidades. 
  - Exemplo: **DependencyException** quando o registro tem dependências (ex: uma categoria com produtos atrelados). 
- `Delete_ShouldThrowArgumentException_WhenIdIsInvalid`  
  - Deve lançar uma exceção ao tentar remover um registro com um ID inválido.  
  - Exemplo: **ArgumentException** ao passar um ID nulo ou menor que zero.
