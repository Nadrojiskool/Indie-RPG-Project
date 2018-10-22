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

/* 0:
 * 1: Inventory, 2: Build Menu, 3: Kame House, 4: Obelisk
 * 
 * 
 * 
 * 
 * 
 */

namespace Game1
{
    public class Object
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //static public Object Inventory;
        //static public Object BuildMenu;
        //static public Object Kame;
        //static public Object OrbPillar;
        //static public Object DefenseWallWood;
        static public Object[] objects = new Object[1000];

        public Object(string name, int x, int y, int width, int height)
        {
            Name = name;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static void Initialize()
        {
            objects[100] = new Object("Obelisk", 0, 0, 1, 1);
            objects[101] = new Object("Wall Wood Horizontal", 0, 0, 1, 1);
            objects[102] = new Object("Wall Wood Vertical", 0, 0, 1, 1);
            objects[103] = new Object("Wall Wood Corner Left", 0, 0, 1, 1);
            objects[104] = new Object("Wall Wood Corner Right", 0, 0, 1, 1);
            objects[105] = new Object("Wall Wood Back Left", 0, 0, 1, 1);
            objects[106] = new Object("Wall Wood Back Right", 0, 0, 1, 1);
            objects[201] = new Object("Kame House", 0, 0, 2, 2);
            objects[202] = new Object("Mine", 0, 0, 2, 2);
            objects[901] = new Object("Inventory", 1400, 200, 0, 0);
            objects[902] = new Object("Build Menu", 537, 580, 0, 0);
            objects[903] = new Object("ID Card", 50, 150, 0, 0);
            objects[904] = new Object("ID Card Back", 50, 150, 0, 0);
            objects[905] = new Object("Worker List", 50, 50, 0, 0);
        }
    }
}
