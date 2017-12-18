using System.Diagnostics;
using System.Threading.Tasks;

namespace SasnoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var startup = new SasnoBot())
            {
                await startup.Configure();
                Debug.WriteLine("Configuration finished");
                await Task.Delay(-1);
            }
        }
    }
}
