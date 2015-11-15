using BrainTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

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
                Brainfuck.Run(bfCode, msIn, msOut);

                return Encoding.ASCII.GetString(msOut.ToArray());
            }
        }

        [TestMethod]
        public void BasicBrainfuckTest()
        {
            Assert.AreEqual("Hello World!", RunMemoryBFTest(">+++++++++[<++++++++>-]<.>+++++++[<++++>-]<+.+++++++..+++.>>>++++++++[<++++>-]<.>>>++++++++++[<+++++++++>-]<---.<<<<.+++.------.--------.>>+."));
        }

        [TestMethod]
        public void CellWrappingTest()
        {
            // Revision of execution tape. Supports negative cell values.
            Assert.AreEqual("H", RunMemoryBFTest("-[------->+<]>-."));
        }

        [TestMethod]
        public void LoopSkippingTest()
        {
            // The second loop should be skipped entirely since the cell value is zero.
            Assert.AreEqual("F", RunMemoryBFTest("++++++++++[->+++++++<][>++++<]>."));
        }

        [TestMethod]
        public void UnmatchedLoopTokenTest()
        {
            try
            {
                Brainfuck.Run("[++");
                Brainfuck.Run("++]");
            }
            catch (IndexOutOfRangeException)
            {
                // Test fails
                Assert.Fail();
            }
        }
    }
}