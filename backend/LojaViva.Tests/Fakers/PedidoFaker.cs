using Bogus;
using LojaViva.API.Models;

namespace LojaViva.Tests.Fakers
{
    public static class PedidoFaker
    {
        public static Faker<Pedido> GeneratePedido()
        {
            return new Faker<Pedido>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1) // IDs sequenciais
                .RuleFor(p => p.ClienteId, f => f.Random.Int(1, 100)) // ID do cliente fictício
                .RuleFor(p => p.Total, f => f.Finance.Amount(50, 1000)) // Valor total fictício
                .RuleFor(p => p.Data, f => f.Date.Past(1)); // Data fictícia (último ano)
        }
    }
}
