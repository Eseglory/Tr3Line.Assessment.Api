using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tr3Line.Assessment.Api.Entities;
using Tr3Line.Assessment.Entities;

namespace Tr3Line.Assessment.Helpers
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CompareResult> CompareResults { get; set; }

        
        private readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("Tr3Line.Assessment.Api.Database"));
        }
    }
}