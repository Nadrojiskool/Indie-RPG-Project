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
        public int Width { get; set; }
        public int Height { get; set; }
        //static public Object Inventory;
        //static public Object BuildMenu;
        //static public Object Kame;
        //static public Object OrbPillar;
        //static public Object DefenseWallWood;
        static public Object[] Objects = new Object[250];
        static public Object[] InterfaceObjects = new Object[100];

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
            Objects[000] = new Object("Zero", 50, 50, 1, 1);
            Objects[001] = new Object("Land", 50, 50, 1, 1);
            Objects[002] = new Object("Water", 50, 50, 1, 1);
            Objects[003] = new Object("Bush", 50, 50, 1, 1);
            Objects[004] = new Object("Deer", 50, 50, 1, 1);
            Objects[005] = new Object("Tree", 50, 50, 1, 1);
            Objects[006] = new Object("Rock", 50, 50, 1, 1);
            Objects[100] = new Object("Obelisk", 50, 100, 1, 1);
            Objects[101] = new Object("Wall Wood Horizontal", 50, 100, 1, 1);
            Objects[102] = new Object("Wall Wood Vertical", 50, 100, 1, 1);
            Objects[103] = new Object("Wall Wood Corner Left", 50, 100, 1, 1);
            Objects[104] = new Object("Wall Wood Corner Right", 50, 100, 1, 1);
            Objects[105] = new Object("Wall Wood Back Left", 50, 100, 1, 1);
            Objects[106] = new Object("Wall Wood Back Right", 50, 100, 1, 1);
            Objects[201] = new Object("Kame House", 100, 150, 2, 2);
            Objects[202] = new Object("Mine", 100, 100, 2, 2);
        }
    }
}
