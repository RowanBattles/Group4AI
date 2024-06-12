using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace _5_dot_test
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : Window
    {
        public Result(int value)
        {
            InitializeComponent();
            TotalScore.Text = $"Total score: {value.ToString()}";
            PredictClusters();
        }

        private void PredictClusters()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string pythonScript = System.IO.Path.Combine(currentDirectory, "..", "..", "PredictCluster.py"); // Name of your Python script

            if (!File.Exists(pythonScript))
            {
                MessageBox.Show($"Python script not found: {pythonScript}");
                return;
            }

            string pythonExecutable = @"C:\Users\rowan\AppData\Local\Programs\Python\Python311\python.exe";

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = pythonExecutable;
            psi.Arguments = $"\"{pythonScript}\"";
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;

            try
            {
                using (Process process = Process.Start(psi))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        if(result == "0\r\n") 
                        {
                            result = "Slow learner";
                        }
                        else if(result == "1\r\n")
                        {
                            result = "Quick thinker";
                        }
                        else if(result == "2\r\n")
                        {
                            result = "Complex thinker";
                        }
                        else if(result == "3\r\n")
                        {
                            result = "Hesitant student";
                        }
                        else
                        {
                            result = "Unknown";
                        }
                        ResultGroup.Text = $"Result: {result}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                ResultGroup.Text = "Error";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Start start = new Start();
            start.Show();
            this.Close();
        }
    }
}
