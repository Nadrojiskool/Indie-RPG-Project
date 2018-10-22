using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public static class Build
    {
        public static void Run()
        {
            Object.objects[201].X = 600;
            Object.objects[201].Y = 650;
            Object.objects[101].X = Game1.buildRect2.X + 25;
            Object.objects[101].Y = Game1.buildRect2.Y + 25;
            Object.objects[102].X = Game1.buildRect3.X + 25;
            Object.objects[102].Y = Game1.buildRect3.Y + 25;
            Object.objects[103].X = Game1.buildRect4.X + 25;
            Object.objects[103].Y = Game1.buildRect4.Y + 25;
            Object.objects[104].X = Game1.buildRect5.X + 25;
            Object.objects[104].Y = Game1.buildRect5.Y + 25;
            Object.objects[105].X = Game1.buildRect6.X + 25;
            Object.objects[105].Y = Game1.buildRect6.Y + 25;
            Object.objects[106].X = Game1.buildRect7.X + 25;
            Object.objects[106].Y = Game1.buildRect7.Y + 25;
            Object.objects[202].X = Game1.buildRect8.X + 25;
            Object.objects[202].Y = Game1.buildRect8.Y + 25;
        }

    }
}
