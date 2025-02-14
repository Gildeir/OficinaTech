using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Data.Configuration
{
    public class MovimentacaoEstoqueConfiguration : IEntityTypeConfiguration<MovimentacaoEstoque>
    {
        public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
        {
            builder.ToTable("movimentacoes_estoque");

            builder.HasKey(m => m.Id);
            builder.Property(o => o.Id)
                   .UseIdentityAlwaysColumn();

            builder.Property(m => m.Quantidade)
                   .IsRequired();

            builder.Property(m => m.Tipo)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(m => m.DataMovimentacao)
                   .IsRequired();

            builder.HasOne(m => m.Peca)
                   .WithMany()
                   .HasForeignKey(m => m.PecaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Peca)
                   .WithMany(p => p.MovimentacoesEstoque)
                   .HasForeignKey(m => m.PecaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
