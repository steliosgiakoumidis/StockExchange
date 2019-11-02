using DbUp;
using System;
using System.Reflection;

namespace DbSetUp
{
    class Program
    {
        static void Main(string[] args)
        {
            var upgradeEngine = DeployChanges.To
                .SqlDatabase("Server=.\\sqlexpress;Database=alpcot;Trusted_Connection=True;")
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .LogToConsole()
                .Build();

            EnsureDatabase.For.SqlDatabase("Server=.\\sqlexpress;Database=alpcot;Trusted_Connection=True;");
            if (upgradeEngine.IsUpgradeRequired())
                upgradeEngine.PerformUpgrade();
        }
    }
}
