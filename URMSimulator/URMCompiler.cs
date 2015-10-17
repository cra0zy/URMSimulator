using System;
using System.Collections.Generic;

namespace URMSimulator
{
    public class URMCommand
    {
        public char type;
        public int[] args;

        public static URMCommand Parse(string data, out string errormessage)
        {
            if (!(new List<char>{ 'S', 'Z', 'T', 'J' }).Contains(data[0]))
            {
                errormessage = "Unknown Command: '" + data[0] + "'";
                return null;
            }
            else if (data.Length < 2 || data[1] != '(')
            {
                errormessage = "Missing: '('";
                return null;
            }
            else if (data[data.Length - 1] != ')')
            {
                errormessage = "Missing: ')'";
                return null;
            }
            else if (data.Length == 3)
            {
                errormessage = "Missing arguments";
                return null;
            }

            var arglen = 1;
            switch (data[0])
            {
                case 'T':
                    arglen = 2;
                    break;
                case 'J':
                    arglen = 3;
                    break;
            }

            var ret = new URMCommand();
            ret.type = data[0];

            var args = data.Substring(2, data.Length - 3).Split(',');

            if (args.Length != arglen)
            {
                errormessage = "Incorrect argument count, command '" + ret.type + "' requiers " + arglen + " arguments";
                return null;
            }

            ret.args = new int[arglen];
            for(int i = 0;i < arglen;i++)
            {
                try
                {
                    ret.args[i] = int.Parse(args[i]);

                    if(ret.args[i] <= 0)
                    {
                        errormessage = "Argument value has to be higher then 0 at position " + (i + 1);
                        return null;
                    }
                }
                catch
                {
                    errormessage = "Incorrect argument at position: " + (i + 1);
                    return null;
                }
            }

            errormessage = "";
            return ret;
        }
    }

    public static class URMCompiler
    {
        public static List<URMCommand> urmcommands = new List<URMCommand>();

        public static bool Compile(string[] lines, out string errormessage)
        {
            urmcommands.Clear();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Split(new [] { "//" }, StringSplitOptions.None)[0];

                if (!string.IsNullOrWhiteSpace(line) && !string.IsNullOrEmpty(line))
                {
                    string tmperrormessage;
                    var urmcommand = URMCommand.Parse(line.Replace(" ", "").ToUpper(), out tmperrormessage);

                    if (urmcommand == null)
                    {
                        errormessage = "Line " + (i + 1) + ": " + tmperrormessage;
                        return false;
                    }

                    urmcommands.Add(urmcommand);
                }
                else
                    urmcommands.Add(new URMCommand());
            }

            errormessage = "";
            return true;
        }
    }
}

