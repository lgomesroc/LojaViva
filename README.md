# Projeto LojaViva

Este é o repositório do projeto LojaViva, uma aplicação de loja virtual desenvolvida com .NET 8 e MySQL.

## Estrutura do Projeto
```
.
├── LojaViva.sln
├── backend
│   ├── Dockerfile
│   └── LojaViva.API
│       ├── DbContext
│       │   └── ApplicationDbContext.cs
│       ├── LojaViva.API.csproj
│       ├── LojaViva.API.http
│       ├── Migrations
│       │   ├── 20250407141846_InitialCreate.Designer.cs
│       │   ├── 20250407141846_InitialCreate.cs
│       │   └── ApplicationDbContextModelSnapshot.cs
│       ├── Program.cs
│       ├── Properties
│       │   └── launchSettings.json
│       ├── appsettings.Development.json
│       └── appsettings.json
├── docker-compose.yml
└── frontend
```

## Tecnologias escolhidas
### Backend
1. Tecnologias utilizadas
- .NET 8
- MySQL

2. Pacotes instalados
- **Microsoft.AspNetCore.Authentication.JwtBearer**: gerencia autenticação baseada em tokens JWT.
- **Microsoft.AspNetCore.Authorization**: fornece controle de acesso baseado em políticas.
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore**: implementação do Identity com suporte a Entity Framework Core.
- **Microsoft.AspNetCore.OpenApi**: gera documentação OpenAPI/Swagger para APIs ASP.NET Core.
- **Microsoft.EntityFrameworkCore.Design**: suporte para ferramentas de design (ex.: scaffolding) do EF Core.
- **Microsoft.EntityFrameworkCore.Relational**: adiciona funcionalidades relacionais ao EF Core (ex.: SQL).
- **Microsoft.EntityFrameworkCore.Tools**: ferramentas CLI para gerenciar EF Core (migrations, scaffolding).
- **Microsoft.Extensions.Logging.Console**: exibe logs no console.
- **Pomelo.EntityFrameworkCore.MySql**: implementação do EF Core para bancos MySQL.
- **Swashbuckle.AspNetCore**: integração do Swagger para ASP.NET Core.
- **System.IdentityModel.Tokens.Jwt**: manipula tokens JWT para autenticação.
- **Microsoft.Extensions.Caching.Memory**: gerencia cache em memória.
- **xunit**: framework para testes unitários em .NET.
- **xunit.runner.visualstudio**: executa testes xUnit no Visual Studio.
- **Moq**: Mocking de objetos para testes.
- **Moq.EntityFrameworkCore**: Mocking de contexto EF Core em testes.
- **Microsoft.EntityFrameworkCore.InMemory**: banco de dados em memória para testes do EF Core.
- **Microsoft.AspNetCore.Mvc.Testing**: facilita testes de integração em ASP.NET Core.
- **coverlet.collector**: gera relatórios de cobertura de código para testes.
- **Bogus**: gera dados aleatórios como usuários, pedidos ou produtos para testes e desenvolvimento.

### Frontend
Ainda não implementado.

## Configuração do Docker

### Dockerfile
O arquivo Dockerfile para o backend está localizado em `./backend/Dockerfile`.

### Docker Compose
O arquivo **docker-compose.yml** na raiz do projeto.

## Comandos Úteis

### Construir e Iniciar os Contêineres
```
docker-compose up --build
```

### Parar os Contêineres
```
docker-compose down
```

### Acessar o Terminal do Contêiner Backend
```
docker exec -it lojaviva-loja-viva-backend-1 /bash
```

### Executar Migrações de Banco de Dados
Dentro do contêiner backend:
```
cd /app/src
dotnet ef database update
```

## Problemas Conhecidos e Soluções
### Erro: "backend/LojaViva.API not found"
**Problema:** quando os caminhos no Dockerfile incluem o prefixo `./backend/` enquanto o contexto de build já está definido como `./backend` no docker-compose.yml.
**Solução:** remover o prefixo `./backend/` nos comandos COPY do Dockerfile.

### Erro: "dotnet-ef does not exist"
**Problema:** a ferramenta Entity Framework Core CLI não está disponível no contêiner em execução.
**Solução:** instalar o `dotnet-ef` na imagem final do Dockerfile e garantir que esteja no PATH.

### Erro: "can't cd to /src/LojaViva"
**Problema:** o diretório de código-fonte não existe na imagem final do contêiner.
**Solução:** adicionar um comando COPY no Dockerfile para copiar o código-fonte para a imagem final:
```
COPY --from=build /src/LojaViva ./src
```

## Histórico de Migrações
### 20250407141846_InitialCreate
Migração inicial que cria a tabela `Produtos` com as seguintes colunas:
- `Id` (int, auto-incremento, chave primária)
- `Nome` (longtext)
- `Preco` (decimal)
- `Estoque` (int)

Comando executado:
```
cd /app/src
dotnet ef database update
```

Resultado:
```
Build started...
Build succeeded.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (87ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='LojaVivaDB' AND TABLE_NAME='__EFMigrationsHistory';
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (178ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE `__EFMigrationsHistory` (
          `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
          `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
          CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
      ) CHARACTER SET=utf8mb4;
[...]
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (22ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
      VALUES ('20250407141846_InitialCreate', '8.0.13');
Done.
```

## Histórico do Projeto
### Backend
1. Configuração do Ambiente
- Estruturamos o projeto com:
  - Backend com .NET 8, utilizando Entity Framework Core e MySQL.
- Diretório organizado com:
  - Data para o contexto do banco.
  - Models com as classes Produto, Cliente e Pedido.
  - Controllers para os endpoints da API.

2. Docker
- Configurado o Dockerfile para o backend, garantindo:
  - Copiar o arquivo .csproj e todo o código para o contêiner.
  - Executar o dotnet restore e dotnet publish corretamente.
  - Instalar o CLI do dotnet-ef para gerenciar migrações dentro do contêiner.
  - Ajustar o prompt do contêiner para mostrar a localização no terminal.

3. Migrações
- Geramos e aplicamos migrações para criar tabelas no banco MySQL, incluindo:
  - Tabelas para Produtos, Clientes, e Pedidos.
  - Validamos que o banco foi corretamente atualizado com as tabelas.

4. Endpoints CRUD
- Implementamos o CRUD completo para a entidade Cliente:
  - GET, POST, PUT, e DELETE funcionando nos controladores.
  - Planejei replicar o padrão para as outras entidades.

5. Solução de Problemas
- Corrigi:
  - Problemas com caminhos duplicados no **Dockerfile**.
  - Namespace ausente no código do **DbContext**.
  - Configuração correta dos arquivos no contêiner.
  - Lidei com vários erros de compilação durante o build do contêiner.

6. Testes
- Garantimos que os endpoints estavam funcionando via **Swagger** e **Postman**.
  - Validei o banco de dados com consultas diretas ao MySQL.
- Teste e Sucesso no Endpoint de Clientes:
  - Validei o funcionamento correto do endpoint de Clientes, incluindo operações como criação, atualização e exclusão de registros no **Swagger**.
- Configuração de Autenticação e Segurança:
  - Implementei **ASP.NET Core Identity** para gerenciar usuários e senhas.
  - Adicionei suporte a autenticação com **JWT** para proteger os endpoints.
  - Configurado os serviços no `Program.cs` e ajustamos o appsettings.json para suportar as chaves JWT.
- Criação de Controladores (Controller):
  - Adicionado controladores específicos para as entidades Produto e Pedido. Implementado endpoints básicos para operações como listar, criar, atualizar e excluir registros.
- Correções e Melhorias:
  - Resolvido os erros de versão e conflitos entre pacotes do .NET 8 durante a configuração de dependências.
  - Adicionaso o método `EnableRetryOnFailure()` para lidar com falhas transitórias ao conectar ao **MySQL**.
  - Revisão e atualização do `Program.cs` para que ele inclua todas as funcionalidades necessárias, como autenticação, autorização, e documentação **Swagger**.
- Resolução de Problemas de Conexão:
  - Lidei com problemas relacionados à porta do servidor (5000) já estar em uso, configurando uma nova porta (5001).
  - Garanti que a string de conexão com o banco **MySQL** estivesse correta e o banco estivesse acessível.
- Ajustes no **Swagger**:
  - Certificado de que os controladores e endpoints aparecessem corretamente no **Swagger** para facilitar testes e documentação da API.

7. Configuração inicial
- Estrutura do **Dockerfile**: 
  - Ajustado o **Dockerfile** para garantir que os pacotes necessários fossem instalados e que o ambiente estivesse configurado corretamente para compilar e rodar os testes.
- Pacotes adicionados:
  - **Microsoft.AspNetCore.Mvc.Testing**
  - **Microsoft.EntityFrameworkCore.InMemory**
  - **Moq.EntityFrameworkCore**
  - Outros pacotes essenciais para o projeto de testes.

8. Implementação de testes
- Testes de unidade:
  - Criado testes para validar métodos isolados no `ProdutoController`.
  - Ajustado a estrutura do `ApplicationDbContext` para incluir um construtor sem parâmetros e tornamos propriedades como Produtos virtual para permitir mock com Moq.
- Testes de Integração:
  - Desenvolvido testes para validar endpoints no `AuthController`.
  - Configurado um banco de dados `InMemory` para os testes, garantindo que o ambiente fosse isolado.

9. Correção de Erros
Resolvido problemas relacionados à incompatibilidade de versões de pacotes (como **Microsoft.EntityFrameworkCore.InMemory** e **Moq.EntityFrameworkCore**).

Ajustado o método Register no AuthController para validar corretamente os dados enviados.

Adicionamos a configuração UseDeveloperExceptionPage para facilitar a depuração de erros durante o desenvolvimento.

10. Separação de arquivos
Extraímos a classe UserDto para um arquivo separado (UserDto.cs) na pasta Models, seguindo boas práticas de organização do projeto.

11. Testes Passaram
Finalizado a implementação dos testes, garantindo que todos os casos de teste no projeto fossem bem-sucedidos.

12. Repository Pattern
Introduzido o padrão de repositório para separar a lógica de acesso a dados e melhorar a organização.

Arquivos criados:

Repositories/IProdutoRepository.cs — Define a interface para operações de Produto.

Repositories/ProdutoRepository.cs — Implementa a interface e encapsula o acesso ao banco de dados.

Modificações no ProdutoController:

Refatoramos o ProdutoController para usar IProdutoRepository em vez de acessar diretamente o ApplicationDbContext.

13. Testes para Cliente e Pedido
- Desenvolveido testes unitários para os controladores ClientesController e PedidoController, garantindo a validação do comportamento esperado para os endpoints.
  - Cenários Testados:

    - ClientesController:
      - GetClientes: Retorna a lista de clientes com 200 OK.
      - GetCliente: Retorna um cliente específico ou 404 Not Found para IDs inexistentes.
      - PostCliente: Adiciona um novo cliente e retorna 201 Created.
      - DeleteCliente: Exclui um cliente existente com sucesso ou retorna 404 Not Found quando o cliente não existe.
    - PedidoController:
      - GetPedidos: Retorna a lista de pedidos com 200 OK.
      - GetPedido: Retorna um pedido específico ou 404 Not Found para IDs inexistentes.
      - AddPedido: Adiciona um novo pedido e retorna 201 Created.
      - DeletePedido: Exclui um pedido existente com sucesso ou retorna 404 Not Found quando o pedido não existe.

14. Implementação do Repository Pattern para Cliente e Pedido
- Cliente:
  - Criamos os arquivos:
    - **IClienteRepository.cs**: Interface que define as operações do repositório.
    - **ClienteRepository.cs**: Implementação do repositório que encapsula a lógica de manipulação de clientes no banco de dados.
  - Atualizamos o `ClientesController` para utilizar o `IClienteRepository` em vez de acessar diretamente o `ApplicationDbContext`.
- Pedido:
  - Criamos os arquivos:
    - `IPedidoRepository.cs`: Interface que define as operações do repositório.
    - `PedidoRepository.cs`: Implementação do repositório que encapsula a lógica de manipulação de pedidos, incluindo o relacionamento com a entidade Cliente.
  - Atualizamos o `PedidoController` para utilizar o `IPedidoRepository` em vez de acessar diretamente o `ApplicationDbContext`.

15. Refatoração do Program.cs
Dividimos o código do Program.cs em métodos de extensão para melhorar a organização e aderir ao princípio de responsabilidade única (SRP):

ServiceExtensions.cs:

Configuração de serviços, incluindo DbContext, Identity, e repositórios.

**AuthenticationExtensions.cs**:

Configuração da autenticação **JWT**.

Atualizamos o **Program.cs** para ficar mais limpo e modular, delegando responsabilidades aos métodos de extensão.

Seguindo boas práticas de arquitetura com:

Separação clara de responsabilidades (controllers, repositories, etc.)
Testes unitários abrangentes
Uso adequado de mocks para isolar as unidades testadas
Verificações específicas e precisas nos testes

16. Implementação de Logs
- Adicionado suporte ao logging no console utilizando o pacote Microsoft.Extensions.Logging.Console.
  - Configuramos o logging no arquivo Program.cs:
  - `builder.Logging.AddConsole();` foi adicionado para ativar o registro de logs no console.
  - Limpeza dos provedores padrão de logging com `builder.Logging.ClearProviders();`.
- Integrado o ILogger ao `ClientesController`:
  - Registra informações importantes, como chamadas de endpoints e manipulação de dados.
  - Mensagens de warning para eventos inesperados, como tentativas de acesso a clientes inexistentes.
- Exemplo de log no controlador:
```
_logger.LogInformation("GET /api/clientes chamado");
_logger.LogWarning($"Cliente com ID {id} não encontrado");
```

17. Implementação de Cache
- Adicionado suporte ao cache em memória utilizando o pacote Microsoft.Extensions.Caching.Memory.
- Configuramos o cache nos controladores:
  - `ClientesController`:
    - Adiciona ao cache a lista de clientes recuperada do repositório, com expiração configurada.
    - Invalida o cache nas operações de escrita (POST, PUT e DELETE).
  - `PedidoController`:
    - Implementa lógica semelhante para pedidos.
  - `ProdutoController`:
    - Cache aplicado para listagem de produtos.
  - `AuthController`:
    - Adiciona ao cache os usuários registrados para evitar duplicidade e melhora o desempenho.
- Configuração de `MemoryCacheEntryOptions`:
  - Expiração absoluta de 5 a 10 minutos dependendo do tipo de dado.
  - Expiração renovável para acessos recentes.

18. Geração de dados fictícios para testes
- Instalado o pacote **Bogus** no projeto de testes (**LojaViva.Tests**).
- Criado geradores de dados fictícios (fakers) para as entidades:
  - Cliente:
    - Gerador criado no arquivo **ClienteFaker.cs**, que simula dados como nome, e-mail, e telefone.
  - Pedido:
    - Gerador criado no arquivo **PedidoFaker.cs**, simulando dados como cliente associado, total e data.
  - Produto:
    - Gerador criado no arquivo **ProdutoFaker.cs**, que cria dados fictícios como nome, preço e estoque.
- Desenvolvido testes unitários para validar os fakers:
  - **ClienteTests.cs**: Teste para validar a geração de clientes fictícios.
  - **PedidoTests.cs**: Teste para validar a geração de pedidos fictícios.
  - **ProdutoTests.cs**: Teste para validar a geração de produtos fictícios.


### Frontend
Ainda não foi implementado





https://copilot.microsoft.com/chats/1Sz56pfAxtZZ4fe8YVay8