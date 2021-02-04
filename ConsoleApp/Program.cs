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
            //RetrieveAndReleteASamurai(1);

            //RetrieveAndDeleteASamurai(1);
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();



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
             
    }
}
