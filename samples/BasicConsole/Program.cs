using Commander;
using System;
using System.Drawing;

namespace BasicConsole
{
    class Program
    {
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
                catch(Exception ex)
                {
                    // catch and print any exceptions
                    Service.Output.WriteLine(ex.Message, Color.Red);
                }
            }
        }
    }
}
