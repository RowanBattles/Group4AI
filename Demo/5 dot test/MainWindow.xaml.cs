using _5_dot_test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using Line = _5_dot_test.Line;

namespace FiveDotTest
{
    public partial class MainWindow : Window
    {
        private Line[] lines;
        private Box[] boxes;
        private int currentBox;
        private int submissions;
        private int emptySubmissions;
        private int pauses;
        private DateTime lastSubmission;
        private int duplicates;
        private List<Pattern> submittedPatterns;
        private int timeLeft;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            submissions = 0;
            timeLeft = 180;
            currentBox = 0;
            pauses = 0;
            submittedPatterns = new List<Pattern>();
            duplicates = 0;
            emptySubmissions = 0;
            lastSubmission = DateTime.Now;


            lines = new Line[8];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new Line();
            }

            boxes = new Box[18];
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i] = new Box();
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void DisableLines()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].IsClicked = false;
                Rectangle line = (Rectangle)FindName($"line{i + 1}");
                line.Fill = new SolidColorBrush(Colors.Gray);
            }
        }

        private void HandleDuplicate()
        {
            var newPattern = new Pattern();
            for (int i = 0; i < lines.Length; i++)
            {
                newPattern.lines[i] = lines[i].IsClicked;
            }

            foreach (var pattern in submittedPatterns)
            {
                if (pattern.Matches(newPattern))
                {
                    duplicates++;
                    DuplicatesTextBlock.Text = $"Duplicates: {duplicates}";
                    return;
                }
            }

            submittedPatterns.Add(newPattern);
        }

        private void HandleEmptySubmission()
        {
            bool isEmpty = true;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IsClicked)
                {
                    isEmpty = false;
                    break;
                }
            }

            if (isEmpty)
            {
                emptySubmissions++;
                EmptySubmissionsTextBlock.Text = $"Empty Submissions: {emptySubmissions}";
            }
        }

        private void HandlePause()
        {
            DateTime currentSubmission = DateTime.Now;

            // Count the clicked lines from the submitted pattern
            int clickedLines = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IsClicked)
                {
                    clickedLines++;
                }
            }

            if ((currentSubmission - lastSubmission).TotalSeconds > (5 + (0.8 * clickedLines)))
            {
                pauses++;
                PausesTextBlock.Text = $"Pauses: {pauses}";
            }
            lastSubmission = currentSubmission;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            TimerTextBlock.Text = $"Time: {timeLeft} Seconds";
            currentBox = (180 - timeLeft) / 10;
            BoxNumberBlock.Text = $"Box: {currentBox}";
            if (timeLeft == 0)
            {
                timer.Stop();
                MessageBox.Show("Time's up!");
            }
        }

        private void Line_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (timeLeft > 0)
            {
                Rectangle clickedRectangle = sender as Rectangle;
                int index = int.Parse(clickedRectangle.Name.Replace("line", "")) - 1;

                if (lines[index].IsClicked)
                {
                    lines[index].IsClicked = false;
                    clickedRectangle.Fill = new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    lines[index].IsClicked = true;
                    clickedRectangle.Fill = new SolidColorBrush(Colors.LimeGreen);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (timeLeft > 0)
            {
                HandlePause();
                HandleDuplicate();
                HandleEmptySubmission();
                submissions++;
                boxes[currentBox].Submissions++;
                CounterTextBlock.Text = submissions.ToString();
                DisableLines();
            }   
        }

        private void Button_export(object sender, RoutedEventArgs e)
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "counter.csv");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Create header
                writer.Write("pauses,duplicates,empty_submissions,");
                for (int i = 0; i < boxes.Length; i++)
                {
                    writer.Write($"Box_{i + 1}_Submissions,Box_{i + 1}_Clicks,Box_{i + 1}_Unclicks");
                    if (i < boxes.Length - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();

                // Create value line
                writer.Write($"{pauses},{duplicates},{emptySubmissions},");
                for (int i = 0; i < boxes.Length; i++)
                {
                    writer.Write($"{boxes[i].Submissions},{boxes[i].Clicks},{boxes[i].Unclicks}");
                    if (i < boxes.Length - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();
            }
            MessageBox.Show($"Counter value exported to {filePath}");
        }
    }
}