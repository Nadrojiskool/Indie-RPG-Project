using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

/// <summary>
/// Object IDs
/// </summary>

namespace Game1
{
    public class Object
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //static public Object Inventory;
        //static public Object BuildMenu;
        //static public Object Kame;
        //static public Object OrbPillar;
        //static public Object DefenseWallWood;

        public Object(string name, int x, int y, int width, int height)
        {
            Name = name;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
