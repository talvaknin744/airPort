using System;

namespace AirPortBl
{
    class Program
    {
        static void Main(string[] args)
        {
            TestLogic testLogic = new TestLogic();
            Logic logic = new Logic(testLogic);
            logic.CreateConnection();
            logic.InitTimer();

            Console.ReadKey();
        }
    }
}
