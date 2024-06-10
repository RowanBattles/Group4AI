using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5_dot_test
{
    public class Pattern
    {
        public bool[] lines { get; set; }
        public DateTime timestamp { get; set; }

        public Pattern(DateTime timestamp)
        {
            lines = new bool[8];
            this.timestamp = timestamp;
        }

        public bool Matches(Pattern pattern)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != pattern.lines[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
