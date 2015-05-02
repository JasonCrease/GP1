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
            m_Engine.FitnessFunction = new FitnessFunctionIsItAStraight();
        }

        private Program m_Program1;
        private Program m_Program2;
        private Engine m_Engine;

        private void buttonSearchRandomly_Click(object sender, RoutedEventArgs e)
        {
            float bestFitnessSoFar = 1000f;
            int maxTrials = 100000;
            int trials = 0;

            while (trials++ < maxTrials && bestFitnessSoFar > 0f)
            {
                Program program = m_Engine.CreateRandomProgram();
                float fitness = m_Engine.FitnessFunction.Evaluate(program);
                if (fitness < bestFitnessSoFar)
                {
                    m_Program1 = program;
                    bestFitnessSoFar = fitness;
                }
            }

            DrawProgram(m_Program1, imageProgram1);
            ShowFitness(m_Program1);
            GP1.Compiler.Compiler compiler = new GP1.Compiler.Compiler();
            compiler.Compile(m_Program1, "Prog.dll");
        }

        public void EvolutionDone(object sender, EventArgs e)
        {
            Program p = m_Engine.GetStrongestProgram();
            GP1.Compiler.Compiler compiler = new GP1.Compiler.Compiler();
            compiler.Compile(p, "Prog.dll");

            UpdateUiWhenEvolving(null);
        }

        System.Threading.Timer updateUiTimer;

        private void buttonDoEvolution_Click(object sender, RoutedEventArgs e)
        {
            m_Engine.RunAsync(EvolutionDone);
            updateUiTimer = new System.Threading.Timer(UpdateUiWhenEvolving, null, 1000, 1000);
        }

        private void UpdateUiWhenEvolving(object state)
        {
            Dispatcher.Invoke(delegate {
                labelGeneration.Content = "Generation: " + m_Engine.CurrentGeneration;
                Program p = m_Engine.GetStrongestProgram();
                DrawProgram(p, imageProgram1);
                ShowFitness(p);
                }
            );
        }

        private void buttonGenRandProgram1_Click(object sender, RoutedEventArgs e)
        {
            Program program = m_Engine.CreateRandomProgram();
            DrawProgram(program, imageProgram1);
            ShowFitness(program);
            m_Program1 = program;

            GP1.Compiler.Compiler compiler = new GP1.Compiler.Compiler();
            compiler.Compile(m_Program1, "Prog.dll");
        }
        private void buttonGenRandProgram2_Click(object sender, RoutedEventArgs e)
        {
            Program program = m_Engine.CreateRandomProgram();
            DrawProgram(program, imageProgram2);
            ShowFitness(program);
            m_Program2 = program;
        }
        private void buttonShowCrossoverProgram_Click(object sender, RoutedEventArgs e)
        {
            Program program = m_Program1.Crossover(m_Program2);
            DrawProgram(program, imageProgram3);
            ShowFitness(program);
        }
        private void buttonMutateProgram_Click(object sender, RoutedEventArgs e)
        {
            m_Program1.Mutate();
            DrawProgram(m_Program1, imageProgram3);
            ShowFitness(m_Program1);
        }

        private void ShowFitness(Program program)
        {
            labelFitness.Content = program.Fitness.ToString("0.00");
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
