using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Vivelin.Home.Data
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Tagline> Taglines { get; set; }

        public async Task SeedAsync()
        {
            var migrations = await Database.GetPendingMigrationsAsync();
            if (migrations.Any())
                throw new InvalidOperationException("Unable to seed a database with pending migrations");

            await Database.ExecuteSqlCommandAsync("DELETE FROM Quotes");
            await Database.ExecuteSqlCommandAsync("DELETE FROM Taglines");

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

                Quotes.Add(new Quote
                {
                    Text = "We can dance if we want to<br>We can leave your friends behind<br>'Cause your friends don't dance and if they don't dance<br>Well, they're no friends of mine",
                    Citation = "Men Without Hats, <cite><a href='https://www.youtube.com/watch?v=0QDKLglEP5Y' target='_blank' rel='external'>The Safety Dance</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "It takes a lot to make a stew<br>A pinch of salt and laugher, too<br>A scoop of kids to add the spice<br>A dash of love to make it nice<br>And you've got",
                    Citation = "<cite class='too-many-cooks'><a href='https://www.youtube.com/watch?v=QrGrOK8oZG8' target='_blank' rel='external'>Too Many Cooks | Adult Swim</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "So, we’re starting with a game that we announced a few weeks ago through our friends at Walmart Canada… perhaps best known for their low prices and ability to keep a secret.",
                    Citation = "Pete Hines at the <cite><a href='https://youtu.be/OxGorVTMDIU?t=5295' target='_blank' rel='external'>2018 Bethesda E3 Showcase</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "<q>E3 has become such an incredible week of entertainment, but we know that most of you came here for one thing.</q><br><q>TODD HOWARD!</q>",
                    Citation = "Todd Howard and someone in the audience at the <cite><a href='https://youtu.be/oOceqR-rtLo?t=4392' target='_blank' rel='external'>2018 Bethesda E3 Showcase</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "…because evidently, these online games are hard; they can have some nasty issues… I read on the internet that our games have had a few bugs.",
                    Citation = "Todd Howard at the <cite><a href='https://youtu.be/OxGorVTMDIU?t=9159' target='_blank' rel='external'>2018 Bethesda E3 Showcase</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "…and that sometimes, it doesn’t <i>just work</i>.",
                    Citation = "Todd Howard at the <cite><a href='https://youtu.be/OxGorVTMDIU?t=9174' target='_blank' rel='external'>2018 Bethesda E3 Showcase</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Guys, the thermal drill! Go get it!",
                    Citation = "Bain in <cite><a href='https://youtu.be/vsW2sYiChCo' target='_blank' rel='external'>PAYDAY 2</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Do you get to the Cloud District very often? Oh, what am I saying—of course you don’t.",
                    Citation = "Nazeem in <cite><a href='https://youtu.be/Im8lQFEwShg' target='_blank' rel='external'>The Elder Scrolls V: Skyrim</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "I work with my mother, to sell fruits and vegetables. It’s fun most days, but hard work.",
                    Citation = "Mila Valentia in <cite><a href='https://youtu.be/ogcrW1Uns5s?t=30' target='_blank' rel='external'>The Elder Scrolls V: Skyrim</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "This rose has thorns, and here they are!",
                    Citation = "Female sylvari with <i>retaliation</i> in <cite><a href='https://youtu.be/V74blbz5Ywc' target='_blank' rel='external'>Guild Wars 2</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "I could outrun a centaur!",
                    Citation = "Female human with <i>swiftness</i> in <cite><a href='https://youtu.be/XNVSXIRcN5U' target='_blank' rel='external'>Guild Wars 2</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "You’re dumb, you’ll die, and you’ll leave a dumb corpse.",
                    Citation = "Female asura in <cite>Guild Wars 2</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Just one more confirmation of how <em>great</em> I am.",
                    Citation = "Female human getting an achievement in <cite>Guild Wars 2</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Honestly, if you could see just what was going on outside your realm of perception, it would blow your minds! That's the main reason I kept it back. If I were to give you an analogy, I'd describe it like this: when a duck swims on the water, you only see it glide, apparently effortlessly across the lake. But underneath, as in beneath the surface, it's a whole different story. Its legs are moving like he's pedaling a fucking bicycle up a mountain! Well, that's me right now. <strong>I am that duck!</strong>",
                    Citation = "Dr. Monty in Revelations, <cite><a href='https://youtu.be/MhmWCWaTucY' target='_blank' rel='external'>Call of Duty: Black Ops III</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "This, uh, next band asked me not to read this but goddamn it, I'm going to read it anyway, because I wrote it, and it's the truth. I fucking love this band! They are the best band ever, period! Ladies and gentlemen, Tenacious D!",
                    Citation = "<cite><a href='https://youtu.be/80DtQD5BQ_A' target='_blank' rel='external'>Tenacious D: The Pick of Destiny — Master Exploder</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "That’s what friends are for. Friendliest friends that ever friended. And they friended all the way to friendsville in their friend-mobile. Just friends forever!",
                    Citation = "Auralnauts, <cite><a href='https://youtu.be/gI8aSJBC9u0?t=735' target='_blank' rel='external'>STAR WARS EP 2: The Friend Zone</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "<dl class='dialog'><dt>Gunnery Chief</dt><dd>This, recruits, is a 20-kilo ferrous slug. Feel the weight. Every five seconds, the main gun of an Everest-class dreadnought accelerates one to 1.3 percent of light speed. It impacts with the force of a 38-kiloton bomb. That is three times the yield of the city buster dropped on Hiroshima back on Earth. That means Sir Isaac Newton is the deadliest son-of-a-bitch in space. Now! Serviceman Burnside! What is Newton's First Law?</dd>"
                         + "<dt>First Recruit</dt><dd>Sir! An object in motion stays in motion, sir!</dd>"
                         + "<dt>Gunnery Chief</dt><dd>No credit for partial answers, maggot!</dd>"
                         + "<dt>First Recruit</dt><dd>Sir! Unless acted on by an outside force, sir!</dd>"
                         + "<dt>Gunnery Chief</dt><dd>Damn straight! I dare to assume you ignorant jackasses know that space is empty. Once you fire this husk of metal, it keeps going till it hits something. That can be a ship, or the planet behind that ship. It might go off into deep space and hit somebody else in ten thousand years. If you pull the trigger on this, you're ruining someone's day, somewhere and sometime. That is why you check your damn targets! That is why you wait for the computer to give you a damn firing solution! That is why, Serviceman Chung, we do not <q>eyeball it!</q> This is a weapon of mass destruction. You are not a cowboy shooting from the hip!</dd>"
                         + "<dt>Second Recruit</dt><dd>Sir, yes sir!</dd></dl>",
                    Citation = "Gunnery Chief and two recruits in the Citadel in <cite><a href='https://youtu.be/p77XnhzJz7g' target='_blank' rel='external'>Mass Effect 2</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "<q>Oh, I'm very spiritual, you know. I can see auras. But I only use that for healing and helping people. I'm like a wisewoman, you know? I give people insight and advice.</q><br><q>Look, just because I say crazy things, that doesn't mean I believe everything.</q>",
                    Citation = "Alan Wake in <cite>Alan Wake's American Nightmare</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "You aren’t going to win this. I swear it.",
                    Citation = "Yadira Ban in Flashpoint: The Black Talon, <cite>Star Wars: The Old Republic</cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "<p>You just never quit, do you?" +
                        "<p>Took out Ghaul. Woke up the Traveler. And now half of what I hear on the streets is how much you and your clan are making a difference.",
                    Citation = "Suraya Hawthorne in <cite><a href='https://youtu.be/qY30_hCQZOI' target='_blank' rel='external'>Destiny 2</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Actual cannibal Shia LaBeouf",
                    Citation = "Rob Cantor, <cite><a href='https://youtu.be/o0u4M6vppCI' target='_blank' rel='external'>\"Shia LaBeouf\" Live</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Legendary fight with Shia LaBeouf<br>"
                         + "Normal Tuesday night for Shia LaBeouf<br>",
                    Citation = "Rob Cantor, <cite><a href='https://youtu.be/o0u4M6vppCI' target='_blank' rel='external'>\"Shia LaBeouf\" Live</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "You’re walking in the woods<br>"
                         + "There’s no one around and your phone is dead<br>"
                         + "Out of the corner of your eye, you spot him<br>"
                         + "<i>Shia LaBeouf</i>",
                    Citation = "Rob Cantor, <cite><a href='https://youtu.be/o0u4M6vppCI' target='_blank' rel='external'>\"Shia LaBeouf\" Live</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Wait! He isn’t dead! Shia surprise!<br>"
                         + "There’s a gun to your head, and death in his eyes<br>"
                         + "But you can do Jiu Jitsu<br>"
                         + "Body slam superstar Shia LaBeouf",
                    Citation = "Rob Cantor, <cite><a href='https://youtu.be/o0u4M6vppCI' target='_blank' rel='external'>\"Shia LaBeouf\" Live</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Baby I’m in the zone<br>"
                         + "Like a king without a throne<br>"
                         + "I’d rather have you by my side<br>"
                         + "Than play the skin trombone<br>"
                         + "I thought that what we had was real<br>"
                         + "But I couldn’t close the deal<br>"
                         + "I’m a piece of shit, cause baby<br>"
                         + "Baby I’m in the zone",
                    Citation = "Auralnauts, <cite><a href='https://youtu.be/6DD45wBDLNs' target='_blank' rel='external'>In the Zone</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Let me tell ya something...<br>"
                         + "Sleepin’ makes me feel good!<br>"
                         + "<i>[snoring]</i><br>"
                         + "I ain’t afraid of no sleep!<br>"
                         + "I ain’t afraid of no bed!<br><br>"
                         + "An invisible bed...<br>"
                         + "A freaky ghost bed!<br>"
                         + "Yeah! Yeah! Yeah! Yeah! Yeah! Yeah!",
                    Citation = "Neil Cicierega, <cite><a href='https://youtu.be/0tdyU_gW6WE' target='_blank' rel='external'>Bustin</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN BUSTIN Bustin’ makes me feel good!",
                    Citation = "Neil Cicierega, <cite><a href='https://youtu.be/0tdyU_gW6WE' target='_blank' rel='external'>Bustin</a></cite>"
                });

                Quotes.Add(new Quote
                {
                    Text = "Everyone told me not to stroll on that beach<br>"
                         + "Said seagulls gonna come<br>"
                         + "Poke me in the coconut<br>"
                         + "And they did<br>"
                         + "And they did<br>",
                    Citation = "Bad Lip Reading, <cite><a href='https://youtu.be/U9t-slLl30E' target='_blank' rel='external'>SEAGULLS! (Stop It Now)</a></cite>"
                });

                await SaveChangesAsync();
            }

            var hasTaglines = await Taglines.AnyAsync();
            if (!hasTaglines)
            {
                Taglines.Add(new Tagline
                {
                    Text = "Nothing to see here, please move along."
                });

                await SaveChangesAsync();
            }
        }
    }
}