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
            
            var hasQuotes = await Quotes.AnyAsync();
            if (!hasQuotes)
            {
                Quotes.Add(new Quote
                {
                    Text = "<q>I speak metaphorically, my lord,</q> he said. Russ nodded, still amused. <q>That's all right, Constantin. Sometimes I dismember metaphorically.</q>",
                    Citation = "Dan Abnett. <cite>Prospero Burns</cite> (Black Library, 2014), 331."
                });

                Quotes.Add(new Quote
                {
                    Text = "Not a wise decision, but a decision nonetheless.",
                    Citation = "<cite>Age of Mythology</cite>, <a href='https://youtu.be/-EvhQcmNORo?t=85' target='_blank' rel='external'>taunt 37</a>."
                });

                Quotes.Add(new Quote
                {
                    Text = "<q>I had no idea who she is. I knew as much about myself and what I wanted as I do about how to squeeze a fruit in a grocery store and know if it's gonna be good in a meal. I still don't know; I'd pick up a lemon and squeeze it and I'm like <q>Well, it's definitely a lemon... 'cause there's a label here that says so... Guess we're gonna order out tonight.</q></q>",
                    Citation = "Sean Plott. <cite><a href='https://youtu.be/CdLnuGAPNUg?t=195' target='_blank' rel='external'>Day[9] Story Time #5 - A High School Crush</a></cite>."
                });

                await SaveChangesAsync();
            }
        }
    }
}
