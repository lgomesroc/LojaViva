using LojaViva.API.Models;
using LojaViva.Tests.Fakers;
using Xunit;

namespace LojaViva.Tests
{
    public class PedidoTests
    {
        [Fact]
        public void DeveGerarPedidoFicticio()
        {
            // Gerar um pedido fictício
            var pedidoFaker = PedidoFaker.GeneratePedido();
            var pedidoFicticio = pedidoFaker.Generate();

            // Validar que o pedido foi gerado
            Assert.NotNull(pedidoFicticio);
            Assert.True(pedidoFicticio.Total > 0);
            Assert.False(string.IsNullOrEmpty(pedidoFicticio.Data.ToString()));
        }
    }
}
