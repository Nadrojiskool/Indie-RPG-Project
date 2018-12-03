using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Unit : Object
    {
        public int[] stats = new int[200];
        public int ID { get; set; }
        public Stopwatch ActionTime = new Stopwatch();
        public int ActionDuration { get; set; }
        public float Rotation { get; set; }
        public int LastMove { get; set; }
        public int depth { get; set; }
        public int AutoX { get; set; }
        public int AutoY { get; set; }
        public static List<Unit> Active = new List<Unit>();

        public Unit(int x, int y, int id, int[] array) : base($"Worker {id}", x, y, 0, 0)
        {
            this.X = x;
            this.Y = y;
            this.ID = id;
            this.stats = array;
        }

    }
}
