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
    /*
     * Final Tile Size to be 60 x 60 for PC.
     * This implies 32 x 18 (16:9) Tiles drawn to a 1080p screen, minimizing resize calculations
     * 
     * Release Version will include ability to adjust Tile Scale to pre-set adjustments
     * Which is likely best optimized by resyncing library to pre-compressed scale sizes
     * Client will initialize MAP-scale asset compression library at launch, stored in parallel
     * 
     * --Pending Optimization
     */

    public class Show : Game1
    {
        static Random Random = new Random();
        static public Object[] Objects = new Object[250];
        static public Object[] InterfaceObjects = new Object[100];

        public static void Initialize()
        {
            Objects[000] = new Object("Zero", 50, 50, 1, 1);
            Objects[001] = new Object("Land", 50, 50, 1, 1);
            Objects[002] = new Object("Water", 50, 50, 1, 1);
            Objects[003] = new Object("Bush", 50, 50, 1, 1);
            Objects[004] = new Object("Deer", 50, 50, 1, 1);
            Objects[005] = new Object("Tree", 100, 100, 1, 1);
            Objects[006] = new Object("Rock", 50, 50, 1, 1);
            Objects[100] = new Object("Obelisk", 50, 100, 1, 1);
            Objects[101] = new Object("Wall Wood Horizontal", 50, 100, 1, 1);
            Objects[102] = new Object("Wall Wood Vertical", 50, 100, 1, 1);
            Objects[103] = new Object("Wall Wood Corner Left", 50, 100, 1, 1);
            Objects[104] = new Object("Wall Wood Corner Right", 50, 100, 1, 1);
            Objects[105] = new Object("Wall Wood Back Left", 50, 100, 1, 1);
            Objects[106] = new Object("Wall Wood Back Right", 50, 100, 1, 1);
            Objects[200] = new Object("Cabin", 100, 100, 2, 2);
            Objects[201] = new Object("Kame House", 100, 150, 2, 2);
            Objects[202] = new Object("Mine", 100, 100, 2, 2);
        }

        public static void Interface()
        {
            Tiles();

            DrawingBoard.DrawObjects(player, new Vector2(
                    (Player.player.DrawX),
                    (Player.player.DrawY)),
                tileScale, Player.player.Rotation, new Rectangle(0, 0, 50, 50));
            LocalUnits(Player.LocalWorkers, Player.LocalEnemies);

            if (invOpen == true) {
                Inventory(); }

            if (buildMenuOpen == true) {
                Blueprints(); }

            if (workerListOpen == true) {
                WorkerList(); }

            Text();
        }

        public static void MainMenu()
        {
            spriteBatch.Draw(BGFinalFantasy,
                new Rectangle(0, 0, 1920, 1080),
                Color.White);
        }

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
         * --Pending 60x60 Optimization
         */

        static void Tiles()
        {
            double rnd;

            for (int y = 0; y < ((displayHeight / 50) / tileScale) + 2 / tileScale; y++)
            {
                for (int x = 0; x < ((displayWidth / 50) / tileScale) + 2 / tileScale; x++)
                {
                    /*rnd = Random.Next(0, 10000);
                    if (rnd == 5 || rnd == 9995 && landArray[cameraLocationX + x, cameraLocationY + y].land == 5)
                    {
                        if (landArray[cameraLocationX + x, cameraLocationY + y].frame < 5 && rnd > 5000)
                        {
                            landArray[cameraLocationX + x, cameraLocationY + y].frame = Check.Max(landArray[cameraLocationX + x, cameraLocationY + y].frame + 1, 5);
                            landArray[cameraLocationX + x + 1, cameraLocationY + y].frame = Check.Max(landArray[cameraLocationX + x + 1, cameraLocationY + y].frame + 1, 5);
                            landArray[cameraLocationX + x, cameraLocationY + y + 1].frame = Check.Max(landArray[cameraLocationX + x, cameraLocationY + y + 1].frame + 1, 5);
                        }
                        else if (landArray[cameraLocationX + x, cameraLocationY + y].frame > 0 && rnd < 5000)
                        {
                            rnd = Random.Next(3, 6);
                            for (int i = 1; i < rnd; i++)
                            {
                                landArray[cameraLocationX + x, cameraLocationY + y].frame = Check.Min(landArray[cameraLocationX + x, cameraLocationY + y].frame - 1, 0);
                                landArray[cameraLocationX + x + i, cameraLocationY + y].frame = Check.Min(landArray[cameraLocationX + x + i, cameraLocationY + y].frame - 1, 0);
                                landArray[cameraLocationX + x, cameraLocationY + y + i].frame = Check.Min(landArray[cameraLocationX + x, cameraLocationY + y + i].frame - 1, 0);
                                landArray[cameraLocationX + x + i, cameraLocationY + y + i].frame = Check.Min(landArray[cameraLocationX + x + i, cameraLocationY + y + i].frame - 1, 0);
                            }
                        }
                    }*/


                    Texture2D texture = DrawingBoard.Tiles[landArray[cameraLocationX + x, cameraLocationY + y].land,
                            landArray[cameraLocationX + x, cameraLocationY + y].biome,
                            landArray[cameraLocationX + x, cameraLocationY + y].frame];
                    Object obj = Objects[landArray[cameraLocationX + x, cameraLocationY + y].land];

                    // Note of Intense Calculation in Visual Rendering Method // 
                    // Potential Optimization in Indexing or Off-Loading Redundant Math 
                    // Example: int modifiedTileScale = (int)(50 * tileScale);

                    int modifiedTileScale = (int)(50 * tileScale);
                    Land land = landArray[cameraLocationX + x, cameraLocationY + y];

                    if (land.land != 0)
                    {
                        spriteBatch.Draw(DrawingBoard.Tiles[0, land.biome, 5],
                            new Rectangle((x * modifiedTileScale), (y * modifiedTileScale),
                                modifiedTileScale, modifiedTileScale),
                            Color.White);
                    }

                    float fl = 1.0f;
                    Vector2 origin = new Vector2(0, 0);
                    if (land.land == 5) {
                        origin.X = -12;
                        origin.Y = 8;
                        fl = 2.0f; }
                    
                    spriteBatch.Draw(texture,
                        new Vector2((x - ((obj.X / 50) - 1)) * (modifiedTileScale),
                            (y - ((obj.Y / 50) - 1)) * (modifiedTileScale)),
                        new Rectangle(0, 0, obj.X, obj.Y),
                        Color.White, 0, origin,
                        fl * (float)tileScale, SpriteEffects.None, 1);
                }
            }
        }

        static void Text(/*string text, int width, int height*/)
        {
            if (actionPending == true) {
                spriteBatch.DrawString(font, $"{actionTimer.ElapsedMilliseconds / 1000}", new Vector2(1000, 500), Color.Red); }

            if (cantBuild.IsRunning == true) {
                string output = "Not Enough Resources!";
                Vector2 FontOrigin = font.MeasureString(output) / 2;
                spriteBatch.DrawString(font, output, new Vector2(1000, 450), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f); }

            if (Player.LocalEnemies.Count > 0) {
                //int[] array = Player.LocalEnemies[0].Stats;
                spriteBatch.DrawString(font, $"{ Player.player.X }", new Vector2(50, 10), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.player.Y }", new Vector2(100, 10), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].LeftOrRight }", new Vector2(50, 50), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].ActionID }", new Vector2(100, 50), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].Stats[1] }", new Vector2(150, 50), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].X }", new Vector2(50, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].Y }", new Vector2(100, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].DestinationOffset[0] }", new Vector2(50, 150), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].DestinationOffset[1] }", new Vector2(100, 150), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].OriginOffset[0] }", new Vector2(50, 200), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].OriginOffset[1] }", new Vector2(100, 200), Color.DarkViolet);
            }

            spriteBatch.DrawString(font, $"Total Enemies: { Player.Enemies.Count() }", new Vector2(50, 900), Color.Red);
            spriteBatch.DrawString(font, $"Total Active Enemies: { Player.LocalEnemies.Count() }", new Vector2(50, 950), Color.Red);
            spriteBatch.DrawString(font, $"{ Player.player.Stats[1] }", new Vector2(50, 1000), Color.Red);
            spriteBatch.DrawString(font, $"{ Player.player.Stats[2] }", new Vector2(125, 1000), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ Player.player.Stats[3] }", new Vector2(200, 1000), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ testHP }//{ testVit }//{ testPhy }", new Vector2(300, 1000), Color.DarkViolet);
        }

        static void Inventory()
        {
            DrawingBoard.DrawObjects(inventory, new Vector2(1400, 200), 5, 0, new Rectangle(0, 0, 122, 174));
            if (newMouseState.RightButton == ButtonState.Pressed)
            {
                DrawingBoard.DrawObjects(idCardBack, new Vector2(50, 150), 1, 0, new Rectangle(0, 0, 1200, 732));
                spriteBatch.DrawString(font, $"Gathering: {Player.player.Stats[11]} ", new Vector2(370, 450), Color.Black);
                spriteBatch.DrawString(font, $"Wood Cutting: {Player.player.Stats[12]} ", new Vector2(370, 500), Color.Black);
                spriteBatch.DrawString(font, $"Mining: {Player.player.Stats[14]}", new Vector2(370, 550), Color.Black);
                spriteBatch.DrawString(font, $"Construction: {Player.player.Stats[17] }", new Vector2(370, 600), Color.Black);
                spriteBatch.DrawString(font, $"Hunting: {Player.player.Stats[18]}", new Vector2(370, 650), Color.Black);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.Stats[11] / 50}", new Vector2(750, 450), Color.Gold);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.Stats[12] / 500}", new Vector2(750, 500), Color.Gold);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.Stats[14] / 50}", new Vector2(750, 550), Color.Gold);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.Stats[17] / 20}", new Vector2(750, 600), Color.Gold);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.Stats[18] / 10}", new Vector2(750, 650), Color.Gold);
            }
            else
            {
                DrawingBoard.DrawObjects(idCard, new Vector2(50, 150), 1, 0, new Rectangle(0, 0, 1200, 732));
                spriteBatch.DrawString(font, $"Experience: {Player.player.Stats[10]}", new Vector2(50, 450), Color.DarkViolet);
                spriteBatch.DrawString(font, $"Workers: {Player.Workers.Count} / {Player.player.resources[10]}", new Vector2(50, 500), Color.Blue);
                spriteBatch.DrawString(font, $"Gold: {Player.player.gold}", new Vector2(50, 550), Color.DarkGoldenrod);
                spriteBatch.DrawString(font, $"Water: {Player.player.resources[2]}", new Vector2(50, 600), Color.DarkBlue);
                spriteBatch.DrawString(font, $"Vines: {Player.player.resources[3]}", new Vector2(50, 650), Color.ForestGreen);
                spriteBatch.DrawString(font, $"Meat: {Player.player.resources[4]}", new Vector2(50, 700), Color.Crimson);
                spriteBatch.DrawString(font, $"Lumber: {Player.player.resources[5]}", new Vector2(50, 750), Color.SaddleBrown);
                spriteBatch.DrawString(font, $"Vitality: {Player.player.Stats[11]}", new Vector2(370, 350), Color.Black);
                spriteBatch.DrawString(font, $"Physique: {Player.player.Stats[12]}", new Vector2(370, 400), Color.Black);
                spriteBatch.DrawString(font, $"Agility: {Player.player.Stats[13]}", new Vector2(370, 450), Color.Black);
                spriteBatch.DrawString(font, $"Combat: {Player.player.Stats[14]}", new Vector2(370, 500), Color.Black);
                spriteBatch.DrawString(font, $"Magic: {Player.player.Stats[15]}", new Vector2(370, 550), Color.Black);
                spriteBatch.DrawString(font, $"Knowledge: {Player.player.Stats[16]}", new Vector2(370, 600), Color.Black);
                spriteBatch.DrawString(font, $"Leadership: {Player.player.Stats[17]}", new Vector2(370, 650), Color.Black);
                spriteBatch.DrawString(font, $"Sociability: {Player.player.Stats[18]}", new Vector2(370, 700), Color.Black);
                spriteBatch.DrawString(font, $"Expertise: {Player.player.Stats[19]}", new Vector2(980, 220), Color.Gold);
            }
        }

        static void Blueprints()
        {
            DrawingBoard.DrawObjects(buildMenu, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 846, 535));
            DrawingBoard.DrawObjects(house_kame, new Vector2(600, 650), 1, 0, new Rectangle(0, 0, 100, 150));
            DrawingBoard.DrawObjects(WallWoodHorizontal, new Vector2(750, 650), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodVertical, new Vector2(800, 650), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodCornerLeft, new Vector2(850, 650), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodCornerRight, new Vector2(900, 650), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodBackLeft, new Vector2(950, 650), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodBackRight, new Vector2(1000, 650), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(mine, new Vector2(1050, 650), 1, 0, new Rectangle(0, 0, 100, 100));
            DrawingBoard.DrawObjects(cabin1, new Vector2(1150, 650), 1, 0, new Rectangle(0, 0, 100, 100));
            spriteBatch.DrawString(font, $"Spawn Camp", new Vector2(600, 980), Color.DarkViolet);
            spriteBatch.DrawString(font, $"Spawn Village", new Vector2(900, 980), Color.DarkViolet);

        }

        static void WorkerList()
        {
            int[] array = new int[10];
            DrawingBoard.DrawObjects(workerList, new Vector2(50, 50), 1, 0, new Rectangle(0, 0, 510, 825));
            for (int i = 0; i < Player.Workers.Count; i++)
            {
                array = Player.Workers[i].Stats;
                spriteBatch.Draw(player, new Vector2(80, 80 + i * 50), Color.White);
                spriteBatch.DrawString(font, $": {array[0]} | {array[1]} | {array[2]} | {array[3]} | {array[4]} | {array[5]} | " +
                    $"{array[6]} | {array[7]} | {array[8]} | {array[9]}", new Vector2(135, 80 + i * 50), Color.Black);
            }
        }

        static void LocalUnits(List<Unit> localWorkers, List<Unit> localEnemies)
        {
            if (localWorkers.Count > 0)
            {
                foreach (Unit unit in localWorkers)
                {
                    int modifiedTileScale = (int)(50 * tileScale);
                    int x = Check.Range((Player.player.DrawX - ((Player.player.X - unit.X) * modifiedTileScale)), modifiedTileScale, (int)(1920 - modifiedTileScale));
                    int y = Check.Range((Player.player.DrawY - ((Player.player.Y - unit.Y) * modifiedTileScale)), modifiedTileScale, (int)(1080 - modifiedTileScale));
                    DrawingBoard.DrawObjects(player, new Vector2(x, y), tileScale, unit.Rotation, new Rectangle(0, 0, 50, 50));
                    DrawingBoard.DrawObjects(DrawingBoard.HPBar[0], new Vector2(x, y + modifiedTileScale), tileScale, 0, new Rectangle(0, 0, 50, 10));
                    DrawingBoard.DrawObjects(DrawingBoard.HPBar[1], new Vector2(x, y + modifiedTileScale + (int)(2 * tileScale)), tileScale, 0, new Rectangle(0, 0, 50, 6));
                }
            }

            if (localEnemies.Count > 0)
            {
                foreach (Unit unit in localEnemies)
                {
                    int modifiedTileScale = (int)(50 * tileScale);
                    int x = Check.Range((Player.player.DrawX - ((Player.player.X - unit.X) * modifiedTileScale)), modifiedTileScale, (int)(1920 - modifiedTileScale));
                    int y = Check.Range((Player.player.DrawY - ((Player.player.Y - unit.Y) * modifiedTileScale)), modifiedTileScale, (int)(1080 - modifiedTileScale));
                    DrawingBoard.DrawObjects(DrawingBoard.Enemies[0, unit.LastMove, 0], new Vector2(x, y), tileScale, 0, new Rectangle(0, 0, 50, 50));
                    DrawingBoard.DrawObjects(DrawingBoard.HPBar[0], new Vector2(x, y + modifiedTileScale), tileScale, 0, new Rectangle(0, 0, 50, 10));
                    int maxHP = 10 * (2 + unit.Stats[11] + unit.Stats[12]);
                    spriteBatch.Draw(DrawingBoard.HPBar[1], 
                            new Rectangle(x - (modifiedTileScale / 2), y - (modifiedTileScale / 2) + modifiedTileScale, 
                            (int)((48 * ((double)unit.Stats[1] / maxHP)) * tileScale), (int)(6 * tileScale)), 
                        Color.White);
                }
            }

            if (Player.Animations.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < Player.Animations.Count(); i++)
                {
                    int modifiedTileScale = (int)(50 * tileScale);
                    Animation animation = Player.Animations[i - count];
                    int x = (Player.player.DrawX - ((Player.player.X - animation.X) * modifiedTileScale));
                    int y = (Player.player.DrawY - ((Player.player.Y - animation.Y) * modifiedTileScale));
                    spriteBatch.Draw(DrawingBoard.Blast[animation.Frame], new Rectangle(x - (modifiedTileScale / 2), y - (modifiedTileScale / 2), 5 * modifiedTileScale, 5 * modifiedTileScale), Color.White);
                    if (animation.Frame == 73)
                    {
                        Player.Animations.RemoveAt(i - count);
                        count += 1;
                    }
                    else
                    {
                        animation.Frame += 1;
                    }
                }
            }
        }
    }
}
