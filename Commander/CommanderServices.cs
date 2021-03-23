﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Commander
{
    public static class CommanderServices
    {
        [Command("Commander")]
        [Doc("Prints the provided string a number of times.")]
        [DocArg("string", "words", "The string to print.")]
        [OpDocArg("int", "count", "1", "The number of times to print the string.")]
        public static void Say(string words, int count = 1)
        {
            for (int i = 0; i < count; i++)
                Console.WriteLine(words);
        }

        [Command("Commander")]
        [Doc("Calls Enviroment.Exit() with either the provided code or 0")]
        [OpDocArg("int", "code", "0", "The exit code of the program.")]
        public static void Exit(int code = 0)
        {
            Environment.Exit(code);
        }

        [Command("Commander")]
        [Doc("Provides information on available commands.")]
        public static void Help()
        {
            var o = Service.Output;

            o.WriteLine("Available commands:", Service.Style.Lowlight);

            foreach (var cmd in Service.RegisteredCommands)
            {
                o.Write(cmd.ToString() + " - ");
                var doc = cmd.MethodInfo.GetCustomAttribute<DocAttribute>();
                o.WriteLine(doc?.Doc, Service.Style.Text);
            }
        }
    }
}
