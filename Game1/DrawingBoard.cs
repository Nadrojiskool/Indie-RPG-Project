using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Game1
{

    public class DrawingBoard
    {
        public static Texture2D[,,] Tiles = new Texture2D[1000, 5, 10]; // Tile ID, Tile Biome, Tile Frame
        /*
         * 1: Occupied, Unrendered
         * 2: Water
         * 3: Bush
         * 4: Deer
         * 5: Tree
         * 6: Stone
         * 7:
         * 9:
         * 10: Ore 1
         * 11: Ore 2
         * 12: Ore 3
         * 13: Ore 4
         * 14: Ore 5
         * 15: Ore 6
         * 16: Ore 7
         * 17: Ore 8
         * 18: Ore 9
         * 19: Ore 10
         * 20:
         * 99:
         * 100: Campfire
         * 101: Wall
         * 102: Wall
         * 103: Wall
         * 104: Wall
         * 105:
         * 199:
         * 200: Cabin1
         * 201: Kame House
         * 202: Mine
         */
        public static Texture2D[,,] Enemies = new Texture2D[2, 5, 3]; // Enemy ID, Enemy Direction (LastMove), Enemy Frame
        public static Texture2D[,,] Allies = new Texture2D[2, 5, 3]; // Ally ID, Ally Direction (LastMove), Ally Frame
        public static Texture2D[] HPBar = new Texture2D[2];
        public static List<Texture2D[]> Animations = new List<Texture2D[]>();

        public static void DrawObjects(Texture2D spr, Vector2 location, double scale, float rotate, Rectangle sourceRectangle)
        {
            Game1.spriteBatch.Draw(spr, location, sourceRectangle, Color.White, rotate, new Vector2(25, 25), 1.0f*(float)scale, SpriteEffects.None, 1);
        }
    }
}
