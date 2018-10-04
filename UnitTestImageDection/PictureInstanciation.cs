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

        private  Bitmap testBitmap = null;
        private  string FileName = "test.png";
        private  StreamReader streamReader = new StreamReader(FileName);

        public  Bitmap InitializeTestPic()
        {
            testBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();
            return testBitmap;
        }

    }
}
