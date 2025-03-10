using Microsoft.EntityFrameworkCore;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Data.Configuration;

namespace OficinaTech.Infrastructure.Data.Context
{
    public class OficinaTechDbContext : DbContext
    {
        public OficinaTechDbContext(DbContextOptions<OficinaTechDbContext> options) : base(options)
        {

        }

        public DbSet<Orcamento> Orcamentos { get; set; }
        public DbSet<Peca> Pecas { get; set; }
        public DbSet<OrcamentoPeca> OrcamentoPecas { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration<Orcamento>(new OrcamentoConfiguration());
            modelBuilder.ApplyConfiguration<Peca>(new PecaConfiguration());
            modelBuilder.ApplyConfiguration<OrcamentoPeca>(new OrcamentoPecaConfiguration());
            modelBuilder.ApplyConfiguration<MovimentacaoEstoque>(new MovimentacaoEstoqueConfiguration());

            modelBuilder.Entity<Peca>().HasData(
                new Peca { Id = -1, Nome = "Filtro de Óleo", Estoque = 10, Preco = 25.90m },
                new Peca { Id = -2, Nome = "Pastilha de Freio", Estoque = 15, Preco = 120.50m },
                new Peca { Id = -3, Nome = "Correia Dentada", Estoque = 8, Preco = 75.30m }
            );
        }

    }
}
