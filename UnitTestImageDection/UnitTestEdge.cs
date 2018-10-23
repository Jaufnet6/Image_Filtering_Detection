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

        //Testing the black result when sending a white images
        [TestMethod]
        public void TestBlackWhenSentWhiteImage()
        {
            //Create white Bitmap
            Bitmap whiteBPM = new Bitmap(100, 100);
            using (Graphics gfx = Graphics.FromImage(whiteBPM))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                gfx.FillRectangle(brush, 0, 0, 100, 100);
            }

            //Create black Bitmap
            Bitmap blackBPM = new Bitmap(100, 100);
            using (Graphics gfx = Graphics.FromImage(blackBPM))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0)))
            {
                gfx.FillRectangle(brush, 0, 0, 100, 100);
            }

            //Send white Bitmap to laplacian
            Bitmap testResult = ExtBitmap.Laplacian3x3Filter(whiteBPM, false);

            //Compare same pixel that should have changed to black
            Assert.AreEqual(blackBPM.GetPixel(2,2), testResult.GetPixel(2,2));


        }

        [TestMethod]
        public void TestImageLaplacian3x3()
        {
            //send a photo to modify it
            Bitmap testedBitmap = ExtBitmap.Laplacian3x3Filter(originalBitmapTest, false);

            //Retrieve the already modified photo
            Bitmap testBitmap = null;
            string FileName = @"C:\Users\quent\Documents\GitHub\Image_Filtering_Detection\UnitTestImageDection\Laplacian3x3.png";
            
            StreamReader streamReader = new StreamReader(FileName);
            testBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();

            //comparison
            Assert.AreEqual(testBitmap.GetPixel(2,2), testedBitmap.GetPixel(2, 2));
        
        }


        [TestMethod]
        public void TestImageLaplacian5x5()
        {
            //send a photo to modify it
            Bitmap testedBitmap = ExtBitmap.Laplacian5x5Filter(originalBitmapTest, false);

            //Retrieve the already modified photo
            Bitmap testBitmap = null;
            string FileName = @"C:\Users\quent\Documents\GitHub\Image_Filtering_Detection\UnitTestImageDection\Laplacian5x5.png";
            
            StreamReader streamReader = new StreamReader(FileName);
            testBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();

            //comparison
            Assert.AreEqual(testBitmap.GetPixel(2, 2), testedBitmap.GetPixel(2, 2));

        }

        [TestMethod]
        public void TestImageLaplacianOfGaussian()
        {
            //send a photo to modify it
            Bitmap testedBitmap = ExtBitmap.LaplacianOfGaussianFilter(originalBitmapTest);

            //Retrieve the already modified photo
            Bitmap testBitmap = null;
            string FileName = @"C:\Users\quent\Documents\GitHub\Image_Filtering_Detection\UnitTestImageDection\LaplacianOfGaussian.png";
            
            StreamReader streamReader = new StreamReader(FileName);
            testBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();

            //comparison
            Assert.AreEqual(testBitmap.GetPixel(2, 2), testedBitmap.GetPixel(2, 2));

        }


        //Test when sending a bitmap with superior width that canvas and height
        [TestMethod]
        public void TestCopyToSquareCanvasWithWidthGreaterThatCanvasAndHeight()
        {
            //Initialize sizes
            float bpmHeight = 200f;
            float bpmWidth = 1000f;
            int canvasSize = 400;

            //Determine the ratio for expected results
            float ratio = bpmWidth / canvasSize;
            
            //Send bitmap with height, width and canvas to tested method
            Bitmap bpmAfterMethodCanvas = ExtBitmap.CopyToSquareCanvas (new Bitmap((int)bpmWidth, (int)bpmHeight), canvasSize);

            //All expected sizes
            float bpmWidthExpected = canvasSize;
            float bpmHeightExpected = bpmHeight / ratio;
            //Tested sizes
            float widthResult = (float) bpmAfterMethodCanvas.Width;
            float heigthResult = (float) bpmAfterMethodCanvas.Height;
            //Compare values
            Assert.AreEqual(bpmWidthExpected, widthResult);
            Assert.AreEqual(bpmHeightExpected, heigthResult);

        }

        //Test when sending a bitmap with superior height that canvas and width
        [TestMethod]
        public void TestCopyToSquareCanvasWithHeightGreaterThatCanvasAndWidth()
        {
            //Initialize sizes
            float bpmHeight = 400f;
            float bpmWidth = 200f;
            int canvasSize = 100;

            //Determine the ratio for expected results
            float ratio = bpmHeight / canvasSize;

            //Send bitmap with height, width and canvas to tested method
            Bitmap bpmAfterMethodCanvas = ExtBitmap.CopyToSquareCanvas(new Bitmap((int)bpmWidth, (int)bpmHeight), canvasSize);

            //All expected sizes
            float bpmWidthExpected = bpmWidth / ratio;
            float bpmHeightExpected = canvasSize;
            //Tested sizes
            float widthResult = (float)bpmAfterMethodCanvas.Width;
            float heigthResult = (float)bpmAfterMethodCanvas.Height;
            //Compare values
            Assert.AreEqual(bpmWidthExpected, widthResult);
            Assert.AreEqual(bpmHeightExpected, heigthResult);

        }

        //Test when sending a bitmap with hight and width same size as the canvas
        [TestMethod]
        public void TestCopyToSquareCanvasWithAllSameSize()
        {
            //Initialize sizes
            float bpmHeight = 200f;
            float bpmWidth = 200f;
            int canvasSize = 200;

            //Determine the ratio for expected results
            float ratio = bpmWidth / canvasSize;

            //Send bitmap with height, width and canvas to tested method
            Bitmap bpmAfterMethodCanvas = ExtBitmap.CopyToSquareCanvas(new Bitmap((int)bpmWidth, (int)bpmHeight), canvasSize);

            //All expected sizes
            float bpmWidthExpected = canvasSize;
            float bpmHeightExpected = bpmHeight / ratio;
            //Tested sizes
            float widthResult = (float)bpmAfterMethodCanvas.Width;
            float heigthResult = (float)bpmAfterMethodCanvas.Height;
            //Compare values
            Assert.AreEqual(bpmWidthExpected, widthResult);
            Assert.AreEqual(bpmHeightExpected, heigthResult);

        }

        //Test when sending a bitmap with canvas smaller that height and width (both same size)
        [TestMethod]
        public void TestCopyToSquareCanvasWithSamllerCanvas()
        {
            //Initialize sizes
            float bpmHeight = 200f;
            float bpmWidth = 200f;
            int canvasSize = 100;

            //Determine the ratio for expected results
            float ratio = bpmWidth / canvasSize;

            //Send bitmap with height, width and canvas to tested method
            Bitmap bpmAfterMethodCanvas = ExtBitmap.CopyToSquareCanvas(new Bitmap((int)bpmWidth, (int)bpmHeight), canvasSize);

            //All expected sizes
            float bpmWidthExpected = canvasSize;
            float bpmHeightExpected = bpmHeight / ratio;
            //Tested sizes
            float widthResult = (float)bpmAfterMethodCanvas.Width;
            float heigthResult = (float)bpmAfterMethodCanvas.Height;
            //Compare values
            Assert.AreEqual(bpmWidthExpected, widthResult);
            Assert.AreEqual(bpmHeightExpected, heigthResult);

        }




    }
}
