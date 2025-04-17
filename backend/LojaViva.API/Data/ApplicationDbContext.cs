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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Cliente
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Configuração da relação entre Pedido e Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // Para configurar o DbContext se nenhuma opção explícita for passada
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Aqui, configuramos uma string de conexão padrão para cenários de teste
                optionsBuilder.UseMySql(
                    "Server=localhost;Port=3306;Database=LojaVivaDB;User=lojaviva_user;Password=lojaviva123;",
                    ServerVersion.AutoDetect("Server=localhost;Port=3306;Database=LojaVivaDB;User=lojaviva_user;Password=lojaviva123;")
                );
            }
        }
    }
}
