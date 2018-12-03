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
         
        public void DrawTiles(int tempTilePointerX, int tempTilePointerY)
        {
            for (int y = 0; y < ((displayHeight / 50) / tileScale) + 2 / tileScale; y++)
                {
                    for (int x = 0; x < ((displayWidth / 50) / tileScale) + 2 / tileScale; x++)
                    {
                        if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 2)// && tileArray[x, y].land != 2)
                        {
                            spriteBatch.Draw(water, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        else if (Player.player.depth > 0)
                        {
                            spriteBatch.Draw(white, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.Black);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 1)
                        {
                            spriteBatch.Draw(land, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 2)
                        {
                            spriteBatch.Draw(snow, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 3)
                        {
                            spriteBatch.Draw(nodeStone, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(player, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        //tileArray[x, y].biome = landArray[tempTilePointerX + x, tempTilePointerY + y].biome;
                        //tileArray[x, y].land = landArray[tempTilePointerX + x, tempTilePointerY + y].land;
                        //tileArray[x, y].mod = landArray[tempTilePointerX + x, tempTilePointerY + y].mod;
                    }
                }

                for (int y = 0; y < ((displayHeight / 50) / tileScale) + 2 / tileScale; y++)
                {
                    for (int x = 0; x < ((displayWidth / 50) / tileScale) + 2 / tileScale; x++)
                    {
                        month = rnd.Next(0, 10000);
                        if (month == 5 || month == 9995 && landArray[tempTilePointerX + x, tempTilePointerY + y].land == 5)
                        {
                            if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame < 5 && month > 5000)
                            {
                                landArray[tempTilePointerX + x, tempTilePointerY + y].frame = Check.Max(landArray[tempTilePointerX + x, tempTilePointerY + y].frame + 1, 5);
                                landArray[tempTilePointerX + x + 1, tempTilePointerY + y].frame = Check.Max(landArray[tempTilePointerX + x + 1, tempTilePointerY + y].frame + 1, 5);
                                landArray[tempTilePointerX + x, tempTilePointerY + y + 1].frame = Check.Max(landArray[tempTilePointerX + x, tempTilePointerY + y + 1].frame + 1, 5);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame > 0 && month < 5000)
                            {
                                int rand = rnd.Next(3, 6);
                                for (int i = 1; i < rand; i++)
                                {
                                    landArray[tempTilePointerX + x, tempTilePointerY + y].frame = Check.Min(landArray[tempTilePointerX + x, tempTilePointerY + y].frame - 1, 0);
                                    landArray[tempTilePointerX + x + i, tempTilePointerY + y].frame = Check.Min(landArray[tempTilePointerX + x + i, tempTilePointerY + y].frame - 1, 0);
                                    landArray[tempTilePointerX + x, tempTilePointerY + y + i].frame = Check.Min(landArray[tempTilePointerX + x, tempTilePointerY + y + i].frame - 1, 0);
                                    landArray[tempTilePointerX + x + i, tempTilePointerY + y + i].frame = Check.Min(landArray[tempTilePointerX + x + i, tempTilePointerY + y + i].frame - 1, 0);
                                }
                            }
                        }
                        if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 1)
                        {
                            if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 3)// && tileArray[x, y].land != 3)
                            {
                                spriteBatch.Draw(bush, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 4)// && tileArray[x, y].land != 4)
                            {
                                Vector2 location = new Vector2(x * (int)(50 * tileScale) + (int)(25 * tileScale) + 1, y * (int)(50 * tileScale) + (int)(25 * tileScale) + 1);
                                Rectangle sourceRectangle = new Rectangle(0, 0, 50, 50);
                                Vector2 origin = new Vector2(25, 25);
                                CreatureAI(rnd.Next(0, 1000), tempTilePointerX + x, tempTilePointerY + y);
                                spriteBatch.Draw(deer, location, sourceRectangle, Color.White, angle, origin, 1.0f * (float)tileScale, SpriteEffects.None, 1);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 5)// && tileArray[x, y].land != 5)
                            {
                                if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 5)
                                {
                                    spriteBatch.Draw(tree5, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                                }
                                else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 4)
                                {
                                    spriteBatch.Draw(tree4, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                                }
                                else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 3)
                                {
                                    spriteBatch.Draw(tree3, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                                }
                                else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 2)
                                {
                                    spriteBatch.Draw(tree2, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                                }
                                else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 1)
                                {
                                    spriteBatch.Draw(tree1, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                                }
                                else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 0)
                                {
                                    spriteBatch.Draw(tree0, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                                }
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 6)// && tileArray[x, y].land != 5)
                            {
                                spriteBatch.Draw(nodeStone, new Rectangle((x * (int)(50 * tileScale)), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 101)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodHorizontal, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 102)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodVertical, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 103)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodCornerLeft, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 104)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodCornerRight, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 105)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodBackLeft, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 106)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodBackRight, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 201)// && tileArray[x, y].land != 7)
                            {
                                spriteBatch.Draw(house_kame, new Rectangle((x - 1) * (int)(50 * tileScale), (y - 2) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(150 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 202)// && tileArray[x, y].land != 7)
                            {
                                spriteBatch.Draw(mine, new Rectangle((x - 1) * (int)(50 * tileScale), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 2)
                        {
                            if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 3)// && tileArray[x, y].land != 3)
                            {
                                spriteBatch.Draw(snowBush, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 4)// && tileArray[x, y].land != 4)
                            {
                                Vector2 location = new Vector2(x * (int)(50 * tileScale) + (int)(25 * tileScale) + 1, y * (int)(50 * tileScale) + (int)(25 * tileScale) + 1);
                                Rectangle sourceRectangle = new Rectangle(0, 0, 50, 50);
                                Vector2 origin = new Vector2(25, 25);
                                CreatureAI(rnd.Next(0, 1000), tempTilePointerX + x, tempTilePointerY + y);
                                spriteBatch.Draw(snowDeer, location, sourceRectangle, Color.White, angle, origin, 1.0f * (float)tileScale, SpriteEffects.None, 1);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 5)// && tileArray[x, y].land != 5)
                            {
                                spriteBatch.Draw(snowTree, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 6)// && tileArray[x, y].land != 5)
                            {
                                spriteBatch.Draw(nodeStone, new Rectangle((x * (int)(50 * tileScale)), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 101)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodHorizontal, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 102)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodVertical, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 103)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodCornerLeft, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 104)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodCornerRight, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 105)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodBackLeft, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 106)// && tileArray[x, y].land != 6)
                            {
                                spriteBatch.Draw(WallWoodBackRight, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 201)// && tileArray[x, y].land != 7)
                            {
                                spriteBatch.Draw(house_kame, new Rectangle((x - 1) * (int)(50 * tileScale), (y - 2) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(150 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].land == 202)// && tileArray[x, y].land != 7)
                            {
                                spriteBatch.Draw(mine, new Rectangle((x - 1) * (int)(50 * tileScale), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                        }
                        else
                        {
                            spriteBatch.Draw(player, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        if (landArray[tempTilePointerX + x, tempTilePointerY + y].IsOccupied == true)
                        {
                            spriteBatch.Draw(player, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        //tileArray[x, y].biome = landArray[tempTilePointerX + x, tempTilePointerY + y].biome;
                        //tileArray[x, y].land = landArray[tempTilePointerX + x, tempTilePointerY + y].land;
                        //tileArray[x, y].depth[0] = landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0];
                    }
                }
            }



        */






        // new //

        /*
        
         */





    }
}
