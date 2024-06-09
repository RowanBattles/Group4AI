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

        public Pattern()
        {
            lines = new bool[8];
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
