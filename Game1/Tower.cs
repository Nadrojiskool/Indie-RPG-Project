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
        /* Tower Outline
         * 
         * Fire: 3/10/2
         * Ice: 7/5/3
         * Wind: 10/3/5
         * Earth: 5/5/5
         * Lightning: 7/3/5
         * Water: 3/5/7
         * Light: 15/2/8
         * Dark: 5/18/2
         */
        //public Action<int> Scan { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
        public int Speed { get; set; }
        public Stopwatch TimeIdle = new Stopwatch();

        public Tower(/*Action<int> scan, */int range, int damage, int speed)
        {
            //Scan = scan;
            Range = range;
            Damage = damage;
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
