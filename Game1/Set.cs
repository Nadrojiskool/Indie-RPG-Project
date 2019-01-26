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
    public class Set : Game1
    {
        public static void Land(int[,] blueprint, int x, int y)
        {
            for (int y2 = 0; y2 < blueprint.GetLength(0); y2++)
            {
                for (int x2 = 0; x2 < blueprint.GetLength(1); x2++)
                {
                    landArray[x + x2, y + y2].land = blueprint[y2, x2];
                    landArray[x + x2, y + y2].frame = 5;
                }
            }
        }

        public static void CoreStats(Unit unit)
        {
            int level = 1;
            // Level is equal to all skill levels combined
            for (int i = 10; i <= 30; i++)
            {
                level += unit.Stats[i];
            }
            unit.Stats[0] = level;
            // Health (HP) = 10 * (2 + Vitality(11) + Physique(12))
            unit.Stats[1] = 10 * (2 + unit.Stats[11] + unit.Stats[12]);
            // Attack (ATK) = 1 + Combat(14) * (Agility(13) + Physique(12))
            unit.Stats[2] = 1 + unit.Stats[14] * (unit.Stats[13] + unit.Stats[12]);
            // Defense (DEF) = 1 + Combat(14) * (Agility(13) + Physique(12) + Vitality(11))
            unit.Stats[3] = 1 + unit.Stats[14] * (unit.Stats[13] + unit.Stats[12] + unit.Stats[11]);
            // Speed (SPD) = 1 / (101 - (Agility(13) * Vitality(11)))
            unit.Stats[4] = 1 / (101 - (unit.Stats[13] * unit.Stats[11])); // Expected MAX Stats of 10
        }

        public static void GridSize()
        {
            // going to set rectangles for tileScale of .5
            const int x = (int)(42 / tileScaleConst);
            const int y = (int)(24 / tileScaleConst);
            TileFrame = new Rectangle[x, y];
        }

        public static void GridDimensions()
        {
            for (int y2 = 0; y2 < TileFrame.GetLength(1); y2++)
            {
                for (int x2 = 0; x2 < TileFrame.GetLength(0); x2++)
                {
                    TileFrame[x2, y2].X = (x2 * CurrentTileSize) + Player.player.TileOffsetXY[0];
                    TileFrame[x2, y2].Y = (y2 * CurrentTileSize) + Player.player.TileOffsetXY[1];
                    TileFrame[x2, y2].Width = CurrentTileSize;
                    TileFrame[x2, y2].Height = CurrentTileSize;
                }
            }
        }
    }
}
