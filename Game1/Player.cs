using System;
using System.Collections;
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
using Game1.Contract;

namespace Game1
{
    public class Player : Unit
    {
        public static Player player;
        public int tileX { get; set; }
        public int tileY { get; set; }
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public int[] resources = new int[1000];
        /* (Old resources pending removal)
         * 0: Experience
         * 1: Vitality
         * 2: Physique
         * 3: Agility
         * 4: Combat
         * 5: Magic
         * 6: Knowledge
         * 7: Leadership
         * 8: Sociability
         * 9: Expertise
         * 10: Capacity Workers
         * 11: Gathering
         * 12: Wood Cutting
         * 13: Wood Working
         * 14: Mining
         * 15: Smithing
         * 16: Engineering
         * 17: Construction
         * 18: Hunting
         * 19: Skinning
         * 20: Butchering
         * 21: Cooking
         * 101: Gold
         * 102: Workers
         */
        // I'm concerned with the way I implemented these lists
        // I would also like them to just be pointers to the unit at their residence to prevent additional syncing
        public static List<Unit> Workers = new List<Unit>();
        public static List<Unit> Enemies = new List<Unit>();
        public static List<Unit> LocalWorkers = new List<Unit>();
        public static List<Unit> LocalEnemies = new List<Unit>();
        //public static List<Asset> Assets = new List<Asset>();
        public static List<Animation> Animations = new List<Animation>();
        public static List<Item> ItemIndex = new List<Item>();

        public Player (int x, int y, int[] array) : base(x, y, 0, array)
        {
            this.X = x;
            this.Y = y;
            this.Stats = array;
            this.Depth = 0;
        }
    }
}
