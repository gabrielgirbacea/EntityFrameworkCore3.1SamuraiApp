using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Diagnostics;

namespace Tests
{
    [TestClass]
    public class InMemoryTests
    {
        [TestMethod]
        public void CanInsertSamuraiIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanInsertSamurai");

            using (var context = new SamuraiContext(builder.Options, true))
            {
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var samurai = new Samurai();
                context.Samurais.Add(samurai);
                Debug.WriteLine($"Before save: {samurai.Id}");
                context.SaveChanges();
                Debug.WriteLine($"AFter save: {samurai.Id}");

                Assert.AreNotEqual(0, samurai.Id);
            }
        }
    }
}
