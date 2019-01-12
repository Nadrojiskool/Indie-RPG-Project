using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Animation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ID { get; set; }
        public int Frame { get; set; }

        public Animation(int x, int y, int id, int frame)
        {
            this.X = x;
            this.Y = y;
            this.ID = id;
            this.Frame = frame;
        }
    }
}
