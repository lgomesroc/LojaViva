using Bogus;
using LojaViva.API.Models;

namespace LojaViva.Tests.Fakers
{
    public static class ClienteFaker
    {
        public static Faker<Cliente> GenerateCliente()
        {
            return new Faker<Cliente>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1) // Gera IDs sequenciais
                .RuleFor(c => c.Nome, f => f.Name.FullName()) // Nome completo aleatório
                .RuleFor(c => c.Email, f => f.Internet.Email()) // E-mail fictício
                .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber()); // Número de telefone fictício
        }
    }
}
