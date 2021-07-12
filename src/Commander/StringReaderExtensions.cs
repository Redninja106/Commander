using System;
using System.IO;

namespace Commander
{
    /// <summary>
    /// Conatains extensions for the StringReader class for easier internal command processing.
    /// </summary>
    internal static class StringReaderExtensions
    {
        public static string ReadWord(this StringReader stringReader, bool strict = false)
        {
            stringReader.ReadWhile(c => char.IsWhiteSpace(c));

            return stringReader.ReadWhile(current => strict ? char.IsLetterOrDigit(current) : !char.IsWhiteSpace(current));
        }

        public static string ReadWhile(this StringReader stringReader, Func<char, bool> condition)
        {
            var result = string.Empty;

            char current = '\u0000';
            while (current != '\uffff')
            {
                current = unchecked((char)stringReader.Peek());

                if (!condition(current) || current == '\uffff')
                {
                    break;
                }
                else
                {
                    result += unchecked((char)stringReader.Read());
                }
            }

            return result;
        }
    }
}
