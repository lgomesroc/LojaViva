using LojaViva.API.Models;
using LojaViva.Tests.Fakers;
using Xunit;

namespace LojaViva.Tests
{
    public class ClienteTests
    {
        [Fact]
        public void DeveGerarClienteFicticio()
        {
            // Gerar um cliente fictício
            var clienteFaker = ClienteFaker.GenerateCliente();
            var clienteFicticio = clienteFaker.Generate();

            // Validar que o cliente foi gerado
            Assert.NotNull(clienteFicticio);
            Assert.False(string.IsNullOrEmpty(clienteFicticio.Nome));
            Assert.False(string.IsNullOrEmpty(clienteFicticio.Email));
        }
    }
}
