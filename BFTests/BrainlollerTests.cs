using System.Drawing;
using BrainTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BFTests
{
    [TestClass]
    public class BrainlollerTests
    {
        [TestMethod, Description("Validates the results of running a valid piece of BL code.")]
        public void BasicBrainlollerTest()
        {
            var bf = "+++++++[->+++++[->++>++>++>++<<<<]<]>+++++[->>+++>>+<<<<]>>>---<<[.>]";
            using (Bitmap bmp = Brainloller.Encode(bf, 6, Color.Black))
            {
                Assert.AreEqual(Brainloller.Decode(bmp), bf);
            }
        }
    }
}
