using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageEdgeDetection;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace UnitTestImageDection
{
    [TestClass]
    public class UnitTestEdge
    {

        private static PictureInstanciation pi = new PictureInstanciation();
        private static Bitmap originalBitmapTest = pi.InitializeTestPic();
       
        //Colors should all be = 0
        [TestMethod]
        public void TestImageWidthInforiorToMatrixShouldNotEnterForLoopsModifyingTheImage()
        {
            //Bitmap for testing
            Bitmap testBitmap = new Bitmap(1, 1);
            //Bitmap sent to Edge Filter
            Bitmap receivedAfterTest = ExtBitmap.Laplacian3x3Filter(testBitmap, false);

            //Lock bytes of image for creating a byter array buffer
            BitmapData sourceData = testBitmap.LockBits(new Rectangle(0, 0,
                                     testBitmap.Width, testBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            //Create 2 byte arrays with full image dimensions (height x width (number of byter for first pixel line)
            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            //Copy byte of image to byte array
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            //Unlock bytes image
            testBitmap.UnlockBits(sourceData);

            //Craete the testing result Bitmap
            Bitmap resultTestBitmap = new Bitmap(testBitmap.Width, testBitmap.Height);


            //Lock byte for copying bytes
            BitmapData resultData = resultTestBitmap.LockBits(new Rectangle(0, 0,
                                    resultTestBitmap.Width, resultTestBitmap.Height),
                                                     ImageLockMode.WriteOnly,
                                                PixelFormat.Format32bppArgb);

            //Copy byte array to data
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            //Unlock bits for bitmap
            resultTestBitmap.UnlockBits(resultData);

            //Compare single pixel
            Assert.AreEqual(resultTestBitmap.GetPixel(0, 0), receivedAfterTest.GetPixel(0, 0));


        }


    }
}
