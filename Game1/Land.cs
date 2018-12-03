﻿using System;
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


        //public int X;
        //public int Y;
        //public int land { get; set; }
        //public int mod { get; set; }
        //public float? rotate = null;
        public int frame = 5;
        public int biome = 0;
        public int land = 0;
        public bool IsActive = false;
        public bool IsOccupied = false;
        public DateTime LastUpdate = new DateTime();
        public sbyte[] Manor { get; set; }

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
