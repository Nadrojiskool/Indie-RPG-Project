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
    // Thought: Each Tile is 5ft or 60in which means a single byte TileOffset could store a tile xy +- distance in half inches. 
    // Example: An sbyte can hold +-128 which would mean 120 half inch segments in either direction from the tile draw position with an additional 16 overflow digits.

    /*
     * Final Default Tile Size to be 60 x 60 for PC.
     * This implies 32 x 18 (16:9) Tiles drawn to a 1080p screen, minimizing resize calculations.
     * With each tile representing 5ft (60in), that means each pixel also represents 1 inch.
     * 
     * Release Version will include ability to adjust Tile Scale to pre-set adjustments
     * Which is likely best optimized by resyncing library to pre-compressed scale sizes
     * Client will initialize MAP-scale asset compression library at launch, stored in parallel
     * 
     * --Pending Optimization
     */

    public class Show : Game1
    {
        public static bool CursorOutline = false;
        public static bool ActiveDialogue = false;
        public static int CursorBuilding = 0;
        public static Land CursorLand { get; set; }
        public static Tower CursorTower { get; set; }
        public static Object[] Objects = new Object[1000];
        public static Object[] InterfaceObjects = new Object[100];
        static Random Random = new Random();
        

        public static void Initialize()
        {
            Set.GridSize();
            Set.GridDimensions();

            Objects[000] = new Object("Land", 50, 50, 1, 1);
            Objects[001] = new Object("Index 001", 50, 50, 1, 1);
            Objects[002] = new Object("Water", 50, 50, 1, 1);
            Objects[003] = new Object("Bush", 50, 50, 1, 1);
            Objects[004] = new Object("Deer", 50, 50, 1, 1);
            Objects[005] = new Object("Tree", 100, 100, 1, 1);
            Objects[006] = new Object("Rock", 50, 50, 1, 1);
            Objects[007] = new Object("Plot", 50, 50, 1, 1);
            Objects[010] = new Object("Ore 1", 50, 50, 1, 1);
            Objects[011] = new Object("Ore 2", 50, 50, 1, 1);
            Objects[012] = new Object("Ore 3", 50, 50, 1, 1);
            Objects[013] = new Object("Ore 4", 50, 50, 1, 1);
            Objects[014] = new Object("Ore 5", 50, 50, 1, 1);
            Objects[015] = new Object("Ore 6", 50, 50, 1, 1);
            Objects[016] = new Object("Ore 7", 50, 50, 1, 1);
            Objects[017] = new Object("Ore 8", 50, 50, 1, 1);
            Objects[018] = new Object("Ore 9", 50, 50, 1, 1);
            Objects[019] = new Object("Ore 10", 50, 50, 1, 1);
            Objects[100] = new Object("Campfire", 50, 50, 1, 1);
            Objects[101] = new Object("Wall Wood Horizontal", 50, 100, 1, 1);
            Objects[102] = new Object("Wall Wood Vertical", 50, 100, 1, 1);
            Objects[103] = new Object("Wall Wood Corner Left", 50, 100, 1, 1);
            Objects[104] = new Object("Wall Wood Corner Right", 50, 100, 1, 1);
            Objects[105] = new Object("Wall Wood Back Left", 50, 100, 1, 1);
            Objects[106] = new Object("Wall Wood Back Right", 50, 100, 1, 1);
            Objects[200] = new Object("Cabin", 100, 100, 2, 2);
            Objects[201] = new Object("Kame House", 100, 150, 2, 2);
            Objects[202] = new Object("Mine", 100, 100, 2, 2);
            Objects[300] = new Object("OrbPurple", 50, 50, 1, 1);
            Objects[301] = new Object("Fire Tower", 50, 50, 1, 1);
            Objects[302] = new Object("Ice Tower", 50, 50, 1, 1);
            Objects[303] = new Object("Wind Tower", 50, 50, 1, 1);
            Objects[304] = new Object("Earth Tower", 50, 50, 1, 1);
            Objects[305] = new Object("Lightning Tower", 50, 50, 1, 1);
            Objects[306] = new Object("Water Tower", 50, 50, 1, 1);
            Objects[307] = new Object("Light Tower", 50, 50, 1, 1);
            Objects[308] = new Object("Dark Tower", 50, 50, 1, 1);
            Objects[398] = new Object("Spawner", 50, 50, 1, 1);
            Objects[399] = new Object("Goal", 50, 50, 1, 1);
        }

        public static void Interface()
        {

            // Note that the order dictates that entities are currently always drawn over any tile
            // While this may be a desirable tool, it's not ideal for the default setting
            // The obvious solution is to implement layers, but instead of that, I'd like natural layering
            // This means that each tile needs to store the hash of a unit in the local unit HashSet
            // Then we can simply draw that unit from its cached data

            spriteBatch.Draw(DrawingBoard.Allies[0, Player.player.LastMove, Player.player.AnimationFrame],
                new Rectangle((Player.player.X - cameraLocationX) * CurrentTileSize, (Player.player.Y - cameraLocationY) * CurrentTileSize, CurrentTileSize, CurrentTileSize),
                Color.White);

            LocalUnits(Player.LocalWorkers, Player.LocalEnemies.Values.ToList());

            if (ActiveDialogue) {
                DialogueBox(); }

            if (invOpen) {
                Inventory(); }

            if (buildMenuOpen) {
                Blueprints(); }
            else if (CursorBuilding != 0)
            {
                int x = (int)Math.Ceiling((double)(newMouseState.X - Player.player.TileOffsetXY[0]));
                int y = (int)Math.Ceiling((double)(newMouseState.Y - Player.player.TileOffsetXY[1]));
                Rectangle rect = new Rectangle(
                    x + ((1 - Objects[CursorBuilding].Width) * CurrentTileSize), 
                    y + ((1 - Objects[CursorBuilding].Height) * CurrentTileSize), 
                    CurrentTileSize * (Objects[CursorBuilding].X / 50), 
                    CurrentTileSize * (Objects[CursorBuilding].Y / 50));
                spriteBatch.Draw(DrawingBoard.Tiles[CursorBuilding, 1, 5], rect, Color.White);
            }

            if (workerListOpen) {
                WorkerList(); }

            Text();
            
            if (actionPending == true)
            {
                spriteBatch.Draw(DrawingBoard.HPBar[0], new Rectangle(800, 900, 320, 40), Color.White);
                if (Player.player.ActionID == 254)
                    spriteBatch.Draw(DrawingBoard.HPBar[1], new Rectangle(805, 905, 310 * (Check.LoopInt((int)actionTimer.ElapsedMilliseconds, 1, 2000)) / 2000, 30), Color.White);
                else if (Player.player.ActionID == 255)
                    spriteBatch.Draw(DrawingBoard.HPBar[1], new Rectangle(805, 905, 310 * (1 - ((int)(actionTimer.ElapsedMilliseconds - Player.player.ActionCache) / 1000)), 30), Color.White);
            }
            else if (CursorOutline == true)
            {
                // Note: Need to separate cursor location logic from rendering methods
                spriteBatch.Draw(outline, Check.TileAtCursor(newMouseState), Color.Red);
                spriteBatch.Draw(buildMenu, new Rectangle(1600, 10, 300, 200), Color.White);
                spriteBatch.Draw(DrawingBoard.Tiles[CursorLand.land, CursorLand.biome, 5], new Rectangle(1825, 135, 50, 50), Color.White);
                spriteBatch.DrawString(font, $"{ Objects[CursorLand.land].Name }", new Vector2(1635, 35), Color.DarkViolet);
            }
        }

        public static void MainMenu()
        {
            spriteBatch.Draw(BGFinalFantasy,
                new Rectangle(0, 0, 1920, 1080),
                Color.White);
            spriteBatch.Draw(gray,
                 new Rectangle(860, 440, 200, 50),
                 Color.White);
            spriteBatch.Draw(gray,
                 new Rectangle(860, 640, 200, 50),
                 Color.White);
        }


        public static void Lands()
        {

            for (int y = 0; y < (displayHeight / CurrentTileSize) + 2; y++)
            {
                for (int x = 0; x < (displayWidth / CurrentTileSize) + 2; x++)
                {
                    Land land = landArray[Check.Range(cameraLocationX + x, 0, MapWidth - 1), Check.Range(cameraLocationY + y, 0, MapHeight - 1)];
                    Rectangle tile = TileFrame[x, y];

                    if (land.biome == 1)
                    {
                        int grass = Check.LoopIntPos((cameraLocationX + x) + (cameraLocationY + y), 0, 3);
                        spriteBatch.Draw(DrawingBoard.Biomes[1, 0, grass], tile, Color.White);
                        if (land.IsBorder)
                            spriteBatch.Draw(DrawingBoard.Borders[land.BorderBiome, land.Border], tile, Color.White);
                    }
                    else if (land.biome == 2)
                    {
                        int snow = Check.LoopIntPos((cameraLocationX + x) + (cameraLocationY + y), 0, 2);
                        spriteBatch.Draw(DrawingBoard.Biomes[2, 0, snow], tile, Color.White);
                    }
                    else if (land.biome == 3)
                    {
                        int sandX = Check.LoopIntPos((cameraLocationX + x), 0, 29);
                        int sandY = Check.LoopIntPos((cameraLocationY + y), 0, 19);
                        spriteBatch.Draw(DrawingBoard.Biomes[3, sandX, sandY], tile, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(DrawingBoard.Tiles[0, land.biome, 5], tile, Color.White);
                    }
                }
            }
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

        public static void Tiles()
        {
            double rnd;
            Color color = new Color(Color.Black, 0.2f);

            for (int y = 0; y < (displayHeight / CurrentTileSize) + 2; y++)
            {
                for (int x = 0; x < (displayWidth / CurrentTileSize) + 2; x++)
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

                    ///
                    // I may be eventually make a Rectangle Container Class to just have them hold the tile information
                    // I may also be able to pre-pack tiles into 16:9 chunks?
                    //
                    // I think this is how I'm going to do it:
                    // First of all we need three separate layers; Biome, Unit, Object
                    // We then need to call these in order, likely in different spriteBatch.End() calls
                    // The biome layer and maybe object (building) layer could then be pre-packed (or rendered)
                    
                    Land land = landArray[Check.Range(cameraLocationX + x, 0, MapWidth - 1), Check.Range(cameraLocationY + y, 0, MapHeight - 1)];
                    Texture2D texture = DrawingBoard.Tiles[land.land, land.biome, land.frame];
                    Object obj = Objects[land.land];
                    Rectangle tile = TileFrame[x, y];
                    int[] adjustLand = { 0, 0 };

                    if (land.land != 0)
                    {
                        if (land.land == 5)
                        {
                            int halfTileSize = CurrentTileSize / 2;
                            adjustLand = new int[2] { halfTileSize, -(halfTileSize / 2) };
                        }

                        Rectangle rectangle = new Rectangle(tile.X - (int)((obj.X - 50) * tileScale) + adjustLand[0],
                                tile.Y - (int)((obj.Y - 50) * tileScale) + adjustLand[1],
                                (int)(obj.X * tileScale), (int)(obj.Y * tileScale));

                        if (Math.Abs(Player.player.tileX - x) < 5 && Math.Abs(Player.player.tileY - y) < 5
                            && rectangle.Contains(Player.player.DrawX, Player.player.DrawY))
                        {
                            spriteBatch.Draw(texture, rectangle, color);
                        }
                        else
                            spriteBatch.Draw(texture, rectangle, Color.White);
                    }
                }


                    /*if (land.IsOwned) {
                        spriteBatch.Draw(outline, tile, Color.White); }*/

                    /*float fl = 1.0f;
                    Vector2 origin = new Vector2(0, 0);
                    if (land.land == 5) {
                        origin.X = -12;
                        origin.Y = 8;
                        fl = 2.0f; }*/

                    /*spriteBatch.Draw(texture,
                        new Vector2((x - ((obj.X / 50) - 1)) * (CurrentTileSize) + Player.player.TileOffsetXY[0],
                            (y - ((obj.Y / 50) - 1)) * (CurrentTileSize) + Player.player.TileOffsetXY[1]),
                        new Rectangle(0, 0, obj.X, obj.Y),
                        Color.White, 0, origin,
                        fl * (float)tileScale, SpriteEffects.None, 1);*/

                    //spriteBatch.DrawString(TileInfoFont, Objects[land.land].Name, new Vector2(x * modifiedTileScale, y * modifiedTileScale), Color.Black);
                
            }
        }

        static void Text(/*string text, int width, int height*/)
        {
            for (int i = 0; i < DrawingBoard.Text.Count; i++)
            {
                spriteBatch.DrawString(font, DrawingBoard.Text[i], new Vector2(1000, 10 + (i * 30)), Color.AntiqueWhite);
            }

            if (cantBuild.IsRunning == true) {
                string output = "Not Enough Resources!";
                Vector2 FontOrigin = font.MeasureString(output) / 2;
                spriteBatch.DrawString(font, output, new Vector2(1000, 450), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f); }

            /*if (Player.LocalEnemies.Count > 0) {
                //int[] array = Player.LocalEnemies[0].Stats;
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].LeftOrRight }", new Vector2(50, 50), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].ActionID }", new Vector2(100, 50), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].Stats[1] }", new Vector2(150, 50), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].X }", new Vector2(50, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].Y }", new Vector2(100, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].DestinationOffset[0] }", new Vector2(50, 150), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].DestinationOffset[1] }", new Vector2(100, 150), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].OriginOffset[0] }", new Vector2(50, 200), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Player.LocalEnemies[0].OriginOffset[1] }", new Vector2(100, 200), Color.DarkViolet);
            }*/

            spriteBatch.DrawString(font, $"{ Player.player.X }", new Vector2(50, 10), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ Player.player.Y }", new Vector2(150, 10), Color.DarkViolet);

            spriteBatch.DrawString(font, $"Total Enemies: { Player.Enemies.Count() }", new Vector2(50, 900), Color.Red);
            spriteBatch.DrawString(font, $"Total Active Enemies: { Player.LocalEnemies.Count() }", new Vector2(50, 950), Color.Red);
            spriteBatch.DrawString(font, $"{ Player.player.Stats[1] }", new Vector2(50, 1000), Color.Red);
            spriteBatch.DrawString(font, $"{ Player.player.Stats[2] }", new Vector2(125, 1000), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ Player.player.Stats[3] }", new Vector2(200, 1000), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ Player.player.TileOffsetXY[0] }", new Vector2(300, 1000), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ framerate }", new Vector2(1850, 1000), Color.Red);
            //spriteBatch.DrawString(font, $"{ testHP }//{ testVit }//{ testPhy }", new Vector2(300, 1000), Color.DarkViolet);
        }

        static void ChatBox()
        {
            //spriteBatch.Draw(new Texture2D(graphics, 600, 400), new Vector2(1320, 680), Color.White);
        }

        static void DialogueBox()
        {
            spriteBatch.Draw(boxPink, new Rectangle(600, 600, 720, 405), Color.White);
            spriteBatch.DrawString(font, Check.WrapText("How convenient that somebody left this Hoe here.", 600), new Vector2(670, 620), Color.Black);
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
                //spriteBatch.DrawString(font, $"Gold: {Player.player.gold}", new Vector2(50, 550), Color.DarkGoldenrod);
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
            spriteBatch.Draw(DrawingBoard.Tiles[100, 1, 5], new Vector2(1250, 600), Color.White);
            spriteBatch.Draw(DrawingBoard.Tiles[300, 1, 5], new Vector2(600, 850), Color.White);
            spriteBatch.Draw(player, new Vector2(700, 850), Color.Red);
            spriteBatch.Draw(player, new Vector2(800, 850), Color.Teal);
            spriteBatch.Draw(player, new Vector2(900, 850), Color.LightGray);
            spriteBatch.Draw(player, new Vector2(1000, 850), Color.Brown);
            spriteBatch.Draw(player, new Vector2(1100, 850), Color.Yellow);
            spriteBatch.Draw(player, new Vector2(1200, 850), Color.Blue);
            spriteBatch.Draw(player, new Vector2(1300, 850), Color.White);
            spriteBatch.Draw(player, new Vector2(1400, 850), Color.Black);
            spriteBatch.Draw(enemy, new Vector2(600, 950), Color.White);
            spriteBatch.Draw(player, new Vector2(700, 950), Color.Black);
            spriteBatch.DrawString(font, $"Spawn Camp", new Vector2(500, 980), Color.DarkViolet);
            spriteBatch.DrawString(font, $"Spawn Village", new Vector2(700, 980), Color.DarkViolet);
            spriteBatch.DrawString(font, $"Spawn Bonfire", new Vector2(900, 980), Color.DarkViolet);

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
                    int x = Check.Range((Player.player.DrawX - ((Player.player.X - unit.X) * CurrentTileSize)), CurrentTileSize, (int)(1920 - CurrentTileSize));
                    int y = Check.Range((Player.player.DrawY - ((Player.player.Y - unit.Y) * CurrentTileSize)), CurrentTileSize, (int)(1080 - CurrentTileSize));
                    spriteBatch.Draw(DrawingBoard.Allies[2, unit.LastMove, unit.AnimationFrame], 
                        new Rectangle(x - (CurrentTileSize / 2), y - (CurrentTileSize / 2), CurrentTileSize, CurrentTileSize), Color.White);
                    DrawingBoard.DrawObjects(DrawingBoard.HPBar[0], new Vector2(x, y + CurrentTileSize), tileScale, 0, new Rectangle(0, 0, 50, 10));
                    DrawingBoard.DrawObjects(DrawingBoard.HPBar[1], new Vector2(x, y + CurrentTileSize + (int)(2 * tileScale)), tileScale, 0, new Rectangle(0, 0, 50, 6));
                }
            }

            if (localEnemies.Count > 0)
            {
                foreach (Unit unit in localEnemies)
                {
                    int x = Check.Range((Player.player.DrawX - ((Player.player.X - unit.X) * CurrentTileSize)), CurrentTileSize, (int)(1920 - CurrentTileSize));
                    int y = Check.Range((Player.player.DrawY - ((Player.player.Y - unit.Y) * CurrentTileSize)), CurrentTileSize, (int)(1080 - CurrentTileSize));
                    int x2 = Check.Range(Player.player.tileX - (Player.player.X - unit.X), 0, (displayWidth / CurrentTileSize));
                    int y2 = Check.Range(Player.player.tileY - (Player.player.Y - unit.Y), 0, (displayHeight / CurrentTileSize));
                    spriteBatch.Draw(DrawingBoard.Enemies[0, unit.LastMove, 0], TileFrame[x2, y2], Color.White);
                    //DrawingBoard.DrawObjects(DrawingBoard.Enemies[0, unit.LastMove, 0], new Vector2(x, y), tileScale, 0, new Rectangle(0, 0, 50, 50));
                    DrawingBoard.DrawObjects(DrawingBoard.HPBar[0], new Vector2(x, y + CurrentTileSize), tileScale, 0, new Rectangle(0, 0, 50, 10));
                    int maxHP = (2 + unit.Stats[11] + unit.Stats[12]);
                    spriteBatch.Draw(DrawingBoard.HPBar[1], 
                            new Rectangle(x - (CurrentTileSize / 2), y - (CurrentTileSize / 2) + CurrentTileSize, 
                            (int)((48 * ((double)unit.Stats[1] / maxHP)) * tileScale), (int)(6 * tileScale)), 
                        Color.White);
                }
            }

            /*if (Player.Animations.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < Player.Animations.Count(); i++)
                {
                    Animation animation = Player.Animations[i - count];
                    int x = (Player.player.DrawX - ((Player.player.X - animation.X) * CurrentTileSize));
                    int y = (Player.player.DrawY - ((Player.player.Y - animation.Y) * CurrentTileSize));
                    spriteBatch.Draw(DrawingBoard.Animations[animation.ID][animation.Frame], new Rectangle(x - (CurrentTileSize / 2), y - (CurrentTileSize / 2), 5 * CurrentTileSize, 5 * CurrentTileSize), Color.White);

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
            }*/

            if (Player.WorldItems.Count > 0)
            {
                foreach (KeyValuePair<GPS, int> item in Player.WorldItems)
                {
                    int x = (item.Key.X - cameraLocationX) * CurrentTileSize;
                    int y = (item.Key.Y - cameraLocationY) * CurrentTileSize;
                    spriteBatch.Draw(DrawingBoard.Items[item.Value], new Rectangle(x, y, CurrentTileSize, CurrentTileSize), Color.White);
                }
            }
        }
    }
}
