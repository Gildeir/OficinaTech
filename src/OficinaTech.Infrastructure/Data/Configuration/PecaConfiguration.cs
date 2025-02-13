using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Data.Configuration
{
    public class PecaConfiguration : IEntityTypeConfiguration<Peca>
    {
        public void Configure(EntityTypeBuilder<Peca> builder)
        {
            builder.ToTable("pecas");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .UseIdentityAlwaysColumn()
                   .IsRequired();

            builder.Property(p => p.Nome)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Estoque)
                   .IsRequired();

            builder.Property(p => p.Preco)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.HasMany(p => p.OrcamentoPecas)
                   .WithOne(op => op.Peca)
                   .HasForeignKey(op => op.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
