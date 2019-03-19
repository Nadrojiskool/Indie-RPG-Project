using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Register
{
    public class Objective : Task
    {
        public static Origin Origin { get; set; }

        public Objective(Origin origin, int x, int y, int id) : base(x, y, id)
        {
            Origin = origin;
        }
    }
}
