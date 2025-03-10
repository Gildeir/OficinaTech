using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Data.Configuration
{
    public class OrcamentoPecaConfiguration : IEntityTypeConfiguration<OrcamentoPeca>
    {
        public void Configure(EntityTypeBuilder<OrcamentoPeca> builder)
        {
            builder.ToTable("orcamento_pecas");

            builder.HasKey(op => op.Id);
            builder.Property(op => op.Id)
                   .UseIdentityAlwaysColumn()
                   .IsRequired();

            builder.HasOne(op => op.Orcamento)
                   .WithMany(o => o.OrcamentoPecas)
                   .HasForeignKey(op => op.OrcamentoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(op => op.Peca)
                   .WithMany(p => p.OrcamentoPecas)
                   .HasForeignKey(op => op.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(op => op.Quantidade)
                   .IsRequired();

            builder.Property(op => op.LiberadaParaCompra)
                   .IsRequired();

            builder.Property(op => op.Status)
                   .HasConversion<string>()
                   .IsRequired();
        }
    }
}
