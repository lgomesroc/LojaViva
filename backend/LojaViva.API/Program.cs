using LojaViva.API.Data;
using LojaViva.API.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Configuração detalhada de logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var appLogger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>(); // Definir logger corretamente

// Obter a string de conexão do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração do MySQL com política de retentativas e timeout
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions => 
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            mySqlOptions.CommandTimeout(60); // Timeout de 60 segundos
        });
    
    // Log detalhado das queries (útil para debug)
    options.LogTo(Console.WriteLine, LogLevel.Information);
    options.EnableSensitiveDataLogging();
});

// Configurar serviços personalizados e autenticação
builder.Services.ConfigureServices(builder.Configuration)
               .ConfigureAuthentication(builder.Configuration);

// Configurar controladores com Newtonsoft.Json
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Migração do banco de dados com tratamento robusto de erros
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var retries = 10; // Número máximo de tentativas
    
    appLogger.LogInformation("Iniciando migração do banco de dados...");

    while (retries > 0)
    {
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            appLogger.LogInformation("Migração do banco de dados concluída com sucesso!");
            break;
        }
        catch (MySqlConnector.MySqlException ex) when (ex.Number == 1042) // Erro de conexão
        {
            retries--;
            appLogger.LogWarning($"Falha na conexão com o MySQL. Tentativas restantes: {retries}. Erro: {ex.Message}");
            
            if (retries == 0)
            {
                appLogger.LogError("Número máximo de tentativas atingido. A aplicação será encerrada.");
                throw;
            }

            Thread.Sleep(5000); // Espera 5 segundos antes de tentar novamente
        }
        catch (Exception ex)
        {
            appLogger.LogError(ex, "Erro inesperado durante a migração do banco de dados");
            throw;
        }
    }
}

// Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

appLogger.LogInformation("Aplicação iniciada com sucesso!");
app.Run();

public partial class Program { } // Para testes de integração
