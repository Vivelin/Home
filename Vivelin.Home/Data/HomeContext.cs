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

#if DEBUG
            await Database.ExecuteSqlCommandAsync("DELETE FROM Quotes");
#endif

            var hasQuotes = await Quotes.AnyAsync();
            if (!hasQuotes)
            {
                Quotes.Add(new Quote
                {
                    Text = "<q>I speak metaphorically, my lord,</q> he said.<br>Russ nodded, still amused.<br><q>That’s all right, Constantin. Sometimes I dismember metaphorically.</q>",
                    Citation = "Dan Abnett. <cite>Prospero Burns</cite> (Black Library, 2014), 331."
                });

                Quotes.Add(new Quote
                {
                    Text = "Not a wise decision, but a decision nonetheless.",
                    Citation = "<cite>Age of Mythology</cite>, <a href='https://youtu.be/-EvhQcmNORo?t=85' target='_blank' rel='external'>taunt 37</a>."
                });

                Quotes.Add(new Quote
                {
                    Text = "I had no idea who she is. I knew as much about myself and what I wanted as I do about how to squeeze a fruit in a grocery store and know if it’s gonna be good in a meal. I still don’t know; I’d pick up a lemon and squeeze it and I’m like <q>Well, it’s definitely a lemon... ’cause there's a label here that says so... Guess we’re gonna order out tonight.</q>",
                    Citation = "Sean Plott. <cite><a href='https://youtu.be/CdLnuGAPNUg?t=195' target='_blank' rel='external'>Day[9] Story Time #5 - A High School Crush</a></cite>."
                });

                Quotes.Add(new Quote
                {
                    Text = "Jelly… Jelly… I'm gonna call you <i>strawberry preserve</i> ’cause you're so je— Don't look at me like that!",
                    Citation = "Jesse Cox. <cite><a href='https://youtu.be/fY9i91pEFWQ?t=1789' target='_blank' rel='external'>The Infectious Madness of Doctor Dekker w/ Dodger [Part 4] - Loop Day</a></cite>."
                });

                Quotes.Add(new Quote
                {
                    Text = "You dare steal in my presence?! That will cost you your life!",
                    Citation = "<cite>Assassin’s Creed</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Dirty thief! I’ll have your hand for that!",
                    Citation = "<cite>Assassin’s Creed</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "GO GO GO!",
                    Citation = "<cite><a href='https://www.youtube.com/watch?v=ElXnQEr8rlU' target='_blank' rel='external'>Mass Effect</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Hold the line!",
                    Citation = "<cite>Mass Effect</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "ENEMY IS EVERYWHERE!",
                    Citation = "<cite><a href='https://www.youtube.com/watch?v=FOwSHXK0444' target='_blank' rel='external'>Mass Effect</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "I will destroy you!",
                    Citation = "<cite>Mass Effect</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "User alert! Main reactor shut down in accordance with emergency containment procedures. Manual restart required.",
                    Citation = "<cite>Mass Effect</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Guilliman nods. The nod means shut up. Thiel shuts up.",
                    Citation = "Dan Abnett. <cite>Know No Fear</cite> (Black Library, 2016), 94."
                });

                await SaveChangesAsync();
            }
        }
    }
}