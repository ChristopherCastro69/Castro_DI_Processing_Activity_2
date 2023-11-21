using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using DIP_Activity_2;
using AForge.Imaging.Filters;



namespace DIP_Activity_2
{
    public partial class Form1 : Form
    {
        Bitmap imageB;
        Bitmap imageA;
        Bitmap resultImage;
        Bitmap processed;
        Bitmap img;

        FilterInfoCollection devicesList;
        VideoCaptureDevice currentDevice;

        bool basicCopy = false;
        bool invertFilter = false;
        bool greyScaleFilter = false;
        bool sepiaFilter = false;
        bool histogram = false;

        public Form1()
        {
            InitializeComponent();
           pictureboxA.SizeMode = PictureBoxSizeMode.Zoom;
           pictureboxB.SizeMode = PictureBoxSizeMode.Zoom;
           pictureBoxC.SizeMode = PictureBoxSizeMode.Zoom;
           
        }



        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureboxA.Image = processed;
            if (imageB != null) // If an image is loaded
            {
                // Apply basic copy to the loaded image
                Features.ApplyBasicCopy(imageB, pictureboxA);
            }
            else if(currentDevice != null)
            {
                basicCopy = true;
                invertFilter = false;
                greyScaleFilter = false;
                sepiaFilter = false;
                histogram = false;
            }

        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageB != null) // If an image is loaded
            {
                // Apply grayscale conversion to the loaded image
                processed = Features.ConvertToGrayscale(imageB);
                pictureboxA.Image = processed; // Set the processed image to the PictureBox
            }

            else if (currentDevice != null)
            {
                greyScaleFilter = true;
                basicCopy = false;
                invertFilter = false;
                sepiaFilter = false;
                histogram = false;
            }
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (imageB != null) // If an image is loaded
            {
                // Apply color inversion to the loaded image
                processed = Features.InvertColors(imageB);
                pictureboxA.Image = processed;
            }
            else if (currentDevice != null)
            {
                invertFilter = true;
                basicCopy = false;
                greyScaleFilter = false;
                sepiaFilter = false;
                histogram = false;
            }

        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageB != null)
            {
                // Calculate histogram for the loaded image
                processed = Features.CalculateHistogram(imageB);
                pictureboxA.Image = processed;

            }

            else if (currentDevice != null) {
                histogram = true;
                invertFilter = false;
                basicCopy = false;
                greyScaleFilter = false;
                sepiaFilter = false;
            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageB != null) // If an image is loaded
            {
                // Apply sepia filter to the loaded image
                processed = Features.ApplySepiaFilter(imageB);
                pictureboxA.Image = processed;
            }
            else if (currentDevice != null)
            {
                sepiaFilter = true;
                basicCopy = false;
                invertFilter = false;
                greyScaleFilter = false;
            }
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureboxB.Image = imageB;
           
           

        }
        private void btnLoadBackground_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureboxA.Image = imageA;

           

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);

        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureboxA.Image = imageA;
        }


        private void btnSubtract_Click(object sender, EventArgs e)
        {
            try
            {
                if (imageA != null && imageB != null)
                {
                    // Subtract images if they are of the same size
                    processed = Features.SubtractImages(imageA, imageB);
                    pictureBoxC.Image = processed;
                }
                else
                {
                    MessageBox.Show("Please load both images.");
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap Image|*.bmp|JPEG Image|*.jpg;*.jpeg|PNG Image|*.png|GIF Image|*.gif";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // Save the processed image from pictureBox2.Image
                if (pictureboxA.Image != null)
                {

                    processed.Save(filePath);
                    MessageBox.Show("Image saved successfully!");
                }
                else
                {
                    MessageBox.Show("No processed image to save.");
                }
            }
        }

        private void bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap Image|*.bmp|JPEG Image|*.jpg;*.jpeg|PNG Image|*.png|GIF Image|*.gif";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // Save the processed image from pictureBox2.Image
                if (pictureboxB.Image != null)
                {
                    processed.Save(filePath);
                    MessageBox.Show("Image saved successfully!");
                }
                else
                {
                    MessageBox.Show("No processed image to save.");
                }
            }
        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap Image|*.bmp|JPEG Image|*.jpg;*.jpeg|PNG Image|*.png|GIF Image|*.gif";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // Save the processed image from pictureBox2.Image
                if (pictureBoxC.Image != null)
                {
                    processed.Save(filePath);
                    MessageBox.Show("Image saved successfully!");
                }
                else
                {
                    MessageBox.Show("No processed image to save.");
                }
            }
        }

        
        private void findDevicesButton_Click(object sender, EventArgs e)
        {
            devicesComboBox.Items.Clear();
            devicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in devicesList)
            {
                devicesComboBox.Items.Add(device.Name);
            }

            if (devicesComboBox.Items.Count > 0)
            {
                devicesComboBox.SelectedIndex = 0;
                currentDevice = new VideoCaptureDevice(devicesList[devicesComboBox.SelectedIndex].MonikerString);

                startCameraButton.Enabled = true;
            }
        }

        private void startCameraButton_Click(object sender, EventArgs e)
        {
            if (currentDevice != null)
            {
                currentDevice = new VideoCaptureDevice(devicesList[devicesComboBox.SelectedIndex].MonikerString);
                currentDevice.Start();
                currentDevice.NewFrame += currentDevice_NewFrame;
                startCameraButton.Enabled = false;
                stopCameraButton.Enabled = true;
              
                findDevicesButton.Enabled = false;
                devicesComboBox.Enabled = false;
            }
        }

        private void stopCameraButton_Click(object sender, EventArgs e)
        {
            if (currentDevice != null)
            {
                currentDevice.Stop();

                startCameraButton.Enabled = true;
                stopCameraButton.Enabled = false;

                findDevicesButton.Enabled = true;
                devicesComboBox.Enabled = true;

                pictureboxB.Image = null;
                pictureboxA.Image = null;
                pictureBoxC.Image = null;
            }
        }


        private void currentDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            pictureboxB.Image = frame;

            if (basicCopy)
            {
                

                pictureboxA.Image = frame;
            }
            else if (invertFilter)
            {
                // Create a separate copy of the frame for inversion
                Bitmap frameForInversion = (Bitmap)frame.Clone();

                // Invert the separate copy using AForge's filters
                AForge.Imaging.Filters.Invert filter = new AForge.Imaging.Filters.Invert();
                Bitmap invertedFrame = filter.Apply(frameForInversion);

                pictureboxA.Invoke((MethodInvoker)delegate {
                    pictureboxA.Image = invertedFrame; // Display inverted frame in PictureBox A
                });
            }
            else if (greyScaleFilter)
            {
                Bitmap frameForGreyscale = (Bitmap)frame.Clone();

                AForge.Imaging.Filters.Grayscale filter = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
                Bitmap greyFrame = filter.Apply(frameForGreyscale);

                pictureboxA.Invoke((MethodInvoker)delegate {
                    pictureboxA.Image = greyFrame; // Display grayscale frame in PictureBox A
                });
            }
            else if (sepiaFilter)
            {
                Bitmap frameForSepia = (Bitmap)frame.Clone();

                AForge.Imaging.Filters.Sepia filter = new AForge.Imaging.Filters.Sepia();
                Bitmap sepiaFrame = filter.Apply(frameForSepia);

                pictureboxA.Invoke((MethodInvoker)delegate {
                    pictureboxA.Image = sepiaFrame; // Display sepia-toned frame in PictureBox A
                });
            }

            else if (histogram)
            {
                // Display a message box with "To be continued..."
                pictureboxA.Invoke((MethodInvoker)delegate {
                    MessageBox.Show("To be continued...", "Continuation Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    histogram = false;
                });
            }

            else
            {
                sepiaFilter = false;
                basicCopy = false;
                invertFilter = false;
                greyScaleFilter = false;
                histogram = false;
            }
        }





        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopCameraButton_Click(sender, e);


        }

        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            if (pictureboxA.Image != null)
            {
                processed = new Bitmap(pictureboxA.ClientSize.Width, pictureboxA.ClientSize.Height);

                using (Graphics graphics = Graphics.FromImage(processed))
                {
                    graphics.CopyFromScreen(pictureboxA.PointToScreen(Point.Empty), Point.Empty, pictureboxA.ClientSize);
                }

                // Set the screenshot as the image for pictureBoxC
                pictureBoxC.Image = processed;
              
            }

            else
            {
                MessageBox.Show("No image to capture.");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            pictureboxA.Image = null;
            pictureboxB.Image = null;
            pictureBoxC.Image = null;

            stopCameraButton_Click(sender, e);
        }
    }
}
