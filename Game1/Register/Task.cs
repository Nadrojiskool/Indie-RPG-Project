using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Register
{
    public class Task
    {
        int X { get; set; }
        int Y { get; set; }
        int ID { get; set; }
        int Progress { get; set; }

        public Task(int x, int y, int id)
        {
            X = x;
            Y = y;
            ID = id;
        }
    }
}
