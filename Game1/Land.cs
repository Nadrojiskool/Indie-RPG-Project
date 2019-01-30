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
        ////////////////////////////////////////

        /*  Advanced Features:
         *  Tiles store a null array[2] (Domain) which is initialized if an Asset is created
         *  It stores an sbyte for X & Y pointing to asset center
         *  This has limitations of run-time variability, lack of real-time sync, and limit of 256x256 tile assets
         *  But is essentially able to un-pack assets for client use with minimal network impact
         *  This keeps the route of communication clear and variable allowing center indexing
         *  As well as [de]centralized logic storage with hierarchical access allowing proper checkpoints during run-time
         *  
         *  Note: Land Leasing - Asset Owner or Regional Lord will redirect you to Asset Leaser
         *  
         *  Note: During map drawing, each tile can be calculated by region offset to see if it matches existing Region and then draw color of Region
         */

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

        /* Target Map Size (5ft Tiles)
         * 26,295,456 (Earth) / 256 = 102717
         * 256 * 256 = 65536 * 256 = 16,777,216 (erf) or 33,554,432 (ERF) with Negative Coordinates
         * 16,777,216 ^ 2 = 281,474,976,710,656
         * 767.7254TB Per Layer (based on 2.86mb 1000x1000 tileMap)
         */

         /* LAND UPKEEP CONCEPT:
          * 
          * Information can be calculated and stored into land tiles
          * I've layed out elsewhere about naturally dynamic land-use pathing
          * The goal is to store in tiles contextual information which is relevant to traveling units
          * Things like the upkeep of roads and their node-network are an example of this
          * Another example of contextual information would be calculating embedded tiles
          * If a tree tile is surrounded on four sides by trees, it becomes a dense tree tile (forest)
          * If an empty tile has nothing around it, it is a plains tile
          * Units could weight their pathing with tile Path Level while seeking Plains tiles and avoiding Dense tiles
          * The interesting thing about this is the idea of sharing computing power on the network
          * If certain routine maintenance is desired by the network, then there's a trade-off
          * Even if I come to the decision that a feature isn't desirable, it could still be provided
          * A Lord of his Manor could tweak a setting and trade-off performance for having the feature
          * He could also have this task performed by his Kingdom on his behalf which would imply tax dues
          * If his Kingdom doesn't already perform this maintenance, he could push seek social support in the Kingdom
          * He could also pay for the task to be performed on his behalf, or just move to another Kingdom
          */



        //public int X;
        //public int Y;
        //public int land { get; set; }
        //public int mod { get; set; }
        //public float? rotate = null;
        public int frame = 5;
        public int biome = 0;
        public int land = 0;
        public bool IsActive = false;
        //public bool IsOccupied = false;
        public DateTime LastUpdate = new DateTime();

        public bool IsOwned = false;
        public sbyte[] Manor { get; set; }

        public bool IsResident { get; set; }
        public Unit Resident { get; set; }

        public bool IsBorder = false;
        public int Border { get; set; }
        public int BorderBiome { get; set; }

        public Land()//int x, int y, int m)
        {
            //X = x;
            //Y = y;
            //land = l;
            //biome = b;
            //mod = m;
            //rotate = (float?)r;
        }

        public void SetManor(sbyte x, sbyte y)
        {
            Manor = new sbyte[2] { x, y };
        }
    }
}
