using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Data.Configuration
{
    public class OrcamentoConfiguration : IEntityTypeConfiguration<Orcamento>
    {
        public void Configure(EntityTypeBuilder<Orcamento> builder)
        {
            builder.ToTable("orcamentos");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                   .UseIdentityAlwaysColumn()
                   .IsRequired();

            builder.Property(o => o.Numero)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.HasIndex(o => o.Numero).IsUnique(); // Índice para performance

            builder.Property(o => o.PlacaVeiculo)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(o => o.Cliente)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(o => o.Total)
                   .HasColumnType("decimal(10,2)");
                   
            builder.HasMany(o => o.OrcamentoPecas)
                   .WithOne(op => op.Orcamento)
                   .HasForeignKey(op => op.OrcamentoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.ServiceOrder)
                     .WithOne(so => so.Orcamento)
                     .HasForeignKey<ServiceOrder>(so => so.OrcamentoId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
