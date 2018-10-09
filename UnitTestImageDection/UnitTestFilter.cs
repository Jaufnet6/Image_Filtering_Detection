using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using ImageEdgeDetection;

namespace UnitTestImageDection
{
    [TestClass]
    public class UnitTestFilter
    {

        private static PictureInstanciation pi = new PictureInstanciation();
        private static Bitmap originalBitmapTest = pi.InitializeTestPic();

        [TestMethod]
        public void TestIfImageWidthIsSmallerThan4()
        {
            Bitmap testBitmap = new Bitmap(3, originalBitmapTest.Height);
            Bitmap receivedAfterTest = ImageFilter.RainbowFilter(testBitmap);
            Assert.AreEqual(testBitmap, receivedAfterTest);
        }

        [TestMethod]
        public void TestColorOf1Band()
        {
            Bitmap testBitmap = new Bitmap(originalBitmapTest.Width, originalBitmapTest.Height);
            //Color the full picture with the first band color
            for (int i = 0; i < originalBitmapTest.Width; i++)
            {
                for (int x = 0; x < originalBitmapTest.Height; x++)
                {
                    testBitmap.SetPixel(i, x, Color.FromArgb(originalBitmapTest.GetPixel(i, x).R / 5, originalBitmapTest.GetPixel(i, x).G, originalBitmapTest.GetPixel(i, x).B));
                }
            }
            //send the original non-touched image in the filter
            Bitmap receivedAfterTest = ImageFilter.RainbowFilter(originalBitmapTest);
            //Determin a pixel to compare
            int raz1 = testBitmap.Width / 4;
            int pixelToCheck = raz1 / 2;
            //Compare pixel
            Assert.AreEqual(testBitmap.GetPixel(pixelToCheck, 1), receivedAfterTest.GetPixel(pixelToCheck,1));
        }

        [TestMethod]
        public void TestColorOf2Band()
        {
            Bitmap testBitmap = new Bitmap(originalBitmapTest.Width, originalBitmapTest.Height);
            //Color the full picture with the second band color
            for (int i = 0; i < originalBitmapTest.Width; i++)
            {
                for (int x = 0; x < originalBitmapTest.Height; x++)
                {
                    testBitmap.SetPixel(i, x, Color.FromArgb(originalBitmapTest.GetPixel(i, x).R, originalBitmapTest.GetPixel(i, x).G / 5, originalBitmapTest.GetPixel(i, x).B));
                }
            }
            //send the original non-touched image in the filter
            Bitmap receivedAfterTest = ImageFilter.RainbowFilter(originalBitmapTest);
            //Determine a pixel to compare
            int raz1 = testBitmap.Width / 4;
            int pixelToCheck = raz1 * 2 - 1;
            //Compare pixel
            Assert.AreEqual(testBitmap.GetPixel(pixelToCheck, 1), receivedAfterTest.GetPixel(pixelToCheck, 1));
        }

        [TestMethod]
        public void TestColorOf3Band()
        {
            Bitmap testBitmap = new Bitmap(originalBitmapTest.Width, originalBitmapTest.Height);
            //Color the full picture with the third band color
            for (int i = 0; i < originalBitmapTest.Width; i++)
            {
                for (int x = 0; x < originalBitmapTest.Height; x++)
                {
                    testBitmap.SetPixel(i, x, Color.FromArgb(originalBitmapTest.GetPixel(i, x).R, originalBitmapTest.GetPixel(i, x).G, originalBitmapTest.GetPixel(i, x).B / 5));
                }
            }
            //send the original non-touched image in the filter
            Bitmap receivedAfterTest = ImageFilter.RainbowFilter(originalBitmapTest);
            //Determine a pixel to compare
            int raz1 = testBitmap.Width / 4;
            int pixelToCheck = raz1 * 3 - 1;
            //Compare pixel
            Assert.AreEqual(testBitmap.GetPixel(pixelToCheck, 1), receivedAfterTest.GetPixel(pixelToCheck, 1));
        }

        [TestMethod]
        public void TestColorOf4Band()
        {
            Bitmap testBitmap = new Bitmap(originalBitmapTest.Width, originalBitmapTest.Height);
            //Color the full picture with the fourth band color
            for (int i = 0; i < originalBitmapTest.Width; i++)
            {
                for (int x = 0; x < originalBitmapTest.Height; x++)
                {
                    testBitmap.SetPixel(i, x, Color.FromArgb(originalBitmapTest.GetPixel(i, x).R / 5, originalBitmapTest.GetPixel(i, x).G, originalBitmapTest.GetPixel(i, x).B / 5));
                }
            }
            //send the original non-touched image in the filter
            Bitmap receivedAfterTest = ImageFilter.RainbowFilter(originalBitmapTest);
            //Determine a pixel to compare
            int raz1 = testBitmap.Width / 4;
            int pixelToCheck = raz1 * 4 - 1;
            //Compare pixel
            Assert.AreEqual(testBitmap.GetPixel(pixelToCheck, 1), receivedAfterTest.GetPixel(pixelToCheck, 1));
        }


        [TestMethod]
        public void TestColorIfWidthNotDisibleBy4()
        {
            //Create the bitmap that will be sent to the filter
            Bitmap testBitmap = new Bitmap(99, originalBitmapTest.Height);
            //Create the bitmap that will be colored with the supplement color
            Bitmap unifiedTestBitmapColor = new Bitmap(99, originalBitmapTest.Height);

            //Color the full picture with the default supplemant color for external pixels
            for (int i = 0; i < testBitmap.Width; i++)
            {
                for (int x = 0; x < testBitmap.Height; x++)
                {
                    unifiedTestBitmapColor.SetPixel(i, x, Color.FromArgb(testBitmap.GetPixel(i, x).R / 5, testBitmap.GetPixel(i, x).G / 5, testBitmap.GetPixel(i, x).B / 5));
                }
            }
            //send the testBitmap to the filter
            Bitmap receivedAfterTest = ImageFilter.RainbowFilter(testBitmap);
            //Determine the pixel to compare
            int pixelToCheck = testBitmap.Width - 1;
            //Compare pixel
            Assert.AreEqual(unifiedTestBitmapColor.GetPixel(pixelToCheck, 1), receivedAfterTest.GetPixel(pixelToCheck, 1));
        }

    }
}
