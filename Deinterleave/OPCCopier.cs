using System;
using System.Globalization;
using System.IO;
using System.IO.Packaging;

namespace Deinterleave
{
    public class OPCCopier
    {
        public static void CopyPackage(string inputPath, string outputPath)
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
            var contentType = IsObfuscatedFont(inputPart) ? "application/vnd.ms-opentype" : inputPart.ContentType;

            var outputPart = outputPackage.CreatePart(inputPart.Uri, contentType, CompressionOption.Normal);
            if (outputPart == null)
            {
                throw new ApplicationException("Couldn't create new part");
            }

            if (IsObfuscatedFont(inputPart))
            {
                CopyObfuscatedFontPart(inputPart, outputPart);
            }
            else
            {
                CopyPart(inputPart, outputPart);
            }
        }

        private static void CopyPart(PackagePart oldPart, PackagePart newPart)
        {
            using (Stream oldPartStream = oldPart.GetStream(),
                          newPartStream = newPart.GetStream(FileMode.OpenOrCreate))
            {
                oldPartStream.CopyTo(newPartStream);
            }
        }

        private static void CopyObfuscatedFontPart(PackagePart oldPart, PackagePart newPart)
        {
            var guidArray = GetGUID(oldPart.Uri);

            using (Stream oldPartStream = oldPart.GetStream(),
                          newPartStream = newPart.GetStream(FileMode.OpenOrCreate),
                          memoryStream = new MemoryStream())
            {
                oldPartStream.CopyTo(memoryStream);

                for (int i = 0; i < 32; i++)
                {
                    (memoryStream as MemoryStream).GetBuffer()[i] ^= guidArray[15 - (i % 16)];
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(newPartStream);
            }
        }

        private static byte[] GetGUID(Uri uri)
        {
            var result = new byte[16];

            string name = uri.ToString();
            int pos = name.Length;
            while (--pos > 0)
            {
                if (name[pos] == '.')
                {
                    name = name.Remove(pos);
                    break;
                }
            }

            name = name.Replace("-", "");
            if (name.Length < 32)
            {
                throw new ArgumentException("GUID must be 32 characters to deobfuscate a font");
            }

            name = name.Substring(name.Length - 32);

            for (int i = 0; i < 16; i++)
            {
                result[i] = Byte.Parse(name.Substring(i*2, 2), NumberStyles.HexNumber);
            }

            return result;
        }

        private static bool IsObfuscatedFont(PackagePart part)
        {
            return part.ContentType == "application/vnd.ms-package.obfuscated-opentype";
        }
    }
}