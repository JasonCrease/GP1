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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GP1;
using System.Drawing;
using System.Runtime.InteropServices;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            m_Engine = new Engine();
        }

        private Program m_Program;
        private Engine m_Engine;

        private void buttonGenRandProgram_Click(object sender, RoutedEventArgs e)
        {
            m_Program = m_Engine.CreateRandomProgram();
            RedrawProgram();
            UpdateFitness();
        }

        private void buttonMutateProgram_Click(object sender, RoutedEventArgs e)
        {
            m_Program.Mutate();
            RedrawProgram();
            UpdateFitness();
        }

        private void UpdateFitness()
        {
            labelFitness.Content = GP1.FitnessFunctionIsQuadratic.(m_Program).ToString("0.00");
        }

        private void RedrawProgram()
        {
            Bitmap bmp = m_Program.Draw();
            imageProgram.Source = loadBitmap(bmp);
        }



        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }
    }
}
