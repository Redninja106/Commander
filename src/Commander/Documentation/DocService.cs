using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Commander.Documentation
{
    public static class DocService
    {
        public static int MaxLineLength = 100;

        [Command("Commander")]
        [Doc("Prints the documentation of the passed command.")]
        [DocArg("string", "command", "The name of the command to output the documentation of.")]
        public static void Doc(string command)
        {
            WriteDoc(Service.Output, command);
        }

        public static string GetDoc(string command)
        {
            IConsole c = new OutputStringConsole();

            WriteDoc(c, command);

            return c.ToString();
        }

        private static bool WriteDoc(IConsole console, string command)
        {
            // parse the command string. It would be overkill to link into the existing command parsing system.
            string serviceName = "";
            string commandName = "";

            if (command.Contains(":"))
            {
                var parts = command.Split(':');
                serviceName = parts[0];
                commandName = parts[1];
            }
            else
            {
                commandName = command ?? "";
            }

            var commands = Service.RegisteredCommands.FindAll(c => c.Signature.Name == commandName.ToLower() && (serviceName == "" ? true : c.Signature.ServiceName == serviceName.ToLower()));

            if (commands.Count < 1)
            {
                return Service.ReportError($"Unrecognized command: {command}");
            }

            if (commands.Count > 1)
            {
                // format an exception string
                string exStr = "Ambiguous reference to two or more commands:";

                foreach (var cmd in commands)
                {
                    exStr += "\n" + cmd.ToString();
                }

                return Service.ReportError(exStr);
            }

            var commandRef = commands.First();

            var doc = commandRef.MethodInfo.GetCustomAttribute<DocAttribute>();
            var parameters = commandRef.MethodInfo.GetCustomAttributes<DocArgAttribute>();
            var opParameters = commandRef.MethodInfo.GetCustomAttributes<OpDocArgAttribute>();

            if (doc != null)
            {
                DocFormat.Instance.Print(console, commandRef);
            }
            else
            {
                return Service.ReportError("There is no documentation for this command");
            }

            return true;
        }

        private class DocFormat
        {
            public static readonly DocFormat Instance = new DocFormat();

            private string line;
            private int positionOnLine;
            private IConsole o;
            private Color currentColor;

            private DocFormat() { }

            public void Print(IConsole o, Command command)
            {
                var oldCol = o.Color;
                this.o = o;
                currentColor = o.Color;
                positionOnLine = 0;
                line = "";

                var doc = command.MethodInfo.GetCustomAttribute<DocAttribute>();
                var parameters = command.MethodInfo.GetCustomAttributes<DocArgAttribute>();
                var opParameters = command.MethodInfo.GetCustomAttributes<OpDocArgAttribute>();

                TitleBar(command.Signature.ToString());
                NewLine();
                NewLine();
                SetColor(Service.Style.Lowlight);
                Put("Description: ");

                SetColor(Service.Style.Text);
                Put(doc.Doc);

                SetColor(Service.Style.Lowlight);
                NewLine();
                NewLine();
                Put("Parameters:");
                NewLine();

                foreach (var p in parameters)
                {
                    SetColor(Service.Style.Lowlight);
                    Tab();
                    Put($"\"");

                    SetColor(Service.Style.Highlight);
                    Put(p.Name);

                    SetColor(Service.Style.Lowlight);
                    Put($"\" (");
                    
                    SetColor(Service.Style.Text);
                    Put(p.Type);

                    SetColor(Service.Style.Lowlight);
                    Put($"): ");

                    SetColor(Service.Style.Text);

                    Put(p.Doc);
                    NewLine();
                }

                foreach (var op in opParameters)
                {
                    SetColor(Service.Style.Lowlight);
                    Tab();
                    Put($"\"");

                    SetColor(Service.Style.Highlight);
                    Put(op.Name);

                    SetColor(Service.Style.Lowlight);
                    Put($"\" (");

                    SetColor(Service.Style.Text);
                    Put(op.Type);

                    SetColor(Service.Style.Lowlight);
                    Put($"): [");

                    SetColor(Service.Style.Highlight);
                    Put($"Optional");

                    SetColor(Service.Style.Lowlight);
                    Put($". Default: \"");

                    SetColor(Service.Style.Highlight);
                    Put(op.DefaultValue);

                    SetColor(Service.Style.Lowlight);
                    Put($"\"] ");

                    SetColor(Service.Style.Text);

                    Put(op.Doc);
                    NewLine();
                }

                if(opParameters.Count() + parameters.Count() == 0)
                {
                    Tab();
                    Put("None.");
                    NewLine();
                }

                NewLine();

                SetColor(Service.Style.Lowlight);
                Put("Usage: ");

                SetColor(Service.Style.Highlight);
                Put(command.Signature.ToString());

                foreach (var p in parameters)
                {
                    SetColor(Service.Style.Highlight);
                    Put(' ' + p.Name);
                }

                foreach (var op in opParameters)
                {
                    SetColor(Service.Style.Lowlight);
                    Put(" [");
                    SetColor(Service.Style.Highlight);
                    Put(op.Name);
                    SetColor(Service.Style.Lowlight);
                    Put("]");
                }

                NewLine();
                BottomBar(command.Signature.ToString());

                SetColor(oldCol);
            }

            private void Tab()
            {
                Put("     ");
                positionOnLine += 5;
            }

            private void Put(char c)
            {
                if (c == '\n')
                    positionOnLine = 0;

                line += c;
                positionOnLine++;

                if(positionOnLine > MaxLineLength)
                {
                    NewLine();
                    positionOnLine += 3;
                }
            }

            private void Put(string s)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    Put(s[i]);
                }
            }

            private void SetColor(Color color)
            {
                o.Write(line, currentColor);
                line = "";
                currentColor = color;
            }

            private void TitleBar(string title)
            {
                // make sure max length and title bar are both even or both odd
                if (MaxLineLength % 2 != title.Length % 2)
                {
                    MaxLineLength ^= 1; // flip the '1' bit 
                }

                int signCount = MaxLineLength - title.Length - 4;

                SetColor(Service.Style.Lowlight);
                Put('+');
                for (int i = 0; i < signCount / 2; i++)
                {
                    Put('=');
                }

                SetColor(Service.Style.Highlight);
                Put($" {title} ");

                SetColor(Service.Style.Lowlight);
                for (int i = 0; i < signCount / 2; i++)
                {
                    Put('=');
                }

                Put('+');
            }

            private void NewLine()
            {
                positionOnLine = 0;
                line += "\n";
                Tab();
            }

            private void BottomBar(string title)
            {
                Put('\n');
                SetColor(Service.Style.Lowlight);
                int signCount = MaxLineLength - 2;

                Put('+');

                for (int i = 0; i < signCount; i++)
                {
                    Put('=');
                }

                line += "+\n";
            }
        }

        private class OutputStringConsole : IConsole
        {
            public Color Color { get; set; }

            string result;

            public void Write(object obj)
            {
                result += obj.ToString();
            }

            public override string ToString()
            {
                return result;
            }
        }
    }
}
