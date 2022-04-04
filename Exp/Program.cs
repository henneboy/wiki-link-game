using System;
using System.Threading.Tasks;

namespace Exp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string s = "not";
            //s = Tester.idk("xd").GetAwaiter().GetResult();
            s = Tester.idk("xd").GetAwaiter().GetResult();
            Console.WriteLine(s);
            Console.WriteLine("Hello World!");
        }
    }

    public static class Tester
    {
        public static async Task<string> idk(string s)
        {
            await waiter();
            //Thread.Sleep(2000);
            return s;
        }

        public static async Task waiter()
        {
            await Task.Run(() =>
            {
                Task.Delay(3000).Wait();
            });
        }
    }
}