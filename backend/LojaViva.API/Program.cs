using LojaViva.API.Data;
using LojaViva.API.Extensions;
using System.Runtime.CompilerServices;

// Tornar o programa acessível ao projeto de testes
[assembly: InternalsVisibleTo("LojaViva.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Configurar logging no console
builder.Logging.ClearProviders(); // Limpa provedores padrão de logging
builder.Logging.AddConsole(); // Adiciona logging no console

// Configurar serviços e autenticação
builder.Services.ConfigureServices(builder.Configuration)
                .ConfigureJwtAuthentication(builder.Configuration);

// Configuração de Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Loja Viva API",
        Version = "v1",
        Description = "Documentação da API Loja Viva com suporte a autenticação JWT e ASP.NET Core Identity"
    });
});

// Adicionar suporte a controladores
builder.Services.AddControllers();

var app = builder.Build();

// Ambiente de desenvolvimento: configurar Swagger e página de erros
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuração de Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Adicionar uma declaração pública da classe Program para torná-la acessível no teste
public partial class Program { }
