using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Vivelin.Home.Data
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Quote> Quotes { get; set; }

        public async Task SeedAsync()
        {
            var migrations = await Database.GetPendingMigrationsAsync();
            if (migrations.Any())
                throw new InvalidOperationException("Unable to seed a database with pending migrations");

            Quotes.Add(new Quote
            {
                Text = "<q>I speak metaphorically, my lord,</q> he said. Russ nodded, still amused. <q>That's all right, Constantin. Sometimes I dismember metaphorically.</q>",
                Citation = "Abnett, Dan. Prospero Burns, p. 331"
            });

            await SaveChangesAsync();
        }
    }
}
