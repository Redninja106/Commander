using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace Commander
{
    internal sealed class SystemConsole : IConsole
    {
        public static readonly SystemConsole Instance = new SystemConsole();

        public Color Color { get => Color.FromName(Console.ForegroundColor.ToString()); set => Console.ForegroundColor = FromColor(value); }

        public void Write(object obj)
        {
            Console.Write(obj);
        }

        public void WriteLine(object obj)
        {
            Console.WriteLine(obj);
        }

        public void WriteLine(object obj, Color color)
        {
            var oldColor = this.Color;
            
            this.Color = color;

            Console.WriteLine(obj);

            this.Color = oldColor;
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void Write(object obj, Color color)
        {
            var oldColor = this.Color;

            this.Color = color;

            Console.Write(obj);

            this.Color = oldColor;
        }

        // adapted from https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor
        private static System.ConsoleColor FromColor(System.Drawing.Color c)
        {
            int index = (c.A < 255 | c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0; // Bright bit
            index |= (c.R > 64) ? 4 : 0; // Red bit
            index |= (c.G > 64) ? 2 : 0; // Green bit
            index |= (c.B > 64) ? 1 : 0; // Blue bit
            return (System.ConsoleColor)index;
        }
    }
}
