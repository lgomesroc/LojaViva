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
1. Tecnologias Utilizadas
- .NET 8
- MySQL

2. Pacotes instalados
- Microsoft.AspNetCore.Authentication.JwtBearer (v8.0.14)
  - Fornece suporte para autenticação usando JSON Web Tokens (JWT). Ele permite proteger endpoints da API com tokens de segurança gerados durante o login.
- Microsoft.AspNetCore.Authorization (v8.0.14)
  - Gerencia a autorização dos usuários, ou seja, o acesso a recursos baseado em políticas específicas, como permissões ou funções.
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (v8.0.14)
  - Integra o ASP.NET Core Identity ao Entity Framework Core, permitindo que usuários, senhas, papéis e outros dados de autenticação sejam armazenados no banco de dados.
- Microsoft.AspNetCore.OpenApi (v8.0.14)
  - Facilita a integração com Swagger/OpenAPI para documentar e explorar endpoints da sua API. Isso ajuda na visualização de como as rotas e dados são estruturados.
- Microsoft.EntityFrameworkCore.Design (v8.0.14)
  - Fornece ferramentas de design necessárias para migrações e scaffolding em Entity Framework Core, como criação de tabelas no banco de dados.
- Microsoft.EntityFrameworkCore.Relational (v8.0.14)
  - Contém funcionalidades adicionais para o EF Core que permitem trabalhar com bancos de dados relacionais, como o MySQL.
- Microsoft.EntityFrameworkCore.Tools (v8.0.14)
  - Fornece suporte para comandos CLI do EF Core, como dotnet ef migrations add e dotnet ef database update, permitindo controle fácil sobre o esquema do banco de dados.
- Pomelo.EntityFrameworkCore.MySql (v8.0.0)
  - Biblioteca específica para trabalhar com o MySQL no Entity Framework Core, fornecendo compatibilidade total com esse banco de dados.
- Swashbuckle.AspNetCore (v6.6.2)
  - Implementa o Swagger para ASP.NET Core, gerando automaticamente documentação interativa para os endpoints da API, onde você pode fazer testes diretamente.
- System.IdentityModel.Tokens.Jwt (v8.8.0)
  - Permite manipulação e geração de JWTs (JSON Web Tokens), facilitando a criação e validação de tokens de segurança.

### Frontend
Ainda não implementado.




## Configuração do Docker

### Dockerfile

O arquivo Dockerfile para o backend está localizado em `./backend/Dockerfile`.

### Docker Compose

O arquivo `docker-compose.yml` na raiz do projeto.


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
**Problema:** Quando os caminhos no Dockerfile incluem o prefixo `./backend/` enquanto o contexto de build já está definido como `./backend` no docker-compose.yml.
**Solução:** Remover o prefixo `./backend/` nos comandos COPY do Dockerfile.

### Erro: "dotnet-ef does not exist"
**Problema:** A ferramenta Entity Framework Core CLI não está disponível no contêiner em execução.
**Solução:** Instalar o `dotnet-ef` na imagem final do Dockerfile e garantir que esteja no PATH.

### Erro: "can't cd to /src/LojaViva"
**Problema:** O diretório de código-fonte não existe na imagem final do contêiner.
**Solução:** Adicionar um comando COPY no Dockerfile para copiar o código-fonte para a imagem final:
```dockerfile
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
Estruturamos o projeto com:

Backend com .NET 8, utilizando Entity Framework Core e MySQL.

Diretório organizado com:

DbContext para o contexto do banco.

Models com as classes Produto, Cliente e Pedido.

Controllers para os endpoints da API.

2. Docker
Configuramos o Dockerfile para o backend, garantindo:

Copiar o arquivo .csproj e todo o código para o contêiner.

Executar o dotnet restore e dotnet publish corretamente.

Instalar o CLI do dotnet-ef para gerenciar migrações dentro do contêiner.

Ajustar o prompt do contêiner para mostrar a localização no terminal.

3. Migrações
Geramos e aplicamos migrações para criar tabelas no banco MySQL, incluindo:

Tabelas para Produtos, Clientes, e Pedidos.

Validamos que o banco foi corretamente atualizado com as tabelas.

4. Endpoints CRUD
Implementamos o CRUD completo para a entidade Cliente:

GET, POST, PUT, e DELETE funcionando nos controladores.

Planejamos replicar o padrão para as outras entidades.

5. Solução de Problemas
Corrigimos:

Problemas com caminhos duplicados no Dockerfile.

Namespace ausente no código do DbContext.

Configuração correta dos arquivos no contêiner.

Lidamos com vários erros de compilação durante o build do contêiner.

6. Testes
Garantimos que os endpoints estavam funcionando via Swagger e Postman.

Validamos o banco de dados com consultas diretas ao MySQL.

Teste e Sucesso no Endpoint de Clientes:

Validamos o funcionamento correto do endpoint de Clientes, incluindo operações como criação, atualização e exclusão de registros no Swagger.

Configuração de Autenticação e Segurança:

Implementamos ASP.NET Core Identity para gerenciar usuários e senhas.

Adicionamos suporte a autenticação com JWT para proteger os endpoints.

Configuramos os serviços no Program.cs e ajustamos o appsettings.json para suportar as chaves JWT.

Criação de Controladores (Controller):

Adicionamos controladores específicos para as entidades Produto e Pedido. Implementamos endpoints básicos para operações como listar, criar, atualizar e excluir registros.

Correções e Melhorias:

Resolvemos erros de versão e conflitos entre pacotes do .NET 8 durante a configuração de dependências.

Adicionamos o método EnableRetryOnFailure() para lidar com falhas transitórias ao conectar ao MySQL.

Revimos e atualizamos o Program.cs para que ele inclua todas as funcionalidades necessárias, como autenticação, autorização, e documentação Swagger.

Resolução de Problemas de Conexão:

Lidamos com problemas relacionados à porta do servidor (5000) já estar em uso, configurando uma nova porta (5001).

Garantimos que a string de conexão com o banco MySQL estivesse correta e o banco estivesse acessível.

Ajustes no Swagger:

Certificamo-nos de que os controladores e endpoints aparecessem corretamente no Swagger para facilitar testes e documentação da API.

1. Configuração Inicial
Estrutura do Dockerfile: Ajustamos o Dockerfile para garantir que os pacotes necessários fossem instalados e que o ambiente estivesse configurado corretamente para compilar e rodar os testes.

Pacotes adicionados:

Microsoft.AspNetCore.Mvc.Testing

Microsoft.EntityFrameworkCore.InMemory

Moq.EntityFrameworkCore

Outros pacotes essenciais para o projeto de testes.

2. Implementação de Testes
Testes de Unidade:

Criamos testes para validar métodos isolados no ProdutoController.

Ajustamos a estrutura do ApplicationDbContext para incluir um construtor sem parâmetros e tornamos propriedades como Produtos virtual para permitir mock com Moq.

Testes de Integração:

Desenvolvemos testes para validar endpoints no AuthController.

Configuramos um banco de dados InMemory para os testes, garantindo que o ambiente fosse isolado.

3. Correção de Erros
Resolvemos problemas relacionados à incompatibilidade de versões de pacotes (como Microsoft.EntityFrameworkCore.InMemory e Moq.EntityFrameworkCore).

Ajustamos o método Register no AuthController para validar corretamente os dados enviados.

Adicionamos a configuração UseDeveloperExceptionPage para facilitar a depuração de erros durante o desenvolvimento.

4. Separação de Arquivos
Extraímos a classe UserDto para um arquivo separado (UserDto.cs) na pasta Models, seguindo boas práticas de organização do projeto.

5. Testes Passaram
Finalizamos a implementação dos testes, garantindo que todos os casos de teste no projeto fossem bem-sucedidos.

3. Repository Pattern
Introduzido o padrão de repositório para separar a lógica de acesso a dados e melhorar a organização.

Arquivos criados:

Repositories/IProdutoRepository.cs — Define a interface para operações de Produto.

Repositories/ProdutoRepository.cs — Implementa a interface e encapsula o acesso ao banco de dados.

Modificações no ProdutoController:

Refatoramos o ProdutoController para usar IProdutoRepository em vez de acessar diretamente o ApplicationDbContext.

1. Testes para Cliente e Pedido
Desenvolvemos testes unitários para os controladores ClientesController e PedidoController, garantindo a validação do comportamento esperado para os endpoints.

Cenários Testados:

ClientesController:

GetClientes: Retorna a lista de clientes com 200 OK.

GetCliente: Retorna um cliente específico ou 404 Not Found para IDs inexistentes.

PostCliente: Adiciona um novo cliente e retorna 201 Created.

DeleteCliente: Exclui um cliente existente com sucesso ou retorna 404 Not Found quando o cliente não existe.

PedidoController:

GetPedidos: Retorna a lista de pedidos com 200 OK.

GetPedido: Retorna um pedido específico ou 404 Not Found para IDs inexistentes.

AddPedido: Adiciona um novo pedido e retorna 201 Created.

DeletePedido: Exclui um pedido existente com sucesso ou retorna 404 Not Found quando o pedido não existe.

2. Implementação do Repository Pattern para Cliente e Pedido
Cliente:

Criamos os arquivos:

IClienteRepository.cs: Interface que define as operações do repositório.

ClienteRepository.cs: Implementação do repositório que encapsula a lógica de manipulação de clientes no banco de dados.

Atualizamos o ClientesController para utilizar o IClienteRepository em vez de acessar diretamente o ApplicationDbContext.

Pedido:

Criamos os arquivos:

IPedidoRepository.cs: Interface que define as operações do repositório.

PedidoRepository.cs: Implementação do repositório que encapsula a lógica de manipulação de pedidos, incluindo o relacionamento com a entidade Cliente.

Atualizamos o PedidoController para utilizar o IPedidoRepository em vez de acessar diretamente o ApplicationDbContext.

3. Refatoração do Program.cs
Dividimos o código do Program.cs em métodos de extensão para melhorar a organização e aderir ao princípio de responsabilidade única (SRP):

ServiceExtensions.cs:

Configuração de serviços, incluindo DbContext, Identity, e repositórios.

AuthenticationExtensions.cs:

Configuração da autenticação JWT.

Atualizamos o Program.cs para ficar mais limpo e modular, delegando responsabilidades aos métodos de extensão.

Seguindo boas práticas de arquitetura com:

Separação clara de responsabilidades (controllers, repositories, etc.)
Testes unitários abrangentes
Uso adequado de mocks para isolar as unidades testadas
Verificações específicas e precisas nos testes





### Frontend
Ainda não foi implementado





https://copilot.microsoft.com/chats/1Sz56pfAxtZZ4fe8YVay8