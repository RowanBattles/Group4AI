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
        private int uniquePatterns;
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
            uniquePatterns = 0;
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
            var newPattern = new Pattern(DateTime.Now);
            bool isUnique = true;
            for (int i = 0; i < lines.Length; i++)
            {
                newPattern.lines[i] = lines[i].IsClicked;
            }

            foreach (var pattern in submittedPatterns)
            {
                if (pattern.Matches(newPattern))
                {
                    duplicates++;
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
                uniquePatterns++;
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
            }
        }

        private void HandlePause()
        {
            DateTime currentSubmission = DateTime.Now;

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
            }
            lastSubmission = currentSubmission;
        }

        private void HandleBox()
        {
            boxes[currentBox].Patterns.Add(new Pattern(DateTime.Now));
            int clickedLines = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IsClicked)
                {
                    clickedLines++;
                }
            }
            boxes[currentBox].Lines += clickedLines;
            boxes[currentBox].Submissions++;
        }

        private void CalculateTimegap()
        {
            foreach (var box in boxes)
            {
                if (box.Patterns.Count > 1)
                {
                    var biggestTimeGap = 0;

                    for (int i = 0; i < box.Patterns.Count - 1; i++)
                    {
                        var timeGap = (box.Patterns[i + 1].timestamp - box.Patterns[i].timestamp).TotalMilliseconds;
                        if (timeGap > biggestTimeGap)
                        {
                            biggestTimeGap = (int)timeGap;
                        }
                    }
                    box.TimeGap = biggestTimeGap;
                }
                else
                {
                    box.TimeGap = 10000;
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            TimerTextBlock.Text = $"Time: {timeLeft} Seconds";
            currentBox = (180 - timeLeft) / 10;
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
                    boxes[currentBox].Unclicks++;
                    lines[index].IsClicked = false;
                    clickedRectangle.Fill = new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    boxes[currentBox].Clicks++;
                    lines[index].IsClicked = true;
                    clickedRectangle.Fill = new SolidColorBrush(Colors.LimeGreen);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (timeLeft > 0)
            {
                HandleBox();
                HandlePause();
                HandleDuplicate();
                HandleEmptySubmission();
                submissions++;
                CounterTextBlock.Text = submissions.ToString();
                DisableLines();
            }   
        }

        private void Button_export(object sender, RoutedEventArgs e)
        {
            CalculateTimegap();
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.csv");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Create header
                writer.Write("pauses,unique_patterns_count,total_values_count,duplicates,empty_submissions,");
                for (int i = 0; i < boxes.Length; i++)
                {
                    writer.Write($"Box_{i + 1}_Submissions,Box_{i + 1}_Lines,Box_{i + 1}_Clicks,Box_{i + 1}_Unclicks,Box_{i + 1}_Timegap");
                    if (i < boxes.Length - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();

                // Create value line
                writer.Write($"{pauses},{uniquePatterns},{submissions},{duplicates},{emptySubmissions},");
                for (int i = 0; i < boxes.Length; i++)
                {
                    writer.Write($"{boxes[i].Submissions},{boxes[i].Lines},{boxes[i].Clicks},{boxes[i].Unclicks},{boxes[i].TimeGap}");
                    if (i < boxes.Length - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();
            }
            MessageBox.Show($"Test succesfully exported!");
        }
    }
}