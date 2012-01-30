using System;

namespace Deinterleave
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Syntax: Deinterleave input.xps output.xps\n");
                return 1;
            }

            OPCCopier.CopyPackage(args[0], args[1]);

            return 0;
        }
    }
}
