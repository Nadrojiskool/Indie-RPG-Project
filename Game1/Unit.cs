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
        // Notes:
        // - Need to establish unit hierarchies so that dumber units can spawn in larger numbers
        // > This means dumber units won't path as far, and are limited to a small number of attacks
        // > Lowest on the hierachy are creatures which have no pathing and can only cast single tile attacks at most
        // >> The next tier may have small tile range attacks and limited pathing
        // >>> The next tier may have advanced pathing and dependent hierachies (including unit-independent animation and logic threads)

        /// <summary> Future Unit Action Array Index
        /// 0: Idle
        /// 1: Pathing
        /// 2: Follow Player
        /// 3: Gather Resources
        /// 4: Attack Target
        /// 
        /// 9: Sleep
        /// </summary>

        /// <summary> Unit Stats Array Index
        /// 0: Level (LVL) = 1 + Stats 10-30
        /// 1: Health (HP) = 10 * (2 + Vitality(11) + Physique(12))
        /// 2: Attack (ATK) = 1 + Combat(14) * (Agility(13) + Physique(12))
        /// 3: Defense (DEF) = 1 + Combat(14) * (Agility(13) + Physique(12) + Vitality(11))
        /// 4: Speed (SPD) = 1 / (101 - (Agility(13) * Vitality(11)))
        /// 5: 
        /// 9: 
        /// 10: Experience
        /// 11: Vitality
        /// 12: Physique
        /// 13: Agility
        /// 14: Combat
        /// 15: Magic
        /// 16: Knowledge
        /// 17: Leadership
        /// 18: Sociability
        /// 19: Expertise
        /// 20: Gathering
        /// 21: Wood Cutting
        /// 22: Wood Working
        /// 23: Mining
        /// 24: Smithing
        /// 25: Engineering
        /// 26: Construction
        /// 27: Hunting
        /// 28: Skinning
        /// 29: Butchering
        /// 30: Cooking
        /// 100: Worker Capacity
        /// 101: Gold
        /// 102: Workers
        /// 
        /// 
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
        public int[] DestinationOffset = new int[2] { 0, 0 };
        public int[] OriginOffset = new int[2] { 0, 0 };
        public sbyte LeftOrRight = 0; // Cached Rotation (LastMove) // Negative (-) is FavorLeft // Positive (+) is FavorRight //
        
        // Bloat for the following Test Pathing variable is concerning
        public List<int[]> Pathed { get; set; }

        //public static List<Unit> Active = new List<Unit>();

        public Unit(int x, int y, int id, int[] array) : base($"{id}", x, y, 0, 0)
        {
            this.X = x;
            this.Y = y;
            this.ID = id;
            this.Stats = array;
        }

        public void Attack(Unit unit)
        {
            unit.Stats[1] -= Check.Min(this.Stats[2] - unit.Stats[3], 1);
        }

        public void CheckSlash(Unit unit)
        {
            int x = X + Game1.MovementXY[LastMove, 0] + Game1.MovementXY[Check.LoopInt(LastMove - 1, 1, 4), 0];
            int y = Y + Game1.MovementXY[LastMove, 1] + Game1.MovementXY[Check.LoopInt(LastMove - 1, 1, 4), 1];
            for (int i = 0; i < 3; i++)
            {
                if (unit.X == (x + (i * Game1.MovementXY[Check.LoopInt(LastMove + 1, 1, 4), 0]))
                    && unit.Y == (y + (i * Game1.MovementXY[Check.LoopInt(LastMove + 1, 1, 4), 1])))
                {
                    Attack(unit);
                }
            }
        }
        
        public void CheckAOE(Unit unit, int x, int y, int width, int height)
        {
            for (int a = 0; a < height; a++)
            {
                for (int b = 0; b < width; b++)
                {
                    if (unit.X == (x + b)
                        && unit.Y == (y + a))
                    {
                        Attack(unit);
                    }
                }
            }
        }
    }
}
