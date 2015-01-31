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

        private void buttonSearchRandomly_Click(object sender, RoutedEventArgs e)
        {
            float bestFitnessSoFar = 1000f;
            int maxTrials = 1000000;
            int trials = 0;

            while (trials++ < maxTrials && bestFitnessSoFar > 0f)
            {
                Program program = m_Engine.CreateRandomProgram();
                float fitness = new GP1.FitnessFunctionAlternatesGettingLarger().Evaluate(program);
                if (fitness < bestFitnessSoFar)
                {
                    m_Program = program;
                    bestFitnessSoFar = fitness;
                }
            }

            DrawProgram(m_Program);
            ShowFitness(m_Program);
        }

        private void buttonGenRandProgram_Click(object sender, RoutedEventArgs e)
        {
            Program program = m_Engine.CreateRandomProgram();
            DrawProgram(program);
            ShowFitness(program);
            m_Program = program;
        }

        private void buttonMutateProgram_Click(object sender, RoutedEventArgs e)
        {
            m_Program.Mutate();
            DrawProgram(m_Program);
            ShowFitness(m_Program);
        }

        private void ShowFitness(Program program)
        {
            labelFitness.Content = new GP1.FitnessFunctionAlternatesGettingLarger().Evaluate(program).ToString("0.00");
        }

        private void DrawProgram(Program program)
        {
            Bitmap bmp = program.Draw();
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
