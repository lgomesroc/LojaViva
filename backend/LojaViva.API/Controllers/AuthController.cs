using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LojaViva.API.Models;
using LojaViva.API.Repositories;
using Microsoft.Extensions.Logging;

namespace LojaViva.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IClienteRepository clienteRepository,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _clienteRepository = clienteRepository;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("POST /api/auth/login chamado");

            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Senha))
            {
                _logger.LogWarning("Dados de login inválidos.");
                return BadRequest("Dados de login inválidos.");
            }

            // Verifica as credenciais do cliente
            var isValid = _clienteRepository.ValidateCredentials(model.Email, model.Senha);
            if (!isValid)
            {
                _logger.LogWarning($"Credenciais inválidas para o email {model.Email}.");
                return Unauthorized("Email ou senha inválidos.");
            }

            // Busca o cliente para obter informações adicionais
            var cliente = _clienteRepository.GetByEmail(model.Email);
            if (cliente == null)
            {
                _logger.LogError($"Cliente com email {model.Email} não encontrado após validação bem-sucedida.");
                return StatusCode(500, "Erro interno do servidor.");
            }

            // Gera o token JWT
            var token = GerarToken(cliente);

            _logger.LogInformation($"Cliente {model.Email} autenticado com sucesso.");
            return Ok(new { token, cliente.Nome, cliente.Email });
        }

        [HttpPost("registro")]
        public IActionResult Registro([FromBody] Cliente cliente)
        {
            _logger.LogInformation("POST /api/auth/registro chamado");

            if (cliente == null || string.IsNullOrEmpty(cliente.Email) || string.IsNullOrEmpty(cliente.Senha))
            {
                _logger.LogWarning("Dados do cliente inválidos.");
                return BadRequest("Dados do cliente inválidos.");
            }

            // Verifica se o cliente já existe
            var clienteExistente = _clienteRepository.GetByEmail(cliente.Email);
            if (clienteExistente != null)
            {
                _logger.LogWarning($"Cliente com email {cliente.Email} já existe.");
                return BadRequest("Email já cadastrado.");
            }

            try
            {
                // Adiciona o novo cliente
                _clienteRepository.Add(cliente);

                _logger.LogInformation($"Cliente {cliente.Email} registrado com sucesso.");
                return Ok("Cliente registrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao registrar cliente {cliente.Email}");
                return StatusCode(500, "Erro ao registrar cliente.");
            }
        }

        private string GerarToken(Cliente cliente)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(key))
            {
                _logger.LogError("A chave JWT não está configurada.");
                throw new InvalidOperationException("A chave JWT não está configurada.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, cliente.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, cliente.Email),
                new Claim(ClaimTypes.Name, cliente.Nome ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Token válido por 2 horas
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}