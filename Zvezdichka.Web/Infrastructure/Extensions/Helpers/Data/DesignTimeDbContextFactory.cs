using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Zvezdichka.Data;

namespace Zvezdichka.Web.Infrastructure.Extensions.Helpers.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ZvezdichkaDbContext>
    {
        public ZvezdichkaDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ZvezdichkaDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new ZvezdichkaDbContext(builder.Options);
        }
    }
}