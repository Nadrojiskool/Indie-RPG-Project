using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Region
{
    public class County
    {
        //List<Manor> Manors = new List<Manor>();
        Player Earl { get; set; } // or COUNT
        Duchy Domain { get; set; }
        short[] Capitol { get; set; }

        public void SetCapitol(short x, short y)
        {
            Capitol = new short[2] { x, y };
        }
    }
}
