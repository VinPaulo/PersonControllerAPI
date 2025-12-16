# Sistema de Controle de Pessoas

Este projeto é uma solução completa (Full Stack) para o gerenciamento de cadastro de pessoas, composta por uma **API em .NET 10** robusta e um **Frontend em Blazor WebAssembly** moderno. 

O sistema oferece operações completas de **CRUD** (Create, Read, Update, Delete), autenticação via **JWT**, e foi desenhado para ser facilmente escalável e containerizado com **Docker**.

---

### 🚀 **Funcionalidades**

#### **API (Backend)**
- **Cadastro de Pessoas** (`POST /pessoas`): Cria novos registros com validação.
- **Consulta** (`GET /pessoas` e `/pessoas/{id}`): Listagem geral e detalhada.
- **Atualização** (`PUT /pessoas/{id}`): Modificação de dados existentes.
- **Remoção** (`DELETE /pessoas/{id}`): Exclusão física de registros.
- **Segurança**: Autenticação e Autorização via tokens **JWT (JSON Web Token)**.

#### **Cliente (Frontend)**
- **Interface Interativa**: Desenvolvida em **Blazor WebAssembly**, rodando diretamente no navegador do cliente.
- **Consumo de API**: Integração fluida com o backend para exibir e manipular dados.
- **Design Responsivo**: Layout construído com componentes modernos.

---

### ⚙️ **Tecnologias Utilizadas**

#### **Backend**
- **Framework**: [.NET 10 (Preview)](https://dotnet.microsoft.com/)
- **Tipo de Projeto**: ASP.NET Core Web API (Minimal API)
- **Banco de Dados**: 
    - **PostgreSQL** (Produção/Docker)
    - **InMemory** (Desenvolvimento/Testes rápidos)
- **ORM**: Entity Framework Core
- **Documentação**: Swagger / OpenAPI

#### **Frontend**
- **Framework**: [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- **Linguagem**: C# (Rodando no Browser)

#### **Infraestrutura**
- **Containerização**: Docker e Docker Compose
- **Gerenciamento de DB**: DBeaver (Recomendado para acesso externo ao Postgres)

---

### 🔧 **Como Executar o Projeto**

Você pode rodar o projeto de duas formas principais: usando **Docker** (recomendado para ver tudo funcionando junto) ou manualmente (para desenvolvimento).

#### **Opção 1: Usando Docker (Recomendado)**
Esta opção sobe o banco de dados PostgreSQL e a API automaticamente.

1.  Certifique-se de ter o [Docker](https://www.docker.com/) instalado.
2.  Na raiz do projeto, execute:
    ```bash
    docker-compose up -d
    ```
    Isso iniciará o banco de dados e a API.
3.  A API estará acessível em: `http://localhost:5000` (ou porta configurada no docker-compose).

#### **Opção 2: Execução Manual (Desenvolvimento)**

**Pré-requisitos**: .NET SDK 10.0 instalado.

1.  **Backend (API)**
    - Navegue até a pasta raiz.
    - O projeto está configurado para usar um banco **InMemory** por padrão para facilitar o desenvolvimento local sem dependências externas.
    - Execute:
      ```bash
      dotnet run
      ```
    - A API (e o Swagger) estarão disponíveis em `http://localhost:5248` (verifique o output do console).

2.  **Frontend (Blazor)**
    - Em um novo terminal, navegue até a pasta `Client`:
      ```bash
      cd Client
      ```
    - Execute:
      ```bash
      dotnet run
      ```
    - O navegador abrirá a aplicação (geralmente em `http://localhost:5200` ou similar).

---

### 🔐 **Autenticação**

Alguns endpoints podem requerer autenticação. O projeto utiliza **Bearer Token (JWT)**.
- Ao fazer login (se implementado) ou configurar o ambiente, você receberá um token.
- No **Swagger**, clique no botão "Authorize" e insira o token no formato: `Bearer SEU_TOKEN_AQUI`.

---

### 📂 **Estrutura do Projeto**

- `/` (Raiz): Contém a API (.NET Web API), Dockerfile e configurações.
- `/Client`: Contém o projeto Frontend (Blazor WebAssembly).
- `/Controllers` e `/Routes`: Definição dos endpoints da API.
- `/Data`: Contexto do banco de dados e migrações.
- `/Models`: Modelos de dados compartilhados.

---

### 🤝 **Contribuindo**

Sinta-se à vontade para abrir _issues_ ou enviar _pull requests_ com melhorias.
