using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Threading;
using GP1;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

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

            m_Engine = new Engine(new FitnessFunction3CardPoker());

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
                       //labelGeneration.Content = trials.ToString();
                       //labelFitness.Content = m_Engine.FitnessFunction.Evaluate(m_Program1).ToString();
                       DrawProgram("Best Program", m_Program1, imageProgram1);
                   });
                }
            }
            
            Dispatcher.Invoke(() =>
            {
                DrawProgram("Best Program", m_Program1, imageProgram1);
                //labelFitness.Content = m_Engine.FitnessFunction.Evaluate(m_Program1).ToString();
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

        bool m_Running = false;

        private void buttonDoEvolution_Click(object sender, RoutedEventArgs e)
        {
            if (!m_Running)
            {
                m_Running = true;
                buttonStartEvolution.Content = "Stop evolution";
                m_Engine = new Engine(new FitnessFunction3CardPoker());
                m_Engine.RunAsync(EvolutionDone);
                updateUiTimer = new Timer(UpdateUiWhenEvolving, null, 1000, 1000);
            }
            else  {
                m_Running = false;
                updateUiTimer.Change(Timeout.Infinite, 1000);
                buttonStartEvolution.Content = "Start evolution";
                m_Engine.Stop();
            }
        }

        bool m_Paused = false;

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            if (!m_Paused) {
                m_Paused = true;
                updateUiTimer.Change(Timeout.Infinite, 1000);
                buttonPause.Content = "Continue";
                m_Engine.Pause();
            }
            else {
                m_Paused = false;
                buttonPause.Content = "Pause";
                updateUiTimer.Change(0, 1000);
                m_Engine.UnPause();
            }
        }

        private void UpdateUiWhenEvolving(object state)
        {
            Dispatcher.Invoke(delegate
            {
                Program p = m_Engine.GetStrongestProgram();
                DrawProgram("Best Program", p, imageProgram1);

                //Program p2 = m_Engine.getRandomProgram();
                //DrawProgram("Some Program", p2, imageProgram2);

                var ps = m_Engine.GetProgramFamily();
                if (ps.Item1 != null)
                    DrawProgram("Parent1", ps.Item1, imageProgram2);
                if (ps.Item2 != null)
                    DrawProgram("Parent2", ps.Item2, imageProgram3);
                if (ps.Item3 != null)
                    DrawProgram("Child", ps.Item3, imageProgram4);

                ShowStats(p);
            }
            );
        }

        private void ShowStats(Program program)
        {
            DrawPopulationHistogram(imageHistogram);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Generation:  {0}\n", m_Engine.CurrentGeneration);
            sb.AppendFormat("Max generations:  {0}\n", Engine.MAXGENERATIONS);
            sb.AppendFormat("Population:  {0}\n", Engine.TARGETPOPULATION);
            sb.AppendFormat("Mutation Rate:  {0}\n", Engine.MUTATIONRATE.ToString("0.000"));
            sb.AppendFormat("Crossover Rate:  {0}\n", Engine.CROSSOVERRATE.ToString("0.000"));
            sb.AppendFormat("Selection P:  {0}\n", Engine.TOURNAMENT_SELECTION_P.ToString("0.000"));
            sb.AppendLine();
            sb.AppendFormat("Best fitness:  {0}\n", m_Engine.GetStrongestProgram().Fitness.ToString("0.0"));
            sb.AppendFormat("Quartile 1:  {0}\n", m_Engine.PopulationStatistics.FitnessFirstQuartile.ToString("0.0"));
            sb.AppendFormat("Quartile 2:  {0}\n", m_Engine.PopulationStatistics.FitnessSecondQuartile.ToString("0.0"));
            sb.AppendFormat("Quartile 3:  {0}\n", m_Engine.PopulationStatistics.FitnessThirdQuartile.ToString("0.0"));
            sb.AppendFormat("Quartile 4:  {0}\n", m_Engine.PopulationStatistics.FitnessFourthQuartile.ToString("0.0"));
            sb.AppendFormat("Best's treesize:  {0}\n", program.TreeSize.ToString());

            labelInfo.Content = sb.ToString();
        }

        private void DrawProgram(String title, Program program, System.Windows.Controls.Image image)
        {
            Bitmap bmp = program.Draw(title, (float)image.Width, (float)image.Height);
            image.Source = loadBitmap(bmp);
        }
        private void DrawPopulationHistogram(System.Windows.Controls.Image image)
        {
            Bitmap bmp = m_Engine.DrawPopulationHistogram((float)image.Width, (float)image.Height);
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
