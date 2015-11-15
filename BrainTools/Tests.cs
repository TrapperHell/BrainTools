using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace BrainTools
{
    public static class Tests
    {
        /*
         * Brainfuck Codes:
         *  Happy Birthday!
         *  ++++++++++[>+>++>+++>++++>+++++>++++++>+++++++>++++++++>+++++++++>++++++++++>+++++++++++>++++++++++++>+++++++++++++<<<<<<<<<<<<<-]>>>>>>>>--------.>>---.>>--------..>---------.<<<<<<<<<--------.>>>----.>>>>-----.>++.++.<-.----.<.>>>.<<<<<<<<<+.
        */

#if DEBUG
        public static void RunTests()
        {
            Console.Title = "BrainTools - Test Suite";

            Console.Write("Brainfuck Encoder Test [Slow]: ");
            string plainText = "Hello, this is a test :)", bf = string.Empty;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(plainText)))
            {
                bf = Brainfuck.Encode(ms);

                ms.Seek(0, SeekOrigin.Begin);
                ms.SetLength(0);
                new BrainfuckBasicInterpreter(Console.OpenStandardInput(), ms).Run(bf);

                ms.Seek(0, SeekOrigin.Begin);
                string plainText2 = Encoding.UTF8.GetString(ms.ToArray());

                if(plainText != plainText2)
                    throw new InvalidOperationException("Brainfuck [En/de]coder implementation is not correct.");
                else
                    Console.Write("Success{0}", Environment.NewLine);
            }

            // Insertion of Cell Initializer data
            Console.Write("Extended Type #2 - Cell Initializer Test: ");
            Brainfuck.Run<BrainfuckExtendedType2Interpreter>("[.>]@Hello World!");
            Console.WriteLine();

            Console.Write("Brainloller Test: ");
            bf = "+++++++[->+++++[->++>++>++>++<<<<]<]>+++++[->>+++>>+<<<<]>>>---<<[.>]";
            using (Bitmap bmp = Brainloller.Encode(bf, Color.Black))
            {
                if (!String.Equals(Brainloller.Decode(bmp), bf, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Brainloller implementation is not correct.");
                else
                    Console.Write("Success{0}", Environment.NewLine);
            }
        }
#endif
    }
}