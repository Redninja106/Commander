using Commander;
using Commander.Documentation;
using System;

namespace AddExample
{
    class Program
    {
        [Command]
        [Doc("Adds two numbers together")]
        [DocArg("int", "a", "The first number to add")]
        [DocArg("int", "b", "The second number to add")]
        public static void Add(int a, int b)
        {
            Console.WriteLine(a + b);
        }

        static void Main(string[] args)
        {
            // print the docs of add
            Service.SubmitCommandString("doc add");

            // add 40 and 60
            Service.SubmitCommandString("add 40 60");

            Console.ReadLine();
        }
    }
}
