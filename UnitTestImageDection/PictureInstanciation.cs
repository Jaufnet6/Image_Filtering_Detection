using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestImageDection
{
    public class PictureInstanciation
    {

        private Bitmap testBitmap = null;
        private string FileName = @"C:\Users\jaufray\source\repos\Image_Filtering_Detection\test.png";
        

        public Bitmap InitializeTestPic()
        {
            StreamReader streamReader = new StreamReader(FileName);
            testBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();
            return testBitmap;
        }

    }
}
