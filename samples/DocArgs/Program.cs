using Commander;
using System;

namespace DocArgs
{
    class Program
    {
        [Command]
        public static void DoSomething(string arg0, int arg1 = 2)
        {
            Console.WriteLine($"arg0 = {arg0}, arg1 = {arg1}");
        }

        static void Main(string[] args)
        {
            Service.SubmitCommandString("DoSomething");
        }
    }
}
