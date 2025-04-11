using LojaViva.API.Models;
using LojaViva.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LojaViva.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogger<ClientesController> _logger;
        private readonly IMemoryCache _cache;

        public ClientesController(IClienteRepository clienteRepository, ILogger<ClientesController> logger, IMemoryCache cache)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
            _cache = cache;
        }

        // GET: api/clientes
        [HttpGet]
        public IActionResult GetClientes()
        {
            _logger.LogInformation("GET /api/clientes chamado");

            // Tentando obter dados do cache
            if (!_cache.TryGetValue("ClientesCache", out List<Cliente> clientes))
            {
                _logger.LogInformation("Cache vazio. Recuperando clientes do repositório.");

                // Recuperar dados do repositório
                clientes = _clienteRepository.GetAll().ToList();

                // Configuração de cache (expira em 5 minutos ou se não for acessado em 2 minutos)
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                // Adicionar clientes ao cache
                _cache.Set("ClientesCache", clientes, cacheOptions);
                _logger.LogInformation("Clientes adicionados ao cache.");
            }
            else
            {
                _logger.LogInformation("Clientes recuperados do cache.");
            }

            return Ok(clientes);
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public IActionResult GetCliente(int id)
        {
            _logger.LogInformation($"GET /api/clientes/{id} chamado");
            var cliente = _clienteRepository.GetById(id);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente com ID {id} não encontrado");
                return NotFound();
            }
            _logger.LogInformation($"Cliente com ID {id} encontrado: {cliente.Nome}");
            return Ok(cliente);
        }

        // POST: api/clientes
        [HttpPost]
        public IActionResult PostCliente([FromBody] Cliente cliente)
        {
            _logger.LogInformation($"POST /api/clientes chamado para adicionar cliente: {cliente.Nome}");
            _clienteRepository.Add(cliente);

            // Invalida o cache para forçar a atualização
            _cache.Remove("ClientesCache");

            _logger.LogInformation($"Cliente {cliente.Nome} adicionado com sucesso, ID: {cliente.Id}");
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        // PUT: api/clientes/{id}
        [HttpPut("{id}")]
        public IActionResult PutCliente(int id, [FromBody] Cliente cliente)
        {
            _logger.LogInformation($"PUT /api/clientes/{id} chamado para atualizar cliente.");
            var existingCliente = _clienteRepository.GetById(id);
            if (existingCliente == null)
            {
                _logger.LogWarning($"Cliente com ID {id} não encontrado para atualização.");
                return NotFound();
            }

            cliente.Id = id;
            _clienteRepository.Update(cliente);

            // Invalida o cache para forçar a atualização
            _cache.Remove("ClientesCache");

            _logger.LogInformation($"Cliente com ID {id} atualizado com sucesso.");
            return NoContent();
        }

        // DELETE: api/clientes/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCliente(int id)
        {
            _logger.LogInformation($"DELETE /api/clientes/{id} chamado para excluir cliente.");
            var cliente = _clienteRepository.GetById(id);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente com ID {id} não encontrado para exclusão.");
                return NotFound();
            }

            _clienteRepository.Delete(id);

            // Invalida o cache para forçar a atualização
            _cache.Remove("ClientesCache");

            _logger.LogInformation($"Cliente com ID {id} excluído com sucesso.");
            return NoContent();
        }
    }
}
