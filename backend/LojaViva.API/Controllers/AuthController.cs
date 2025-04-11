using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using LojaViva.API.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly IMemoryCache _cache;

    public AuthController(UserManager<IdentityUser> userManager, ILogger<AuthController> logger, IMemoryCache cache)
    {
        _userManager = userManager;
        _logger = logger;
        _cache = cache;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        _logger.LogInformation("POST /api/auth/register chamado");

        if (userDto == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
        {
            _logger.LogWarning("Dados do usuário inválidos.");
            return BadRequest("Invalid user data.");
        }

        // Verifica se o usuário já existe no cache
        if (_cache.TryGetValue($"User_{userDto.Email}", out IdentityUser cachedUser))
        {
            _logger.LogInformation($"Usuário {userDto.Email} encontrado no cache.");
            return BadRequest("User already registered.");
        }

        var user = new IdentityUser { UserName = userDto.Email, Email = userDto.Email };
        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {
            _logger.LogWarning($"Erro ao registrar o usuário {userDto.Email}: {string.Join(", ", result.Errors)}");
            return BadRequest(result.Errors);
        }

        // Adiciona o usuário recém-criado ao cache
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache expira em 10 minutos
            SlidingExpiration = TimeSpan.FromMinutes(5) // Renova se acessado dentro de 5 minutos
        };
        _cache.Set($"User_{userDto.Email}", user, cacheOptions);

        _logger.LogInformation($"Usuário {userDto.Email} registrado com sucesso e adicionado ao cache.");
        return Ok("User registered successfully.");
    }

    public class UserDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
