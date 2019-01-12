using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Region
{
    /* In concept there are two ways to lay out Asset and Regional Design
     * Second Approach:
     * Each tile only holds, "Second Layer Information," and points to a Capitol (not Capital) 
     * Capitol.X && Capitol.Y // OR // Capitol.ID
     * Where a Capitol : Tile holds first layer logistic elements
     * 
     * 
     * 
     * First Approach:
     * Each Tile only points to an Owner <Player> 
     * 
     * 
     */



    public class Kingdom
    {
        //List<Duchy> Duchies = new List<Duchy>();
        Player King { get; set; }
        long[] Capitol { get; set; }

        public void SetCapitol(long x, long y)
        {
            Capitol = new long[2] { x, y };
        }
    }
}
