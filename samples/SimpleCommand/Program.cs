using Commander;
using System;

namespace SimpleCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new CommandContext();

            while (true)
            {
                if (! context.TrySubmitCommand(Console.ReadLine()))
                {
                    Console.WriteLine(context.GetLastErrorDescription());
                }
            }
        }
    }

    class MyService
    {
        [Command]
        public static void Add(params char[] a)
        {
        }
    }
}
