using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commander
{
    internal class InvocationBuilder
    {
        StringParser reader;
        List<CommandInvocation> invocations;
        CommandInvocation current;

        public InvocationBuilder()
        {

        }

        public List<CommandInvocation> GetInvocations(string reqString)
        {
            reader = new StringParser(reqString);
            invocations = new List<CommandInvocation>();
            current = CommandInvocation.Default;

            while (!reader.IsAtEnd)
            {
                reader.Next();
                if(!HandleChar())
                {
                    return null;
                }
            }

            if(!current.Equals(CommandInvocation.Default))
            {
                NewCommand();
            }

            return invocations;
        }

        void NewCommand()
        {
            invocations.Add(current);

            current = CommandInvocation.Default;
        }

        bool HandleChar()
        {
            if (current.Name == null)
            {
                reader.ReadWhile(' ');
                if (char.IsLetter(reader.Current) || reader.Current == '_')
                {
                    var s = reader.ReadWhile(() => char.IsLetterOrDigit(reader.Current) || reader.Current == '_');
                    if (reader.Current == ' ' || reader.Current == (char)0)
                    {
                        if (current.Service == null)
                        {
                            current.Service = "";
                        }
                        current.Name = s.ToLower();
                    }
                    else if (reader.Current == ':')
                    {
                        current.Service = s.ToLower();
                        reader.Next();
                        current.Name = reader.ReadWhile(() => char.IsLetterOrDigit(reader.Current) || reader.Current == '_');
                    }
                    else
                    {
                        return Service.ReportError("");
                    }
                }
                else if (reader.Current == (char)0)
                {
                    NewCommand();
                    return true;
                }
                else
                {
                    return Service.ReportError("");
                }
            }
            else
            {
                var s = reader.ReadUntil('|', '"', StringParser.End);

                current.Parameters.AddRange(s.Split());
                current.Parameters.RemoveAll(o => o.ToString() == "");

                if (reader.Current == '"')
                {
                    reader.Next();
                    current.Parameters.Add(reader.ReadUntil('"', StringParser.End));
                }
                else
                {
                    NewCommand();
                }
            }

            return true;
        }

        public static CommandSignature ParseSignature(string sig)
        {
            sig = sig.ToLower();

            if (sig.Contains(":"))
            {
                var parts = sig.Split(':');
                return new CommandSignature(parts[0], parts[1], null);
            }
            else 
            {
                return new CommandSignature("", sig, null);
            } 
        }
    }
}
