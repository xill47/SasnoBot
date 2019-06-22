using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SasnoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var bot = new SasnoBot(args))
            {
                await bot.Configure();
                Console.WriteLine("Configuration finished");
                await Task.Delay(-1);
            }
        }
    }
}
