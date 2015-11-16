using System;
using System.IO;
using System.Text;
using BrainTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BFTests
{
    [TestClass]
    public class BrainfuckTests
    {
        private string RunMemoryBFTest(string bfCode, string inputCode = null)
        {
            if (inputCode == null)
                inputCode = string.Empty;

            using (MemoryStream msOut = new MemoryStream())
            using (MemoryStream msIn = new MemoryStream(Encoding.ASCII.GetBytes(inputCode)))
            {
                var bf = new BrainfuckExtendedType1Interpreter(msIn, msOut);
                bf.Run(bfCode);

                return Encoding.ASCII.GetString(msOut.ToArray());
            }
        }

        [TestMethod, Description("Validates the results of running a valid piece of BF code.")]
        public void BasicBrainfuckTest()
        {
            Assert.AreEqual("Hello World!", RunMemoryBFTest(">+++++++++[<++++++++>-]<.>+++++++[<++++>-]<+.+++++++..+++.>>>++++++++[<++++>-]<.>>>++++++++++[<+++++++++>-]<---.<<<<.+++.------.--------.>>+."));
            Assert.AreEqual("Happy Birthday!", RunMemoryBFTest("++++++++++[>+>++>+++>++++>+++++>++++++>+++++++>++++++++>+++++++++>++++++++++>+++++++++++>++++++++++++>+++++++++++++<<<<<<<<<<<<<-]>>>>>>>>--------.>>---.>>--------..>---------.<<<<<<<<<--------.>>>----.>>>>-----.>++.++.<-.----.<.>>>.<<<<<<<<<+."));
        }

        [TestMethod, Description("Validates the results of running a BF code with cell-wrapping.")]
        public void CellWrappingTest()
        {
            Assert.AreEqual("H", RunMemoryBFTest("-[------->+<]>-."));
        }

        [TestMethod, Description("Validates that the loop-start operator jumps beyond the loop-end when cell value is zero.")]
        public void LoopSkippingTest()
        {
            // The second loop should be skipped entirely since the cell value is zero.
            Assert.AreEqual("F", RunMemoryBFTest("++++++++++[->+++++++<][>++++<]>."));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Invalid BF code provided.")]
        public void UnmatchedLoopTokenTest()
        {
            Brainfuck.Run("[++");
            Brainfuck.Run("++]");
            Brainfuck.Run("++[->++<]]");
        }
    }
}