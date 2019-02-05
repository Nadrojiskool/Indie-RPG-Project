using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Game1
{
    public class Tower
    {
        public Action<int> Scan { get; set; }
        public int Range { get; set; }
        public int Speed { get; set; }
        public Stopwatch TimeIdle = new Stopwatch();

        public Tower(Action<int> scan, int range, int speed)
        {
            Scan = scan;
            Range = range;
            Speed = speed;
            TimeIdle.Start();
        }

        /* Tower Scanning and Attacking Template
        public static void Scan(int range)
        {

        }
        */
    }
}
