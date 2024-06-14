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
        public Result(int value, string group)
        {
            InitializeComponent();
            TotalScore.Text = $"Total score: {value}";
            ResultGroup.Text = $"Result: {group}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Start start = new Start();
            start.Show();
            this.Close();
        }
    }
}
