using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;
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
        }

        private Program m_Program1;
        private Engine m_Engine;

        private void BackgroundRandomSearch()
        {
            double bestFitnessSoFar = 1000f;
            int maxTrials = 1000;
            int trials = 0;

            m_Engine = new Engine(new FitnessFunction2CardPoker());

            while (trials++ < maxTrials && bestFitnessSoFar > 0f)
            {
                Program program = m_Engine.CreateRandomProgram();
                double fitness = m_Engine.FitnessFunction.Evaluate(program);
                if (fitness < bestFitnessSoFar)
                {
                    m_Program1 = program;
                    bestFitnessSoFar = fitness;
                }

                if (trials % 10000 == 0)
                {
                   Dispatcher.Invoke(() =>
                   {
                       labelGeneration.Content = trials.ToString();
                       labelFitness.Content = m_Engine.FitnessFunction.Evaluate(m_Program1).ToString();
                       DrawProgram(m_Program1, imageProgram1);
                   });
                }
            }
            
            Dispatcher.Invoke(() =>
            {
                DrawProgram(m_Program1, imageProgram1);
                labelFitness.Content = m_Engine.FitnessFunction.Evaluate(m_Program1).ToString();
            });
            GP1.Compiler.Compiler compiler = new GP1.Compiler.Compiler();
            compiler.Compile(m_Program1, "Prog.dll");
        }

        private void buttonSearchRandomly_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(BackgroundRandomSearch);
            t.Start();
        }

        public void EvolutionDone(object sender, EventArgs e)
        {
            updateUiTimer.Dispose();

            Program p = m_Engine.GetStrongestProgram();
            GP1.Compiler.Compiler compiler = new GP1.Compiler.Compiler();
            compiler.Compile(p, "Prog.dll");

            UpdateUiWhenEvolving(null);
        }

        System.Threading.Timer updateUiTimer;

        private void buttonDoEvolution_Click(object sender, RoutedEventArgs e)
        {
            m_Engine = new Engine(new FitnessFunction3CardPoker());
            m_Engine.RunAsync(EvolutionDone);
            updateUiTimer = new System.Threading.Timer(UpdateUiWhenEvolving, null, 1000, 1000);
        }

        private void UpdateUiWhenEvolving(object state)
        {
            Dispatcher.Invoke(delegate {
                    Program p = m_Engine.GetStrongestProgram();
                    DrawProgram(p, imageProgram1);
                    Program p2 = m_Engine.getRandomProgram();
                    DrawProgram(p2, imageProgram2);
                    ShowStats(p);
                }
            );
        }

        private void ShowStats(Program program)
        {
            labelGeneration.Content = m_Engine.CurrentGeneration;
            labelFitness.Content = program.Fitness.ToString("0.0");
            label1stQuartile.Content = m_Engine.PopulationStatistics.FitnessFirstQuartile.ToString("0.0");
            label2ndQuartile.Content = m_Engine.PopulationStatistics.FitnessSecondQuartile.ToString("0.0");
            label3rdQuartile.Content = m_Engine.PopulationStatistics.FitnessThirdQuartile.ToString("0.0");
            label4thQuartile.Content = m_Engine.PopulationStatistics.FitnessFourthQuartile.ToString("0.0");
        }

        private void DrawProgram(Program program, System.Windows.Controls.Image image)
        {
            Bitmap bmp = program.Draw();
            image.Source = loadBitmap(bmp);
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
