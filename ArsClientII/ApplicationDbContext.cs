using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsClientII
{
    public class ApplicationDbContext : DbContext
    {


        public DbSet<Information>? Information { get; set; } = default;
        public DbSet<TranslationWords>? TranslationWords { get; set; } = default;
        public DbSet<CoinAnalysis>? CoinAnalysis { get; set; } = default;
        public DbSet<Trader>? Trader { get; set; } = default;
        public DbSet<News>? News { get; set; } = default;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
