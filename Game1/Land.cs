using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Land
    {
        ////////////////////////////////////////
        //  Caches Data for Land Type Tiles   //
        //      Data Includes:                //
        // Global Positioning for Tile (X, Y) //
        // Base Tile Land Type                //
        // Base Tile Natural Biome            //
        // Modifcations to Tile (Draw-Over)   //
        ////////////////////////////////////////


        /// 
        /// VARIABLES THAT NEED REINITIALIZED ON LOAD:
        /// .frame = 5;
        /// .rotate
        /// 
        /// 
        /// VARIABLES THAT NEED SAVED FOR LOADING:
        /// .biome
        /// .depth[0]
        /// 


        //public int X;
        //public int Y;
        //public int land { get; set; }
        //public int mod { get; set; }
        public int frame = 5;
        public int biome = 0;
        public int land = 0;
        public float? rotate = null;
        public bool IsActive = false;
        public bool IsOccupied = false;

        public Land()//int x, int y, int m)
        {
            //X = x;
            //Y = y;
            //land = l;
            //biome = b;
            //mod = m;
            //rotate = (float?)r;
        }

    }
}
