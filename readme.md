
## Sobre o Projeto

Este projeto é uma API RESTful desenvolvida em .NET 6 como um desafio de codificação. O objetivo é fornecer uma base para um sistema de e-commerce, com funcionalidades para gerenciamento de clientes, produtos e pedidos. O projeto foi desenvolvido com foco em boas práticas de desenvolvimento de software, como arquitetura em camadas, injeção de dependência e testes unitários.


### Pré-requisitos

-   [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
-   [Docker](https://www.docker.com/products/docker-desktop) (para executar o SonarQube)
-   Um editor de código de sua preferência (e.g., [Visual Studio Code](https://code.visualstudio.com/), [Visual Studio](https://visualstudio.microsoft.com/))

### Instalação e Configuração

1.  **Clone o repositório:**

    ```bash
    git clone <url-do-repositorio>
    ```

2.  **Navegue até o diretório do projeto:**

    ```bash
    cd prova-backend
    ```

3.  **Restaure as dependências do .NET:**

    ```bash
    dotnet restore
    ```

4.  **Aplique as migrações do Entity Framework:**

    ```bash
    cd src
    dotnet ef database update
    ```

## Como Executar

### Executando a Aplicação

1.  **Navegue até o diretório `src`:**

    ```bash
    cd src
    ```

2.  **Execute a aplicação:**

    ```bash
    dotnet run
    ```

A API estará disponível em `http://localhost:5000` e `https://localhost:5001`.

### Documentação da API (Swagger)

A documentação interativa da API está disponível através do Swagger UI. Com a aplicação em execução, acesse:

`https://localhost:5001/swagger`

## Banco de Dados

-   **Provedor:** Microsoft SQL Server
-   **String de Conexão:** A string de conexão padrão está configurada no `appsettings.json` para usar o LocalDB do SQL Server.

    ```json
    "ConnectionStrings": {
      "ctx": "Server=(localdb)\mssqllocaldb;Database=Teste;Trusted_Connection=True;"
    }
    ```

-   **Migrations:** O projeto utiliza o Entity Framework Core para gerenciar o schema do banco de dados.
-   **Data Seeding:** O banco de dados é populado com dados de teste na primeira vez que a aplicação é executada.

## Testes

Para executar os testes unitários, navegue até o diretório de testes e execute o comando `dotnet test`:

```bash
cd ../tests/ProvaPub.Test
dotnet test
```

## Qualidade de Código

O projeto está integrado com o SonarQube para análise estática de código.

1.  **Inicie o SonarQube:**

    ```bash
    docker-compose up -d
    ```

2.  **Execute a análise:**

    ```bash
    ./run-analysis.ps1
    ```

    Os resultados estarão disponíveis em `http://localhost:9000`.

## Endpoints da API

| Método | Rota                                                              | Descrição                                     |
| ------ | ----------------------------------------------------------------- | ----------------------------------------------- |
| `GET`  | `/Customer/customers?page={page}`                                 | Retorna uma lista paginada de clientes.         |
| `GET`  | `/Customer/CanPurchase?customerId={customerId}&purchaseValue={purchaseValue}` | Verifica se um cliente pode realizar uma compra. |
| `GET`  | `/Product/products?page={page}`                                   | Retorna uma lista paginada de produtos.         |
| `POST` | `/Order/orders`                                                   | Cria um novo pedido.                            |
| `GET`  | `/Random/random`                                                  | Retorna um número aleatório.                    |

## O Desafio de Codificação

Este projeto também serve como um desafio de codificação. Os principais pontos a serem abordados são:

1.  **Correção da Paginação:** A paginação não está funcionando corretamente. É necessário corrigir a lógica para que a página correta de resultados seja retornada.
2.  **Injeção de Dependência:** O código utiliza a instanciação manual de serviços (e.g., `new ProductService()`). É necessário refatorar o código para utilizar o sistema de injeção de dependência do .NET.
3.  **Redução de Duplicação de Código:** Os modelos `CustomerList` e `ProductList`, assim como os serviços `CustomerService` e `ProductService`, possuem estruturas repetitivas. É necessário refatorar o código para criar uma estrutura mais genérica e reutilizável.
4.  **Testes Unitários:** O método `CanPurchase` no `CustomerController` precisa de testes unitários. É necessário criar testes que cubram o máximo de cenários possível.
