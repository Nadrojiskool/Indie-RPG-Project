using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public sealed class GPS : Tuple<int, int, int>
    {
        public GPS(int x, int y, int z) : base(x, y, z) { }

        public int X => Item1;

        public int Y => Item2;

        public int Z => Item3;
    }
}
