using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5_dot_test
{
    public class Submission
    {
        private int counter;

        public Submission()
        {
            counter = 0;
        }

        public int Counter
        {
            get { return counter; }
        }

        public void IncreaseCount()
        {
            counter++;
        }

        public void Export(string filePath)
        {
            File.WriteAllText(filePath, counter.ToString());
        }
    }
}
