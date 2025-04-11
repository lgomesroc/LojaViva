using LojaViva.API.Models;
using LojaViva.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LojaViva.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        [HttpGet]
        public IActionResult GetPedidos()
        {
            var pedidos = _pedidoRepository.GetAll();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public IActionResult GetPedido(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return NotFound(new { Message = "Pedido não encontrado." }); // Retorna mensagem personalizada

            return Ok(pedido);
        }

                [HttpPost]
        public IActionResult AddPedido([FromBody] Pedido pedido)
        {
            _pedidoRepository.Add(pedido);
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePedido(int id, [FromBody] Pedido pedido)
        {
            var pedidoExistente = _pedidoRepository.GetById(id);
            if (pedidoExistente == null) return NotFound(new { Message = "Pedido não encontrado." });

            pedido.Id = id;
            _pedidoRepository.Update(pedido);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePedido(int id)
        {
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null) return NotFound(new { Message = "Pedido não encontrado." });

            _pedidoRepository.Delete(id);
            return NoContent();
        }
    }
}

