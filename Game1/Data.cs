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
    public class Data : Game1
    {
        public static void Save()
        {
            //informationToWriteLand = new String[1000000];
            String[] informationToWriteBiome = new String[1000000];
            String[] informationToWriteMod = new String[1000000];
            String[] informationToWritePlayerResources = new String[1000];
            String[] informationToWritePlayerStats = new String[200];
            String[] informationToWritePlayerWorkers = new String[Player.Workers.Count * 200];
            int[] array = new int[200];
            int counter = 0;
            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    //informationToWriteLand[counter + x] = landArray[x, y].land.ToString();
                    informationToWriteBiome[counter + x] = landArray[x, y].biome.ToString();
                    informationToWriteMod[counter + x] = landArray[x, y].land.ToString();
                    if (counter == 0)
                    {
                        informationToWritePlayerResources[x] = Player.player.resources[x].ToString();
                        if (x < 200)
                        {
                            informationToWritePlayerStats[x] = Player.player.Stats[x].ToString();
                        }
                    }
                }
                counter = counter + 1000;
            }

            //Player.player.Workers.CopyTo(informationToWritePlayerWorkers);

            counter = 0;
            for (int y = 0; y < Player.Workers.Count; y++)
            {
                array = Player.Workers[y].Stats;
                for (int x = 0; x < 200; x++)
                {
                    informationToWritePlayerWorkers[counter + x] = array[x].ToString();
                }
                counter = counter + 200;
            }

            //File.WriteAllLines("C:/Users/2/Desktop/test1land.txt", informationToWriteLand); // Change the file path here to where you want it.
            File.WriteAllLines("C:/Users/2/Desktop/test1biome.txt", informationToWriteBiome);
            File.WriteAllLines("C:/Users/2/Desktop/test1mod.txt", informationToWriteMod);
            File.WriteAllLines("C:/Users/2/Desktop/test1resources.txt", informationToWritePlayerResources);
            File.WriteAllLines("C:/Users/2/Desktop/test1stats.txt", informationToWritePlayerStats);
            File.WriteAllLines("C:/Users/2/Desktop/test1workers.txt", informationToWritePlayerWorkers);

            GC.Collect();
        }

        public static void Load()
        {
            String[] informationToWriteBiome = new String[1000000];
            String[] informationToWriteMod = new String[1000000];
            String[] informationToWritePlayerResources = new String[1000];
            String[] informationToWritePlayerStats = new String[200];
            informationToWriteBiome = File.ReadAllLines("C:/Users/2/Desktop/test1biome.txt");
            informationToWriteMod = File.ReadAllLines("C:/Users/2/Desktop/test1mod.txt");
            informationToWritePlayerResources = File.ReadAllLines("C:/Users/2/Desktop/test1resources.txt");
            informationToWritePlayerStats = File.ReadAllLines("C:/Users/2/Desktop/test1stats.txt");
            int[] array = new int[200];
            int counter = 0;

            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    //landArray[x, y].land = Int32.Parse(informationToWriteLand[counter + x]);
                    landArray[x, y].biome = Int32.Parse(informationToWriteBiome[counter + x]);
                    landArray[x, y].land = Int32.Parse(informationToWriteMod[counter + x]);
                    landArray[x, y].IsActive = false;

                    if (landArray[x, y].land == 5)
                    {
                        landArray[x, y].frame = 5;
                    }

                    if (counter == 0)
                    {
                        Player.player.resources[x] = Int32.Parse(informationToWritePlayerResources[x]);
                        if (x < 200) { Player.player.Stats[x] = Int32.Parse(informationToWritePlayerStats[x]); }
                    }
                }
                counter = counter + 1000;
            }
            counter = 0;

            Player.Workers.Clear();
            Player.LocalWorkers.Clear();
            String[] informationToWritePlayerWorkers = File.ReadAllLines("C:/Users/2/Desktop/test1workers.txt");
            for (int y = 0; y < informationToWritePlayerWorkers.Length / 200; y++)
            {
                for (int x = 0; x < 200; x++)
                {
                    array[x] = Int32.Parse(informationToWritePlayerWorkers[counter + x]);
                }

                Player.Workers.Add(new Unit(0, 0, Player.Workers.Count, array));
                counter = counter + 200;
            }

            GC.Collect();
        }
    }
}
