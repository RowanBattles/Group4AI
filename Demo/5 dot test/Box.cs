using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5_dot_test
{
    public class Box
    {
        public int Submissions { get; set; }
        public int Unclicks { get; set; }
        public int Clicks { get; set; }
        public int TimeGap { get; set; }
        public List<Pattern> Patterns { get; set; }

        public Box()
        {
            Patterns = new List<Pattern>();
            Submissions = 0;
            Unclicks = 0;
            Clicks = 0;
        }
    }
}
