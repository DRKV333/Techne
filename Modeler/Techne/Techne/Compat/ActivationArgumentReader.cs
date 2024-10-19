using System;

namespace Techne.Compat
{
    internal abstract class ActivationArgumentReader
    {
        public static ActivationArgumentReader Current { get; } = new CommandlineActivationArgumentReader();

        public abstract string StartupFileName { get; }
    }

    internal class CommandlineActivationArgumentReader : ActivationArgumentReader
    {
        public override string StartupFileName
        {
            get
            {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length >= 2)
                    return args[1];
                else
                    return null;
            }
        }
    }
}
