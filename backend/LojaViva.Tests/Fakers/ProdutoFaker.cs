using Bogus;
using LojaViva.API.Models;

namespace LojaViva.Tests.Fakers
{
    public static class ProdutoFaker
    {
        public static Faker<Produto> GenerateProduto()
        {
            return new Faker<Produto>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1) // IDs sequenciais
                .RuleFor(p => p.Nome, f => f.Commerce.ProductName()) // Nome do produto fictício
                .RuleFor(p => p.Preco, f => f.Finance.Amount(10, 500)) // Preço fictício
                .RuleFor(p => p.Estoque, f => f.Random.Int(0, 100)); // Quantidade em estoque
        }
    }
}
