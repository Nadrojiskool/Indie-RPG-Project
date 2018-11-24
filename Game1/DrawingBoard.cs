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
        public List<int> Tree = new List<int>();
        public List<int> Deer = new List<int>();
        public static Texture2D[,,] Tiles = new Texture2D[1000, 5, 10];

        public static void DrawObjects(Texture2D spr, Vector2 location, double scale, float rotate, Rectangle sourceRectangle)
        {
            Game1.spriteBatch.Draw(spr, location, sourceRectangle, Color.White, rotate, new Vector2(25, 25), 1.0f*(float)scale, SpriteEffects.None, 1);
        }
    }
}
