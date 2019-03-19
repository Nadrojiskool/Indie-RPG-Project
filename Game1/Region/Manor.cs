using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Contract;

namespace Game1.Region
{
    public class Manor
    {
        public List<Asset> Assets = new List<Asset>();
        public Player Lord { get; set; }
        public County County { get; set; }
        public int[] Capitol { get; set; }

        public void SetCapitol(int x, int y)
        {
            Capitol = new int[2] { x, y };
        }
    }
}
