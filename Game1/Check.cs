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

        public static Rectangle TileAtCursor(MouseState mouseState)
        {
            for (int y = 0; y < (int)(24 / tileScaleConst); y++)
            {
                for (int x = 0; x < (int)(42 / tileScaleConst); x++)
                {
                    if (TileFrame[x, y].Contains(mouseState.X, mouseState.Y))
                    {
                        Show.CursorLand = landArray[cameraLocationX + x, cameraLocationY + y];
                        return (TileFrame[x, y]);
                    }
                }
            }
            return (OverflowRectangle);
        }
    }
}
