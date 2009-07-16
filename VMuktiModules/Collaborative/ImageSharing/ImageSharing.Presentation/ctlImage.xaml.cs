using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using VMuktiAPI;

namespace ImageSharing.Presentation
{
    /// <summary>
    /// Interaction logic for ctlImage.xaml
    /// </summary>
    public partial class ctlImage : UserControl
    {
        int tempImgTag = 0;
        
        public delegate void DelSelectedImage(int ImgTag);
        public event DelSelectedImage EntSelectedImage;


        public ctlImage(string ImagePath,int ImgTag)
        {
            try
            {
                InitializeComponent();

                picImage.Tag = ImgTag.ToString();
                tempImgTag = ImgTag;
                picImage.MouseLeftButtonDown += new MouseButtonEventHandler(picImage_MouseLeftButtonDown);
                picImage.MouseEnter += new MouseEventHandler(picImage_MouseEnter);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlImage", "ctlImage.xaml.cs");
            }
        }

        void picImage_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (EntSelectedImage != null)
                {
                    EntSelectedImage(tempImgTag);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "picImage_MouseEnter", "ctlImage.xaml.cs");
            }
        }

        void picImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (EntSelectedImage != null)
                {
                    EntSelectedImage(tempImgTag);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "picImage_MouseLeftButtonDown", "ctlImage.xaml.cs");
            }
        }

        public byte[] SetImage(string ImagePath)
        {
            try
            {
                Bitmap BM = new Bitmap(ImagePath);
                MemoryStream mms = new MemoryStream();
                BM.Save(mms, System.Drawing.Imaging.ImageFormat.Jpeg);
                mms.Position = 0;
                return mms.ToArray();
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetImage", "ctlImage.xaml.cs");
                return null;
            }
        }

        public void ShowImage(byte[] imgArry)
        {
            try
            {
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = new MemoryStream(imgArry);
                bmi.EndInit();

                picImage.Source = bmi;
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ShowImage", "ctlImage.xaml.cs");
            }
        }
    }
}
