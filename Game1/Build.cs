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
            Object.Objects[201].X = 600;
            Object.Objects[201].Y = 650;
            Object.Objects[101].X = Game1.buildRect2.X + 25;
            Object.Objects[101].Y = Game1.buildRect2.Y + 25;
            Object.Objects[102].X = Game1.buildRect3.X + 25;
            Object.Objects[102].Y = Game1.buildRect3.Y + 25;
            Object.Objects[103].X = Game1.buildRect4.X + 25;
            Object.Objects[103].Y = Game1.buildRect4.Y + 25;
            Object.Objects[104].X = Game1.buildRect5.X + 25;
            Object.Objects[104].Y = Game1.buildRect5.Y + 25;
            Object.Objects[105].X = Game1.buildRect6.X + 25;
            Object.Objects[105].Y = Game1.buildRect6.Y + 25;
            Object.Objects[106].X = Game1.buildRect7.X + 25;
            Object.Objects[106].Y = Game1.buildRect7.Y + 25;
            Object.Objects[202].X = Game1.buildRect8.X + 25;
            Object.Objects[202].Y = Game1.buildRect8.Y + 25;
        }

    }
}
