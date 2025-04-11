using Microsoft.EntityFrameworkCore;
using LojaViva.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LojaViva.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>    
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Construtor sem parâmetros para uso em testes
        public ApplicationDbContext() { }

        // DbSets para as tabelas
        public virtual DbSet<Produto> Produtos { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
    }
}