using Commander;
using System;
using System.Drawing;

namespace Piping
{
    class Program
    {
        [Command]
        public static string GetText()
        {
            return "Hello world";
        }

        // If the LastResult attribute is used on a parameter, the parameter is ignored when typing the command, and instead passed by commander with the return value of the last command executed.
        [Command]
        public static void PrintText([LastResult] string text)
        {
            Console.WriteLine(text);
        }

        static void Main(string[] args)
        {
            // call get text, then print text. get text returns "hello world", and print text prints the return value of the last command.
            string command1 = "getText";
            Service.SubmitCommandString(command1);

            string command2 = "printText";
            Service.SubmitCommandString(command2);

            // the '|' symbol can be used to call multiple commands with only one line.
            string command3 = "getText | printText";
            Service.SubmitCommandString(command3);

            Console.ReadLine();
        }
    }
}
