using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OficinaTech.Infrastructure.Data.Context
{
    public class OficinaTechDbContextFactory : IDesignTimeDbContextFactory<OficinaTechDbContext>
    {
        public OficinaTechDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OficinaTechDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new OficinaTechDbContext(optionsBuilder.Options);
        }
    }
}
