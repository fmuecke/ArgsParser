namespace fmdev.ArgsParser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class ArgsParser
    {
        private string optionPrefix = "-";

        public ArgsParser(List<Command> commands)
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            Header = $"{fileVersion.OriginalFilename} {fileVersion.ProductVersion} - {fileVersion.Comments}\n{fileVersion.LegalCopyright}";
            Commands = commands;
        }

        public Command Result { get; private set; }

        private string Header { get; }

        private List<Command> Commands { get; }

        public bool Parse(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return false;
            }

            var command = Commands.FirstOrDefault(c => string.Equals(c.GetName(), args[0], StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(command?.GetName()))
            {
                PrintUsage();
                return false;
            }

            try
            {
                if (!ParseCommandArgs(command, new ArraySegment<string>(args, 1, args.Length - 1).ToList()))
                {
                    PrintUsage(command.GetName());
                    return false;
                }
            }
            catch (ArgumentException e)
            {
                Console.Error.WriteLine("ERROR: " + e.Message + "\n");
                return false;
            }

            Result = command;
            return true;
        }

        public void PrintUsage()
        {
            Console.WriteLine(Header);
            Console.WriteLine("\nAvailable commands are:\n");

            var paddingLen = Commands.Max(c => c.GetName().Length) + 4;

            foreach (var c in Commands)
            {
                Console.WriteLine(string.Format("  {0}{1}\n", c.GetName().PadRight(paddingLen), c.GetDescription()));
            }
        }

        public void PrintUsage(string commandName)
        {
            Console.WriteLine(Header);
            Console.WriteLine("\nThe options for the '{0}' command are:\n", commandName);

            Command command;
            try
            {
                command = Commands.First(c => string.Equals(c.GetName(), commandName, StringComparison.OrdinalIgnoreCase));
            }
            catch (InvalidOperationException)
            {
                PrintUsage();
                return;
            }
            catch (ArgumentNullException)
            {
                PrintUsage();
                return;
            }

            var options = command.GetOptions();
            var paddingLen = options.Max(o => o.Name.Length);

            foreach (var o in options)
            {
                var a = o.GetCustomAttribute<CommandArgAttribute>();

                Console.Write($"  {optionPrefix}{o.Name.PadRight(paddingLen)}    {a.HelpText}");
                if (o.PropertyType == typeof(bool))
                {
                    Console.Write(" (flag)");
                }

                Console.WriteLine(a.IsRequired ? " (required)\n" : "\n");
            }
        }

        private bool ParseCommandArgs(Command command, List<string> args)
        {
            foreach (var o in command.GetOptions())
            {
                var att = o.GetCustomAttribute<CommandArgAttribute>();
                var pos = args.FindIndex(a => string.Equals(a, optionPrefix + o.Name, StringComparison.OrdinalIgnoreCase));
                if (pos == -1)
                {
                    if (att.IsRequired)
                    {
                        PrintUsage(command.GetName());
                        throw new ArgumentException($"required option '{o.Name}' is missing");
                    }

                    if (o.PropertyType == typeof(bool))
                    {
                        o.SetValue(command, false);
                    }

                    continue;
                }

                args.RemoveAt(pos);

                if (o.PropertyType == typeof(bool))
                {
                    o.SetValue(command, true);
                }
                else
                {
                    if (pos >= args.Count)
                    {
                        PrintUsage(command.GetName());
                        throw new ArgumentException($"option '{o.Name}' misses a parameter");
                    }

                    o.SetValue(command, args.ElementAt(pos));
                    args.RemoveAt(pos);
                }
            }

            return true;
        }
    }
}