using _5_dot_test;
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
using System.Windows.Threading;

namespace FiveDotTest
{
    public partial class MainWindow : Window
    {
        private Submission counter;
        private int timeLeft;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            counter = new Submission();
            timeLeft = 180;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            TimerTextBlock.Text = $"Time: {timeLeft} Seconds";
            if (timeLeft == 0)
            {
                timer.Stop();
                MessageBox.Show("Time's up!");
            }
        }

        private void linetop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void linebottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lineLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lineRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void linetopleft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void linetopright_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void linebottomleft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void linebottomright_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            counter.IncreaseCount();
            CounterTextBlock.Text = counter.Counter.ToString();
        }

        private void Button_export(object sender, RoutedEventArgs e)
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "counter.txt");
            counter.Export(filePath);
            MessageBox.Show("Exported to " + filePath);
        }
    }
}

