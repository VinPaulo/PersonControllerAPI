# API Controle de Pessoas

A **API Controle de Pessoas** é uma solução eficiente para gerenciar o cadastro de indivíduos, oferecendo suporte completo para as operações **CRUD** (Create, Read, Update, Delete). Utilizando o banco de dados **PostgreSQL**, a API segue as melhores práticas de desenvolvimento **RESTful**, garantindo simplicidade e escalabilidade.

---

### 🚀 **Funcionalidades Principais**

- **Cadastro de Pessoas** (`POST /pessoas`)  
  Adiciona uma nova pessoa ao sistema, permitindo o envio de dados como nome, idade, e-mail, entre outros atributos.

- **Consulta de Pessoas** (`GET /pessoas` e `GET /pessoas/{id}`)  
  Recupera a lista de todas as pessoas cadastradas ou os dados específicos de uma pessoa pelo seu ID.

- **Atualização de Dados** (`PUT /pessoas/{id}`)  
  Permite a atualização das informações de uma pessoa existente no banco de dados.

- **Remoção de Pessoas** (`DELETE /pessoas/{id}`)  
  Exclui um registro de pessoa de forma permanente do sistema.

---

### ⚙️ **Tecnologias Utilizadas**

- **Linguagem**: C#
- **Banco de Dados**: PostgreSQL
- **ORM**: [Entity Framework Core (EF Core)](https://docs.microsoft.com/en-us/ef/core/)
- **Metodologia**: Minimal API
- **Framework**: [ASP.NET / .NET Web API](https://dotnet.microsoft.com/en-us/apps/aspnet)
- **Ferramentas de Teste**: Swagger, Postman
- **Gerenciador de Banco de Dados**: DBeaver

---

### 🛠️ **Pacotes e Ferramentas**

A API foi construída utilizando os seguintes pacotes:

- **Microsoft.EntityFrameworkCore**: Facilitando a interação com o banco de dados PostgreSQL.
- **Microsoft.EntityFrameworkCore.Tools**: Suporte para migrações e comandos no Entity Framework.
- **Microsoft.EntityFrameworkCore.Design**: Necessário para gerar migrações e outras operações de design do banco de dados.
- **NpgSql**: Provedor de dados para PostgreSQL, essencial para a comunicação entre a aplicação e o banco de dados.

---

### 🧪 **Testes e Desenvolvimento**

Durante o desenvolvimento, a **API** foi testada utilizando:

- **Swagger**: Para uma interface interativa, que valida as funcionalidades de cada endpoint de forma visual.
- **Postman**: Para simulação de requisições HTTP e garantir o funcionamento correto da API.
- **DBeaver**: Para gerenciar o banco de dados PostgreSQL, visualizando e manipulando diretamente os dados armazenados.

---

### 💡 **Aplicações**

Essa API pode ser utilizada em diversos cenários, como:

- **Sistemas Administrativos**
- **Controle de Acesso**
- **Gestão de Cadastro de Clientes**

---

### 🔧 **Como Usar**

1. **Clone este repositório**:

    ```bash
    git clone https://github.com/VinPaulo/PersonControllerAPI.git
    ```

2. **Instale as dependências**:

    Execute o comando `dotnet restore` para instalar os pacotes necessários.

3. **Configure o banco de dados PostgreSQL** e execute as migrações:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

4. **Execute a API**:

    Execute o comando `dotnet run` para iniciar o servidor da API.

---
