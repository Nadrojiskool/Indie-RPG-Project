using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace Game1
{
    public class Animation
    {
        /// <summary>
        /// Animation IDs:
        /// 0: Fill, Black
        /// </summary>



        public Rectangle Box { get; set; }
        public Stopwatch Clock = new Stopwatch();
        public long Interval { get; set; }
        public int ID { get; set; }
        public int Frame { get; set; }
        public int Frames { get; set; }
        public float Angle { get; set; }

        public Animation(Rectangle box, int id, long interval, int frame, int frames, float angle)
        {
            this.Box = box;
            this.ID = id;
            this.Interval = interval;
            this.Frame = frame;
            this.Frames = frames;
            this.Angle = angle;
            this.Clock.Start();
        }
    }
}
