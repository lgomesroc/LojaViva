# Etapa 1: Build do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj do projeto API para o contêiner
COPY LojaViva.API/LojaViva.API.csproj ./LojaViva/

# Copiar o arquivo .csproj do projeto de testes para o contêiner
COPY LojaViva.Tests/LojaViva.Tests.csproj ./LojaViva.Tests/

# Restaurar dependências do projeto API
WORKDIR /src/LojaViva
RUN dotnet restore

# Restaurar dependências do projeto de testes
WORKDIR /src/LojaViva.Tests
RUN dotnet restore

# Instalar os pacotes necessários ao projeto LojaViva.API
WORKDIR /src/LojaViva
RUN dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.14
RUN dotnet add package Microsoft.AspNetCore.Authorization --version 8.0.14
RUN dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.14
RUN dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.14
RUN dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.14
RUN dotnet add package Microsoft.EntityFrameworkCore.Relational --version 8.0.14
RUN dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.14
RUN dotnet add package Microsoft.Extensions.Logging.Console --version 8.0.1
RUN dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.0
RUN dotnet add package Swashbuckle.AspNetCore --version 6.6.2
RUN dotnet add package System.IdentityModel.Tokens.Jwt --version 8.8.0
RUN dotnet add package Microsoft.Extensions.Caching.Memory --version 8.0.1
RUN dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 8.0.15

# Copiar todo o código do projeto API para o contêiner
COPY LojaViva.API /src/LojaViva/

# Copiar todo o código do projeto de testes para o contêiner
COPY LojaViva.Tests /src/LojaViva.Tests/

# Adicionar os pacotes necessários ao projeto de testes LojaViva.Tests
WORKDIR /src/LojaViva.Tests
RUN dotnet add package xunit
RUN dotnet add package xunit.runner.visualstudio
RUN dotnet add package Moq
RUN dotnet add package Moq.EntityFrameworkCore --version 8.0.1.7
RUN dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.15
RUN dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.15
RUN dotnet add package coverlet.collector
RUN dotnet add package Bogus

# Adicionar referência ao projeto principal no projeto de testes
RUN dotnet add LojaViva.Tests.csproj reference /src/LojaViva/LojaViva.API.csproj

# Publicar o projeto API
WORKDIR /src/LojaViva
RUN dotnet publish -c Release -o /out

# Etapa 2: Configurar a imagem final
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Copiar os arquivos publicados da API para a imagem final
COPY --from=build /out .
COPY --from=build /src/LojaViva ./src

# Instalar ferramentas CLI do EF na imagem final
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Configurar o prompt para mostrar o diretório atual
RUN echo 'export PS1="\u@\h:\w\$ "' >> /root/.bashrc

# Configurar o entrypoint para iniciar a aplicação
ENTRYPOINT ["dotnet", "LojaViva.API.dll"]
