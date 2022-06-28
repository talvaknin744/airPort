using System;

namespace AirPortSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            AirPortSimulator simulator = new AirPortSimulator();
            simulator.InitHubConnection().GetAwaiter().GetResult();
            simulator.InitTimer();
            Console.ReadKey();
        }
    }
}
