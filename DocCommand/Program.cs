using Commander;
using System;
using System.Drawing;

namespace DocCommand
{
    class Program
    {
        [Command]
        [Doc("Prints hello world to the console.")]
        public static void PrintHelloWorld()
        {
            Console.WriteLine("Hello world");
        }

        static void Main(string[] args)
        {
            // set service output to the system console. That is the default, but do this for clarity.
            Service.SetOutputToSystemConsole();

            // just process commands until stopped
            while (true)
            {
                try
                {
                    // submit typed command
                    Service.SubmitCommandString(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    // catch and print any exceptions
                    Service.Output.WriteLine(ex.Message, Color.Red);
                }
            }
        }
    }
}
