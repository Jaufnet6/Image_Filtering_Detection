using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestImageDection
{
    [TestClass]
    public class UnitTestEdge
    {
        private Bitmap testBitmap = null;
        private static string FileName = "test.png";
        StreamReader streamReader = new StreamReader(FileName);

        public void InitializeTestPic()
        {
            testBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();
        }
       

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
