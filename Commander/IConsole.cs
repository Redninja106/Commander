using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Commander
{
    /// <summary>
    /// An interface representing a console input and output.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// The output color of the console. This is not the color of all the visible text, instead it is the color which any newly printed text will be and will stay.
        /// </summary>
        Color Color { get; set; }

        void WriteLine();
        void WriteLine(object obj);
        void WriteLine(object obj, Color color);
        void Write(object obj);
        void Write(object obj, Color color);
        string ReadLine();

    }
}
