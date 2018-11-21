using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Archive
    {
        // OLD //

        /* DrawTiles;
         * Input Values: cameraLocationX, cameraLocation Y;
         * This function will run a number of times that is less than
         * the display height divided by default tile rendering dimension 50
         * divided by the scale of tile drawings to get number of tiles on screen
         * then add overflow number of 2 divided by tile scale to draw past screen broders (+2)
         * Steps: Determine global tile at current position camera x & y plus
         * the point at which in this function (int x or y) it has already drawn to
         * then check for biome before using tile scale and location data to draw tile
         *

        */


        // new
        /*
        public static void DrawTiles(int tempTilePointerX, int tempTilePointerY, int displayWidth, int displayHeight, double tileScale, Land[,] landArray)
        {
            double rnd;

            for (int y = 0; y < ((displayHeight / 50) / tileScale) + 2 / tileScale; y++)
            {
                for (int x = 0; x < ((displayWidth / 50) / tileScale) + 2 / tileScale; x++)
                {
                    rnd = Random.Next(0, 10000);
                    if (rnd == 5 || rnd == 9995 && landArray[tempTilePointerX + x, tempTilePointerY + y].land == 5)
                    {
                        if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame < 5 && rnd > 5000)
                        {
                            landArray[tempTilePointerX + x, tempTilePointerY + y].frame = Check.Max(landArray[tempTilePointerX + x, tempTilePointerY + y].frame + 1, 5);
                            landArray[tempTilePointerX + x + 1, tempTilePointerY + y].frame = Check.Max(landArray[tempTilePointerX + x + 1, tempTilePointerY + y].frame + 1, 5);
                            landArray[tempTilePointerX + x, tempTilePointerY + y + 1].frame = Check.Max(landArray[tempTilePointerX + x, tempTilePointerY + y + 1].frame + 1, 5);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame > 0 && rnd < 5000)
                        {
                            rnd = Random.Next(3, 6);
                            for (int i = 1; i < rnd; i++)
                            {
                                landArray[tempTilePointerX + x, tempTilePointerY + y].frame = Check.Min(landArray[tempTilePointerX + x, tempTilePointerY + y].frame - 1, 0);
                                landArray[tempTilePointerX + x + i, tempTilePointerY + y].frame = Check.Min(landArray[tempTilePointerX + x + i, tempTilePointerY + y].frame - 1, 0);
                                landArray[tempTilePointerX + x, tempTilePointerY + y + i].frame = Check.Min(landArray[tempTilePointerX + x, tempTilePointerY + y + i].frame - 1, 0);
                                landArray[tempTilePointerX + x + i, tempTilePointerY + y + i].frame = Check.Min(landArray[tempTilePointerX + x + i, tempTilePointerY + y + i].frame - 1, 0);
                            }
                        }
                    }
                    if (landArray[tempTilePointerX + x, tempTilePointerY + y].land > 0)
                    {
                        Texture2D texture = DrawingBoard.Tiles[landArray[tempTilePointerX + x, tempTilePointerY + y].land,
                                landArray[tempTilePointerX + x, tempTilePointerY + y].biome,
                                landArray[tempTilePointerX + x, tempTilePointerY + y].frame];
                        Object obj = Object.Objects[landArray[tempTilePointerX + x, tempTilePointerY + y].land];
                        spriteBatch.Draw(texture,
                            new Rectangle((x - (obj.X) / 50 + 1) * (x * (int)(50 * tileScale)),
                                (y - (obj.Y) / 50 + 1) * (y * (int)(50 * tileScale)),
                                (int)(obj.Width * tileScale),
                                (int)(obj.Height * tileScale)),
                            Color.White);
                    }
                }
            }
        }
         */





    }
}
