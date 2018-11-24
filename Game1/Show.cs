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
using System.Net.PeerToPeer;

namespace Game1
{
    public class Show : Game1
    {
        static Random Random = new Random();

        public static void Interface()
        {
            spriteBatch.Begin();

            Tiles();

            DrawingBoard.DrawObjects(player, new Vector2(
                    (Player.player.DrawX),
                    (Player.player.DrawY)),
                tileScale, Player.player.Rotation, new Rectangle(0, 0, 50, 50));

            if (invOpen == true)
            {
                Inventory();
            }
            if (buildMenuOpen == true)
            {
                Blueprints();
            }
            if (workerListOpen == true)
            {
                Workers();
            }

            Text();

            spriteBatch.End();
        }

        static void MainMenu()
        {

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
                    Object obj = Object.Objects[landArray[cameraLocationX + x, cameraLocationY + y].land];

                    if (landArray[cameraLocationX + x, cameraLocationY + y].land != 0)
                    {
                        spriteBatch.Draw(DrawingBoard.Tiles[0, landArray[cameraLocationX + x, cameraLocationY + y].biome, 5],
                            new Rectangle((x * (int)(50 * tileScale)),
                                (y * (int)(50 * tileScale)),
                                (int)(50 * tileScale),
                                (int)(50 * tileScale)),
                            Color.White);
                    }

                    spriteBatch.Draw(texture,
                        new Vector2(
                            (x - ((obj.X / 50) - 1)) * ((int)(50 * tileScale)),
                            (y - ((obj.Y / 50) - 1)) * ((int)(50 * tileScale))),
                        new Rectangle(0, 0, obj.X, obj.Y),
                        Color.White, 0, new Vector2(0, 0),
                        1.0f * (float)tileScale, SpriteEffects.None, 1);
                }
            }
        }

        static void Text(/*string text, int width, int height*/)
        {
            if (actionPending == true)
            {
                spriteBatch.DrawString(font, $"{actionTimer.ElapsedMilliseconds / 1000}", new Vector2(1000, 500), Color.Red);
            }
            if (cantBuild.IsRunning == true)
            {
                string output = "Not Enough Resources!";
                Vector2 FontOrigin = font.MeasureString(output) / 2;
                spriteBatch.DrawString(font, output, new Vector2(1000, 450), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            }
            spriteBatch.DrawString(font, $"{ objland }", new Vector2(50, 50), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ objbiome }", new Vector2(50, 150), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ objframe }", new Vector2(50, 200), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ Object.Objects[201].X }", new Vector2(50, 250), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ Object.Objects[201].Y }", new Vector2(50, 300), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ objwidth }", new Vector2(50, 350), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ objheight }", new Vector2(50, 400), Color.DarkViolet);
            if (Unit.Active.Count > 0)
            {
                int[] array = Unit.Active[0].stats;
                spriteBatch.DrawString(font, $"{ array[0] }", new Vector2(50, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ array[1] }", new Vector2(100, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ array[2] }", new Vector2(150, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ array[3] }", new Vector2(200, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Unit.Active[0].X }", new Vector2(50, 200), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Unit.Active[0].Y }", new Vector2(150, 200), Color.DarkViolet);
            }
        }

        static void Inventory()
        {
            DrawingBoard.DrawObjects(inventory, new Vector2(1400, 200), 5, 0, new Rectangle(0, 0, 122, 174));
            if (newMouseState.RightButton == ButtonState.Pressed)
            {
                DrawingBoard.DrawObjects(idCardBack, new Vector2(50, 150), 1, 0, new Rectangle(0, 0, 1200, 732));
                spriteBatch.DrawString(font, $"Gathering: {Player.player.stats[11]} ", new Vector2(370, 450), Color.Black);
                spriteBatch.DrawString(font, $"Wood Cutting: {Player.player.stats[12]} ", new Vector2(370, 500), Color.Black);
                spriteBatch.DrawString(font, $"Mining: {Player.player.stats[14]}", new Vector2(370, 550), Color.Black);
                spriteBatch.DrawString(font, $"Construction: {Player.player.stats[17] }", new Vector2(370, 600), Color.Black);
                spriteBatch.DrawString(font, $"Hunting: {Player.player.stats[18]}", new Vector2(370, 650), Color.Black);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[11] / 50}", new Vector2(750, 450), Color.Gold);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[12] / 500}", new Vector2(750, 500), Color.Gold);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[14] / 50}", new Vector2(750, 550), Color.Gold);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[17] / 20}", new Vector2(750, 600), Color.Gold);
                spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[18] / 10}", new Vector2(750, 650), Color.Gold);
            }
            else
            {
                DrawingBoard.DrawObjects(idCard, new Vector2(50, 150), 1, 0, new Rectangle(0, 0, 1200, 732));
                spriteBatch.DrawString(font, $"Experience: {Player.player.stats[0]}", new Vector2(50, 450), Color.DarkViolet);
                spriteBatch.DrawString(font, $"Workers: {Player.Units.Count} / {Player.player.resources[10]}", new Vector2(50, 500), Color.Blue);
                spriteBatch.DrawString(font, $"Gold: {Player.player.gold}", new Vector2(50, 550), Color.DarkGoldenrod);
                spriteBatch.DrawString(font, $"Water: {Player.player.resources[2]}", new Vector2(50, 600), Color.DarkBlue);
                spriteBatch.DrawString(font, $"Vines: {Player.player.resources[3]}", new Vector2(50, 650), Color.ForestGreen);
                spriteBatch.DrawString(font, $"Meat: {Player.player.resources[4]}", new Vector2(50, 700), Color.Crimson);
                spriteBatch.DrawString(font, $"Lumber: {Player.player.resources[5]}", new Vector2(50, 750), Color.SaddleBrown);
                spriteBatch.DrawString(font, $"Vitality: {Player.player.stats[1]}", new Vector2(370, 350), Color.Black);
                spriteBatch.DrawString(font, $"Physique: {Player.player.stats[2]}", new Vector2(370, 400), Color.Black);
                spriteBatch.DrawString(font, $"Agility: {Player.player.stats[3]}", new Vector2(370, 450), Color.Black);
                spriteBatch.DrawString(font, $"Combat: {Player.player.stats[4]}", new Vector2(370, 500), Color.Black);
                spriteBatch.DrawString(font, $"Magic: {Player.player.stats[5]}", new Vector2(370, 550), Color.Black);
                spriteBatch.DrawString(font, $"Knowledge: {Player.player.stats[6]}", new Vector2(370, 600), Color.Black);
                spriteBatch.DrawString(font, $"Leadership: {Player.player.stats[7]}", new Vector2(370, 650), Color.Black);
                spriteBatch.DrawString(font, $"Sociability: {Player.player.stats[8]}", new Vector2(370, 700), Color.Black);
                spriteBatch.DrawString(font, $"Expertise: {Player.player.stats[9]}", new Vector2(980, 220), Color.Gold);
            }
        }

        static void Blueprints()
        {
            DrawingBoard.DrawObjects(buildMenu, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 846, 535));
            DrawingBoard.DrawObjects(house_kame, new Vector2(600, 650), 1, 0, new Rectangle(0, 0, 100, 150));
            DrawingBoard.DrawObjects(WallWoodHorizontal, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodVertical, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodCornerLeft, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodCornerRight, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodBackLeft, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(WallWoodBackRight, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 50, 100));
            DrawingBoard.DrawObjects(mine, new Vector2(537, 580), 1, 0, new Rectangle(0, 0, 100, 100));

        }

        static void Workers()
        {
            int[] array = new int[10];
            DrawingBoard.DrawObjects(workerList, new Vector2(50, 50), 1, 0, new Rectangle(0, 0, 510, 825));
            for (int i = 0; i < Player.Units.Count; i++)
            {
                array = Player.Units[i].stats;
                spriteBatch.Draw(player, new Vector2(80, 80 + i * 50), Color.White);
                spriteBatch.DrawString(font, $": {array[0]} | {array[1]} | {array[2]} | {array[3]} | {array[4]} | {array[5]} | " +
                    $"{array[6]} | {array[7]} | {array[8]} | {array[9]}", new Vector2(135, 80 + i * 50), Color.Black);
            }
        }
    }
}
