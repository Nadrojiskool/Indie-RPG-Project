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

        public static void DrawObjects(Texture2D spr, Object obj, double scale, float rotate, Rectangle sourceRectangle)
        {
            Vector2 location = new Vector2(obj.DrawX + 1, obj.DrawY + 1);
            Vector2 origin = new Vector2(25, 25);
            Game1.spriteBatch.Draw(spr, location, sourceRectangle, Color.White, rotate, origin, 1.0f*(float)scale, SpriteEffects.None, 1);
        }
    }
}
