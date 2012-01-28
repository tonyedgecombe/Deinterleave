﻿using System;
using System.IO;
using System.IO.Packaging;

namespace Delinearise
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Syntax: Delinearise input.xps output.xps\n");
                return 1;
            }

            DelinearisePackage(args[0], args[1]);

            return 0;
        }

        private static void DelinearisePackage(string inputPath, string outputPath)
        {
            using (Package inpputPackage = Package.Open(inputPath),
                           outputPackage = Package.Open(outputPath, FileMode.Create))
            {
                CopyPackage(inpputPackage, outputPackage);
            }
        }

        private static void CopyPackage(Package inputPackage, Package outputPackage)
        {
            foreach (var inputPart in inputPackage.GetParts())
            {
                CreateNewPart(inputPart, outputPackage);
            }
        }

        private static void CreateNewPart(PackagePart inputPart, Package outputPackage)
        {
            var outputPart = outputPackage.CreatePart(inputPart.Uri, inputPart.ContentType, CompressionOption.Normal);
            if (outputPart == null)
            {
                throw new ApplicationException("Couldn't create new part");
            }

            CopyPart(inputPart, outputPart);
        }

        private static void CopyPart(PackagePart oldPart, PackagePart newPart)
        {
            using (Stream oldPartStream = oldPart.GetStream(),
                          newPartStream = newPart.GetStream(FileMode.OpenOrCreate))
            {
                oldPartStream.CopyTo(newPartStream);
            }
        }
    }
}
