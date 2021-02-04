using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            //context.Database.EnsureCreated();
            //GetSamurais("Before Add");
            //AddSamurai();
            //InsertSamurais(10);
            //RetrieveAndUpdateMultipleSamurais();
            //RetrieveAndDeleteASamurai(1);
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            //InsertNewSamuraiWithAQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(2);
            //AddQuoteToExistingSamuraiNotTracked_Easy(2);

            EagerLoadSamuraiWithQuotes();
            ProjectSamuraisWithQuotes();
            ProjectSomeProperties();

            GetSamurais("After Add");
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void InsertSamurais(int noOfSamurais)
        {
            var samurais = new List<Samurai>();
            for (int i = 0; i < noOfSamurais; i++)
            {
                samurais.Add(new Samurai() { Name = $"Samurai{i}" });
            }

            _context.Samurais.AddRange(samurais);
            _context.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai() { Name = "Julie" };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryFilters(string nameFilter)
        {
            //var samurais = _context.Samurais.Where(s => s.Name == nameFilter).ToList();
            //var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "J")).ToList();
            //var samurais = _context.Samurais.Find(2);
            //var lastSamurai = _context.Samurais.OrderBy(s => s.Id).LastOrDefault();
            //var lastSamuraiNoOrder = _context.Samurais.LastOrDefault(); // will fail.
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }

        private static void RetrieveAndDeleteASamurai(int id)
        {
            var samurai = _context.Samurais.Find(id);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle()
            {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15),
            });
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.AsNoTracking().FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);

            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai()
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>()
                {
                    new Quote ()
                    {
                        Text = "I've come to save you"
                    }
                }
            };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai()
            {
                Name = "Kyūzō",
                Quotes = new List<Quote>()
                {
                    new Quote ()
                    {
                        Text = "Watch out for my sharp sword!"
                    },
                    new Quote ()
                    {
                        Text="I told you to watch out for the sharp sword! Oh well!" 
                    }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(
                new Quote()
                {
                    Text = "I bet you're happy that I've saved you!"
                });

            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(
                new Quote()
                {
                    Text = "Now that I saved you, will you feed me dinner?"
                });

            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiNotTracked_Easy(int samuraiId)
        {
            var quote = new Quote()
            {
                Text = "Now that I saved you, will you feed me dinner again?",
                SamuraiId = samuraiId
            };

            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais.Where(s => s.Name.Contains("Julie"))
                                                     .Include(s => s.Quotes)
                                                     .Include(s => s.Clan)
                                                     .FirstOrDefault();
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idsAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }

        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }

        private static void ProjectSamuraisWithQuotes()
        {
            //var somePropertiesWithQuotes = _context.Samurais
            //   .Select(s => new { s.Id, s.Name, s.Quotes.Count })
            //   .ToList();
            //var somePropertiesWithQuotes = _context.Samurais
            //   .Select(s => new { s.Id, s.Name,
            //     HappyQuotes=s.Quotes.Where(q=>q.Text.Contains("happy")) })
            //   .ToList();
            var samuraisWithHappyQuotes = _context.Samurais
               .Select(s => new {
                   Samurai = s,
                   HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
               })
               .ToList();
        }


    }
}
