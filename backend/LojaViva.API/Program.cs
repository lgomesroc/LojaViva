using LojaViva.API.Data;
using LojaViva.API.Extensions;
using LojaViva.API.Repositories;
using System.Runtime.CompilerServices;

// Tornar o programa acessível ao projeto de testes
[assembly: InternalsVisibleTo("LojaViva.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Configurar logging no console
builder.Logging.ClearProviders(); // Limpa provedores padrão de logging
builder.Logging.AddConsole(); // Adiciona logging no console

// Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Adicionar repositórios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

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
        Description = "Documentação da API Loja Viva com suporte a autenticação JWT"
    });
});

// Adicionar suporte a controladores e Newtonsoft.Json para serialização
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

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
app.UseCors(); // Adiciona o middleware CORS
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Adicionar uma declaração pública da classe Program para torná-la acessível no teste
public partial class Program { }