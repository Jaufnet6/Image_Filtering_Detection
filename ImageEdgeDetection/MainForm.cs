/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ImageEdgeDetection
{
    public partial class MainForm : Form
    {
        private Bitmap originalBitmap = null;
        private Bitmap previewBitmap = null;
        private Bitmap resultBitmap = null;
        private bool coloredFilterApplied = false;

        public MainForm()
        {
            InitializeComponent();

            cmbEdgeDetection.SelectedIndex = 0;
        }

        private void btnOpenOriginal_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file.";
            ofd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
            ofd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);
                originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                streamReader.Close();

                previewBitmap = originalBitmap.CopyToSquareCanvas(picPreview.Width);
                picPreview.Image = previewBitmap;

                ApplyFilter(true);
            }
        }

        private void btnSaveNewImage_Click(object sender, EventArgs e)
        {
            ApplyFilter(false);

            if (resultBitmap != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Specify a file name and file path";
                sfd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                sfd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileExtension = Path.GetExtension(sfd.FileName).ToUpper();
                    ImageFormat imgFormat = ImageFormat.Png;

                    if (fileExtension == "BMP")
                    {
                        imgFormat = ImageFormat.Bmp;
                    }
                    else if (fileExtension == "JPG")
                    {
                        imgFormat = ImageFormat.Jpeg;
                    }

                    StreamWriter streamWriter = new StreamWriter(sfd.FileName, false);
                    resultBitmap.Save(streamWriter.BaseStream, imgFormat);
                    streamWriter.Flush();
                    streamWriter.Close();

                    resultBitmap = null;
                }
            }
        }

        private void ApplyFilter(bool preview)
        {
            if (previewBitmap == null || cmbEdgeDetection.SelectedIndex == -1)
            {
                return;
            }

            Bitmap selectedSource = null;
            Bitmap bitmapResult = null;

            if (preview == true)
            {
                selectedSource = previewBitmap;
            }
            else
            {
                selectedSource = previewBitmap;//originalBitmap;
            }

            //determine the selected filter
            if (selectedSource != null)
            {
                switch (cmbEdgeDetection.SelectedItem.ToString())
                {
                    case "Laplacian 3x3":
                        bitmapResult = selectedSource.Laplacian3x3Filter(false); ;
                        break;
                    case "Laplacian 3x3 Grayscale":
                        bitmapResult = selectedSource.Laplacian3x3Filter(true);
                        break;
                    case "Laplacian 5x5":
                        bitmapResult = selectedSource.Laplacian5x5Filter(false);
                        break;
                    case "Laplacian 5x5 Grayscale":
                        bitmapResult = selectedSource.Laplacian5x5Filter(true);
                        break;
                    case "Laplacian of Gaussian":
                        bitmapResult = selectedSource.LaplacianOfGaussianFilter();
                        break;
                    case "Laplacian 3x3 of Gaussian 3x3":
                        bitmapResult = selectedSource.Laplacian3x3OfGaussian3x3Filter();
                        break;
                    case "Laplacian 3x3 of Gaussian 5x5 - 1":
                        bitmapResult = selectedSource.Laplacian3x3OfGaussian5x5Filter1();
                        break;
                    case "Laplacian 3x3 of Gaussian 5x5 - 2":
                        bitmapResult = selectedSource.Laplacian3x3OfGaussian5x5Filter2();
                        break;
                    case "Laplacian 5x5 of Gaussian 3x3":
                        bitmapResult = selectedSource.Laplacian5x5OfGaussian3x3Filter();
                        break;
                    case "Laplacian 5x5 of Gaussian 5x5 - 1":
                        bitmapResult = selectedSource.Laplacian5x5OfGaussian5x5Filter1();
                        break;
                    case "Laplacian 5x5 of Gaussian 5x5 - 2":
                        bitmapResult = selectedSource.Laplacian5x5OfGaussian5x5Filter2();
                        break;
                    case "Sobel 3x3":
                        bitmapResult = selectedSource.Sobel3x3Filter(false);
                        break;
                    case "Sobel 3x3 Grayscale":
                        bitmapResult = selectedSource.Sobel3x3Filter();
                        break;
                    case "Prewitt":
                        bitmapResult = selectedSource.PrewittFilter(false);
                        break;
                    case "Prewitt Grayscale":
                        bitmapResult = selectedSource.PrewittFilter();
                        break;
                    case "Kirsch":
                        bitmapResult = selectedSource.KirschFilter(false);
                        break;
                    case "Kirsch Grayscale":
                        bitmapResult = selectedSource.KirschFilter();
                        break;
                    default:
                        bitmapResult = selectedSource;
                        break;

                }             
            }

            if (bitmapResult != null)
            {
                if (preview == true)
                {
                    picPreview.Image = bitmapResult;
                }
                else
                {
                    resultBitmap = bitmapResult;
                }
            }
        }
        //Edge Detection Filter
        private void NeighbourCountValueChangedEventHandler(object sender, EventArgs e)
        {
            ApplyFilter(true);
        }
        //Rainbow Filter
        private void colorFilterButton_Click(object sender, EventArgs e)
        {
            if (coloredFilterApplied)
            {
                putImageBackToOriginal();
            }
            else
            {
                picPreview.Image = picPreview.Image;
                picPreview.Image = ImageFilter.RainbowFilter(new Bitmap(previewBitmap));
                previewBitmap = ImageFilter.RainbowFilter(new Bitmap(previewBitmap));
                coloredFilterApplied = true;

            }

        }
        //Crazy Filter button
        private void crazyFilterButton_Click(object sender, EventArgs e)
        {
            if (coloredFilterApplied)
            {
                putImageBackToOriginal();
            }
            else
            {
                picPreview.Image = picPreview.Image;
                picPreview.Image = ImageFilter.ApplyFilterSwapDivide(new Bitmap(previewBitmap), 1, 1, 2, 1);
                picPreview.Image = ImageFilter.ApplyFilterSwap(new Bitmap(previewBitmap));
                previewBitmap = ImageFilter.ApplyFilterSwap(new Bitmap(previewBitmap));
                coloredFilterApplied = true;
            }
                       
        }
        //load the original image back
        private void putImageBackToOriginal()
        {
            previewBitmap = originalBitmap.CopyToSquareCanvas(picPreview.Width);
            picPreview.Image = previewBitmap;
            coloredFilterApplied = false;
        }

    }
}
