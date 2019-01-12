using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Region
{
    public class Duchy
    {
        //List<County> Counties = new List<County>();
        Player Duke { get; set; }
        Kingdom Domain { get; set; }
        int[] Capitol { get; set; }

        public void SetCapitol(int x, int y)
        {
            Capitol = new int[2] { x, y };
        }
    }
}
