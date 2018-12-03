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
using Game1.Region;

namespace Game1
{
    public class Generate : Game1
    {
        static Random Random = new Random();

        /* Generate.Biome;
         * Input Values: cameraLocationX, cameraLocationY, Land[].biome;
         * Generates random biome scale depending on biome ID
         * while making sure to stay within default array bounds
         * by checking for array scale value which is this max i & ii in the function
         * it will run a number of times i while less than the scale of biome
         * and set tile biome values to the passed biome ID b until to scale
         */

        public static void Biome(int tempCameraX, int tempCameraY, int d, Land[,] landArray)
        {
            double rnd = Random.Next(5, 17);

            if ((tempCameraX > rnd && tempCameraX < 1000 - rnd) && (tempCameraY > rnd && tempCameraY < 1000 - rnd))
            {
                for (int i = 0; i < rnd; i++)
                {
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraX++;
                        landArray[tempCameraX, tempCameraY].biome = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraY++;
                        landArray[tempCameraX, tempCameraY].biome = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraX--;
                        landArray[tempCameraX, tempCameraY].biome = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraY--;
                        landArray[tempCameraX, tempCameraY].biome = d;
                    }
                    tempCameraX--;
                    tempCameraY--;
                }
            }
        }

        public static void Land(int tempCameraX, int tempCameraY, int d, Land[,] landArray)
        {
            double rnd = Random.Next(1, 5);

            if ((tempCameraX > rnd && tempCameraX < 1000 - rnd) && (tempCameraY > rnd && tempCameraY < 1000 - rnd))
            {
                for (int i = 0; i < rnd; i++)
                {
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraX++;
                        landArray[tempCameraX, tempCameraY].land = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraY++;
                        landArray[tempCameraX, tempCameraY].land = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraX--;
                        landArray[tempCameraX, tempCameraY].land = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraY--;
                        landArray[tempCameraX, tempCameraY].land = d;
                    }
                    tempCameraX--;
                    tempCameraY--;
                }
            }
        }

        public static async Task All()
        {
            double rnd;
            byte biomeHolder;
            Land checkPosX;
            Land checkPosY;
            Land checkNegX;
            Land checkNegY;
            Land checkTile;

            /*
             * The following statements will populate the land values of the default array 1000 x 1000 
             * as well as the tile cache array currently 400 x 250 which will
             * support the tile scale .1 (10x) on most resolution scales
             * ^^--pending optimization--^^
             * a roll is done to determine some land values as well as if the tile will be a new biome
             * and then check the previously drawn X & Y tiles for water or trees to increase clumping
             * before going on to generate full biomes, to scale 
             * the biomes are then expanded on with more complex near-by random tile generation 
             * done through multiple rescans of the default global land array for definition
             * default snow i = 40
             * default ore i = 5
             */

            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    rnd = Random.Next(0, 10000);

                    /////////////////////////
                    // New Biome Initiator //
                    /////////////////////////

                    if (rnd < 5) { biomeHolder = 0; }
                    else { biomeHolder = 1; }

                    /////////////////////////////////////////////////////////////
                    // Randomly Generate and Instantiate Land Stored in Arrays //
                    /////////////////////////////////////////////////////////////

                    landArray[x, y] = new Land();
                    landArray[x, y].biome = biomeHolder;

                    if (rnd <= 9390)
                    {
                        landArray[x, y].land = 0;
                    }
                    else if (rnd > 9390 && rnd <= 9400)
                    {
                        landArray[x, y].land = 2;
                    }
                    else if (rnd > 9400 && rnd <= 9800)
                    {
                        landArray[x, y].land = 3;
                    }
                    else if (rnd > 9800 && rnd <= 9900)
                    {
                        landArray[x, y].land = 4;
                    }
                    else if (rnd > 9900 && rnd <= 9995)
                    {
                        landArray[x, y].land = 5;
                    }
                    else if (rnd > 9995)
                    {
                        landArray[x, y].land = -6;
                    }

                    /////////////////////
                    // Realistic Water //
                    /////////////////////

                    if (landArray[Check.Min(x - 1, 0), y].land == 2 && rnd < 5500)
                    {
                        landArray[x, y].land = 2;

                    }
                    else if (landArray[x, Check.Min(y - 1, 0)].land == 2 && rnd < 5500)
                    {
                        landArray[x, y].land = 2;
                    }
                    else if (landArray[x, Check.Min(y - 1, 0)].land == 2 && landArray[Check.Min(x - 1, 0), y].land == 2 && rnd < 9920)
                    {
                        landArray[x, y].land = 2;
                    }

                    ///////////////////////
                    // Realistic Forests //
                    ///////////////////////

                    if (landArray[Check.Min(x - 1, 0), y].land == 5 && rnd < 3500)
                    {
                        landArray[x, y].land = 5;
                    }
                    else if (landArray[x, Check.Min(y - 1, 0)].land == 5 && rnd < 3500)
                    {
                        landArray[x, y].land = 5;
                    }
                    else if (landArray[x, Check.Min(y - 1, 0)].land == 5 && landArray[Check.Min(x - 1, 0), y].land == 5 && rnd < 9600)
                    {
                        landArray[x, y].land = 5;
                    }

                }
            }

            MainMenuOpen = false;

            /////////////////////
            // Generate Biomes //
            /////////////////////

            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    if (landArray[x, y].biome == 0)
                    {
                        rnd = Random.Next(2, 3);
                        if (rnd == 2)
                        {
                            landArray[x, y].biome = 2;
                        }
                        Biome(x - 1, y - 1, (int)rnd, landArray);
                    }
                    if (landArray[x, y].land == -6)
                    {
                        landArray[x, y].land = 6;
                        Land(x - 1, y - 1, 6, landArray);
                    }
                }
            }


            /////////////////////////////////////
            // Expanded Snow || Default i < 50 //
            /////////////////////////////////////

            for (int i = 0; i < 50; i++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    for (int x = 0; x < 1000; x++)
                    {
                        checkTile = landArray[x, y];
                        checkPosX = landArray[Check.Max(x + 1, 999), y];
                        checkPosY = landArray[x, Check.Max(y + 1, 999)];
                        checkNegX = landArray[Check.Min(x - 1, 0), y];
                        checkNegY = landArray[x, Check.Min(y - 1, 0)];
                        if (checkTile.biome != 2)
                        {
                            rnd = Random.Next(0, 1000);
                            if (checkNegX.biome == 2)
                            {
                                rnd = rnd * 1.25;
                            }
                            if (checkNegY.biome == 2)
                            {
                                rnd = rnd * 1.25;
                            }
                            if (checkPosX.biome == 2)
                            {
                                rnd = rnd * 1.25;
                            }
                            if (checkPosY.biome == 2)
                            {
                                rnd = rnd * 1.25;
                            }
                            if (rnd > 1120)
                            {
                                landArray[x, y].biome = 2;
                            }
                        }
                    }
                }
            }

            ////////////////////////////////////
            // Sparse Nodes || Default i < 5 //
            ////////////////////////////////////

            for (int i = 0; i < 5; i++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    for (int x = 0; x < 1000; x++)
                    {
                        checkTile = landArray[x, y];
                        checkPosX = landArray[Check.Max(x + 1, 999), y];
                        checkPosY = landArray[x, Check.Max(y + 1, 999)];
                        checkNegX = landArray[Check.Min(x - 1, 0), y];
                        checkNegY = landArray[x, Check.Min(y - 1, 0)];
                        if (landArray[x, y].biome != 3)
                        {
                            rnd = Random.Next(0, 1000);
                            if (landArray[Check.Min(x - 1, 0), y].land == 6 && rnd > 650)
                            {
                                landArray[x, y].land = 6;
                            }
                            if (landArray[x, Check.Min(y - 1, 0)].land == 6 && rnd > 650)
                            {
                                landArray[x, y].land = 6;
                            }
                            if (landArray[Check.Max(x + 1, 999), y].land == 6 && rnd > 650)
                            {
                                landArray[x, y].land = 6;
                            }
                            if (landArray[x, Check.Max(y + 1, 999)].land == 6 && rnd > 650)
                            {
                                landArray[x, y].land = 6;
                            }
                        }
                        if (checkTile.land != 5 && checkTile.land != 2)
                        {
                            rnd = Random.Next(0, 1000);
                            if (checkNegX.land == 5)
                            {
                                rnd = rnd * 1.5;
                            }
                            if (checkNegY.land == 5)
                            {
                                rnd = rnd * 1.5;
                            }
                            if (checkPosX.land == 5)
                            {
                                rnd = rnd * 1.5;
                            }
                            if (checkPosY.land == 5)
                            {
                                rnd = rnd * 1.5;
                            }
                            if (rnd > 1050)
                            {
                                checkTile.land = 5;
                            }
                        }
                    }
                }
            }
            Camp();
        }

        public void ExpandBiome(int d)
        {

        }

        public static int[] Worker()
        {
            int[] array = new int[200];
            double rnd;
            array[0] = 0;
            for (int i = 1; i < 10; i++)
            {
                rnd = Random.Next(0, 4);
                array[i] = (int)rnd;
            }
            return (array);
        }

        static public void Camp()
        {
            int[,] array = new int[14, 20] {
                { 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101 },
                { 101, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 001, 101 },
                { 101, 001, 202, 001, 202, 001, 202, 001, 202, 001, 202, 001, 202, 001, 202, 001, 202, 001, 202, 101 },
                { 101, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 101 },
                { 101, 001, 001, 001, 001, 000, 000, 001, 001, 001, 001, 001, 001, 000, 000, 001, 001, 000, 001, 101 },
                { 101, 001, 202, 001, 202, 000, 000, 001, 202, 001, 202, 001, 202, 000, 000, 001, 202, 001, 202, 101 },
                { 101, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 101 },
                { 101, 001, 001, 001, 001, 000, 000, 001, 001, 001, 001, 001, 001, 000, 000, 001, 001, 000, 001, 101 },
                { 101, 001, 202, 001, 202, 000, 000, 001, 202, 001, 202, 001, 202, 000, 000, 001, 202, 001, 202, 101 },
                { 101, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 101 },
                { 101, 001, 001, 001, 001, 000, 000, 001, 001, 001, 001, 001, 001, 000, 000, 001, 001, 000, 001, 101 },
                { 101, 001, 202, 001, 202, 000, 000, 001, 202, 001, 202, 001, 202, 000, 000, 001, 202, 001, 202, 101 },
                { 101, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 000, 101 },
                { 101, 101, 101, 101, 101, 101, 101, 101, 101, 000, 000, 101, 101, 101, 101, 101, 101, 101, 101, 101 }};

            for (int i = 0; i < 14; i++)
            {
                for (int ii = 0; ii < 20; ii++)
                {
                    landArray[50 + ii, 50 + i].land = array[i, ii];
                    landArray[50 + ii, 50 + i].IsHostile = true;
                }
            }

            //LocalManors.Add(new Manor);
        }
    }
}
