using LojaViva.API.Models;
using LojaViva.Tests.Fakers;
using Xunit;

namespace LojaViva.Tests
{
    public class ProdutoTests
    {
        [Fact]
        public void DeveGerarProdutoFicticio()
        {
            // Gerar um produto fictício
            var produtoFaker = ProdutoFaker.GenerateProduto();
            var produtoFicticio = produtoFaker.Generate();

            // Validar que o produto foi gerado
            Assert.NotNull(produtoFicticio);
            Assert.False(string.IsNullOrEmpty(produtoFicticio.Nome));
            Assert.True(produtoFicticio.Preco > 0);
            Assert.True(produtoFicticio.Estoque >= 0);
        }
    }
}
