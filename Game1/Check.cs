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
    public class Check : Game1
    {
        // Check value against a minimum, if below return min //
        public static int Min(int value, int min)
        {
            if (value < min)
            {
                value = min;
            }
            return (value);
        }
        // Check value against a maximum, if above return max //
        public static int Max(int value, int max)
        {
            if (value > max)
            {
                value = max;
            }
            return (value);
        }
        // Could also be called "CheckRange" //
        public static int Range(int value, int min, int max)
        {
            value = Min(value, min);
            value = Max(value, max);
            return (value);
        }
        // Expected one iteration on loop max - min to avoid while statement
        public static int LoopInt(int num, int loopMin, int loopMax)
        {
            int loopSize = loopMax - (loopMin - 1);

            if (num > loopMax)
            {
                num -= loopSize;
            }
            else if (num < loopMin)
            {
                num += loopSize;
            }

            return (num);
        }
        // LoopInt method with trigger method when executing
        public static int LoopInt2(int num1, int num2, int loopMin, int loopMax)
        {
            int num = num1 + num2;
            int loopSize = loopMax - (loopMin - 1);

            if (num == 0)
            {
                num += num2;
            }
            else if (num > loopMax)
            {
                num -= loopSize;
                Control.PlayerMovement(Player.player);
            }
            else if (num < loopMin)
            {
                num += loopSize;
                Control.PlayerMovement(Player.player);
            }

            return (num);
        }
        // Must be positive
        public static int LoopIntPos(int num, int loopMin, int loopMax)
        {
            int loopSize = loopMax - (loopMin - 1);

            if (num > loopMax)
            {
                int num2 = num / loopSize;
                num = 1 + num - (num2 * loopSize);
            }

            return (num);
        }

        public static int XOrY(int i)
        {
            if (i % 2 != 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        // I think I could speed this up by simply returning the rectangle at cursor position
        // This would only require a
        public static Rectangle TileAtCursor(MouseState mouseState)
        {
            int x = (int)Math.Ceiling((double)((mouseState.X - Player.player.TileOffsetXY[0]) / CurrentTileSize));
            int y = (int)Math.Ceiling((double)((mouseState.Y - Player.player.TileOffsetXY[1]) / CurrentTileSize));
            Show.CursorLand = landArray[cameraLocationX + x, cameraLocationY + y];

            return (TileFrame[x, y]);
            /*for (int y = 0; y < (int)(24 / tileScaleConst); y++)
            {
                for (int x = 0; x < (int)(42 / tileScaleConst); x++)
                {
                    if (TileFrame[x, y].Contains(mouseState.X, mouseState.Y))
                    {
                        Show.CursorLand = landArray[cameraLocationX + x, cameraLocationY + y];
                        return (TileFrame[x, y]);
                    }
                }
            }*/
            //return (OverflowRectangle);
        }

        
        public static int AdjacentBiomes(int x, int y, int value)
        {
            int bitmap = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (landArray[x + MovementXY[i, 0], y + MovementXY[i, 0]].biome == value)
                {
                    value += (int)Math.Pow(2, i - 1);
                }
            }
            return bitmap;
        }
        
    }
}
