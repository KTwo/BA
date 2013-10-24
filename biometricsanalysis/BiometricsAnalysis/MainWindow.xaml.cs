using BiometricsAnalysis.Tables;
using BiometricsAnalysis.Frames;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using System.Windows.Controls.Ribbon;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using BiometricsAnalysis.Signature;

namespace BiometricsAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        BCM bcm = new BCM();

        [DllImport("Kernel32")]
        public static extern void AllocConsole();
        [DllImport("Kernel32")]
        public static extern void FreeConsole();
        private void openConsole()
        {
            AllocConsole();


        }

        SignatureModule sgn = new SignatureModule();

        private Log _log;
        private Database_Utils _db_util;
        List<string> entries = new List<string>();
        List<string> faces = new List<string>();

        // Frames
        private FaceRecognitionPage _faceRecPage;
        private SignaturePage _signPage;
        //----------------------------



        public Log log { get { return _log; } set { _log = value; } }
        public Database_Utils db_util { get { return _db_util; } set { _db_util = value; } }
        public FaceRecognitionPage faceRecPage { get { return _faceRecPage; } set { _faceRecPage = value; } }
        public SignaturePage signPage { get { return _signPage; } set { _signPage = value; } }


        public MainWindow()
        {
            openConsole();
            InitializeComponent();
            db_util = new Database_Utils();
            //drawFaceCanvasGrid();
            db_util = new Database_Utils();
            DBScripts script = new DBScripts();
           
          
        }

        private void exit_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            db_util.close_log(log);
            Application.Current.Shutdown();
        }

        private void logout_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login login_window = new Login();
            login_window.Show();
            db_util.close_log(log);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db_util.close_log(log);
            Application.Current.Shutdown();

        }


        private void Ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((FacePage != null) && (SignPage != null))
            {
                if (SignTab.IsSelected)
                {
                    FacePage.Visibility = Visibility.Hidden;
                    SignPage.Visibility = Visibility.Visible;
                }
                else if (FaceTab.IsSelected)
                {
                    FacePage.Visibility = Visibility.Visible;
                    SignPage.Visibility = Visibility.Hidden;
                }
            }
        }

   

        private void signature_open_file1_btn_Click(object sender, RoutedEventArgs e)
        {
            signature1.Children.Clear();
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.ShowDialog();
            string filePath = ofd.FileName;
            string safeFilePath = ofd.SafeFileName;

            if (filePath != "")
            {
                sgn.getFile1(filePath);
                file1.Content = "File 1:\n" + sgn.file1_text;
                // _signPage.file1.Content = sgn.file1_text;
                int minX = 99999, minY = 99999, maxX = 0, maxY = 0;
                for (int i = 0; i < sgn.points1.Length - 1; i++)
                {
                    if (sgn.points1[i].x < minX)
                        minX = sgn.points1[i].x;
                    if (sgn.points1[i].x > maxX)
                        maxX = sgn.points1[i].x;
                    if (sgn.points1[i].y < minY)
                        minY = sgn.points1[i].y;
                    if (sgn.points1[i].y > maxY)
                        maxY = sgn.points1[i].y;
                }
                for (int i = 0; i < sgn.points1.Length - 1; i++)
                {
                    Line line = new Line();

                    line.Visibility = System.Windows.Visibility.Visible;
                    line.StrokeThickness = 4;
                    line.Stroke = System.Windows.Media.Brushes.Black;

                    line.X1 = (sgn.points1[i].x - minX) * (500 / (maxX - minX));
                    line.Y1 = (sgn.points1[i].y - minY) * (500 / (maxY - minY));
                    line.X2 = (sgn.points1[i + 1].x - minX) * (500 / (maxX - minX));
                    line.Y2 = (sgn.points1[i + 1].y - minY) * (500 / (maxY - minY));



                    signature1.Children.Add(line);
                }
            }
        }

        private void signature_open_file2_btn_Click(object sender, RoutedEventArgs e)
        {
            signature2.Children.Clear();
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.ShowDialog();
            string filePath = ofd.FileName;
            string safeFilePath = ofd.SafeFileName;

            if (filePath != "")
            {
                sgn.getFile2(filePath);
                file2.Content ="File 2:\n"+ sgn.file2_text;
                //   _signPage.file2.Content = sgn.file2_text;
                int minX = 99999, minY = 99999, maxX = 0, maxY = 0;
                for (int i = 0; i < sgn.points2.Length - 1; i++)
                {
                    if (sgn.points2[i].x < minX)
                        minX = sgn.points2[i].x;
                    if (sgn.points2[i].x > maxX)
                        maxX = sgn.points2[i].x;
                    if (sgn.points2[i].y < minY)
                        minY = sgn.points2[i].y;
                    if (sgn.points2[i].y > maxY)
                        maxY = sgn.points2[i].y;
                }
                for (int i = 0; i < sgn.points2.Length - 1; i++)
                {
                    Line line = new Line();

                    line.Visibility = System.Windows.Visibility.Visible;
                    line.StrokeThickness = 4;
                    line.Stroke = System.Windows.Media.Brushes.Red;

                    line.X1 = (sgn.points2[i].x - minX) * (500 / (maxX - minX));
                    line.Y1 = (sgn.points2[i].y - minY) * (500 / (maxY - minY));
                    line.X2 = (sgn.points2[i + 1].x - minX) * (500 / (maxX - minX));
                    line.Y2 = (sgn.points2[i + 1].y - minY) * (500 / (maxY - minY));



                    signature2.Children.Add(line);
                }
            }
        }

        private void signature_compute_btn_Click(object sender, RoutedEventArgs e)
        {
            if (file1 != null && file2 != null)
            {
                sgn.compute();
                rezult.Content = sgn.output;

                out1.Text = "Length Ratio:" + sgn.rasp + "% \n" + "Comparares all the ratios of all the "+
                    "possible distanced between all points and then counts how many of them correspond within"
                    + " given Error";
                out2.Text = "Angle Similarity:" + sgn.rasp2 + "% \n " + "Calculates all the angles bwtween all " +
                    "3 consecutive points, and then checks for similarities whitin given Error. performed by simulating the derivate in one point";
                out3.Text = "Circumcised Circle Center:" + sgn.rasp3 + "% \n " + "Calculates the center of the circumcised circle of the signature " +
                    "and compares the ratios of all the distances between the center and signature's points within"
                    + " given Error"; ;

            }
           // MessageBox.Show(sgn.output,"Output:");
        }

        private void face_comparison_file_btn_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string filePath = ofd.FileName;
            string safeFilePath = ofd.SafeFileName;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(filePath);
            image.EndInit();
            ImageBrush myImageBrush = new ImageBrush(image);
            FaceLocalCompImage.Background = myImageBrush;

        }

        private void face_sample_file_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string filePath = ofd.FileName;
            string safeFilePath = ofd.SafeFileName;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(filePath);
            image.EndInit();
            ImageBrush myImageBrush = new ImageBrush(image);
            FaceLocalSampleImage.Background = myImageBrush;
        }

        public void drawFaceCanvasGrid()
        {
            Line line = new Line();
            line.Visibility = System.Windows.Visibility.Visible;
            line.StrokeThickness = 4;
            line.Stroke = System.Windows.Media.Brushes.Red;
            line.X1 = 0;
            line.Y1 = 0;
            line.X2 = 0;
            line.Y2 = FaceCanvasGrid.Width-10;
            FaceCanvasGrid.Children.Add(line);
            /*
            for (int i = 0; i < FaceCanvasGrid.Width; i += 10)
            {
                
                line.X1 = 0;
                line.Y1 = i;
                line.X2 = 0;
                line.Y2 = ;
                FaceCanvasGrid.Children.Add(line);
            }
             * */
        }
      

        private void face_download_database_btn_Click(object sender, RoutedEventArgs e)
        {
            
            entries = db_util.get_Entries();

            FaceDatabaseEntries.Items.Clear();
            for (int i = 0; i < entries.Count; i++)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = entries.ElementAt(i);
                FaceDatabaseEntries.Items.Add(lbi);
            }
        }

        private void FaceDatabaseEntries_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int id = FaceDatabaseEntries.SelectedIndex;
            String[] fullName = entries.ElementAt(id).Split(' ');

            faces = db_util.get_Faces(fullName[0], fullName[1]);

            FaceDatabaseItems.Items.Clear();
            for (int i = 0; i < faces.Count; i++)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = faces.ElementAt(i);
                FaceDatabaseItems.Items.Add(lbi);
            }
        }

    }

}
