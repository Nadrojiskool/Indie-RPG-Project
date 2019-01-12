using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Boss : Unit
    {
        // Future Bosses:
        // Raging Bull: Charges at any nearby units, leaving a blazing trail of destruction in his wake.
        // Jim: An Earthworm still occasionally needs a breath of fresh air, but he angers quickly if attacked.
        // Scorchbeast: You're not serious, right?
        // Zeus (Enel): Sure, why not.

        public Boss(int x, int y, int id, int[] array) : base(x, y, id, array)
        {
            this.X = x;
            this.Y = y;
            this.ID = id;
            this.Stats = array;
        }
    }
}
