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
        /// <summary> Future Unit Action Array Index
        /// 0: Idle
        /// 1: Pathing
        /// 2: Follow Player
        /// 3: Gather Resources
        /// </summary>

        public int[] Stats = new int[200];
        public int ID { get; set; }
        public byte ActionID = 0;
        public Stopwatch ActionTime = new Stopwatch();
        public int ActionDuration { get; set; }
        public float Rotation { get; set; }
        public int LastMove { get; set; }
        public int Depth { get; set; }
        public int AutoX { get; set; }
        public int AutoY { get; set; }
        //public sbyte[] Path;
        public int[] DestinationOffset = new int[2] { -10, 0 };
        public int[] OriginOffset = new int[2] { 0, 0 };
        public sbyte LeftOrRight = 0; // -1 is FavorLeft // 1 is FavorRight //
        //public static List<Unit> Active = new List<Unit>();

        public Unit(int x, int y, int id, int[] array) : base($"{id}", x, y, 0, 0)
        {
            this.X = x;
            this.Y = y;
            this.ID = id;
            this.Stats = array;
        }

    }
}
