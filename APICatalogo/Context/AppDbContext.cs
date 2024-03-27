using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } //a classe base será a DbContext e seus atributos.

        public DbSet<Categoria>? Categoria { get; set; }
        public DbSet<Produtos>? Produtos { get; set; }
    }
}
