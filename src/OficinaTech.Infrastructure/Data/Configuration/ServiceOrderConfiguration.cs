using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Data.Configuration
{
    public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
    {
        public void Configure(EntityTypeBuilder<ServiceOrder> builder)
        {
            builder.ToTable("service_orders");
           
            builder.HasKey(so => so.Id);
            
            builder.Property(so => so.Id)
                   .UseIdentityAlwaysColumn()
                   .IsRequired();
            
            builder.Property(so => so.Number)
                   .IsRequired()
                   .HasMaxLength(50);
            
            builder.HasIndex(so => so.Number).IsUnique(); // Índice para performance
            
            builder.HasOne(so => so.Orcamento)
                   .WithOne(so => so.ServiceOrder)
                   .HasForeignKey<ServiceOrder>(so => so.OrcamentoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
