using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Player : Unit
    {
        static public Player player;
        public int tileX { get; set; }
        public int tileY { get; set; }
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public int[] resources = new int[1000];
        /*
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
         * 10: Capacity
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
        public int gold { get; set; }
        //public ArrayList Workers = new ArrayList();
        public static List<Unit> Units = new List<Unit>();
        public int xp { get; set; }

        public Player (int x, int y, int[] array) : base(x, y, 0, array)
        {
            this.X = x;
            this.Y = y;
            this.stats = array;
            this.depth = 0;
        }
    }
}
