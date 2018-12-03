using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Contract
{
    public class Asset
    {
        public Player Owner { get; set; }

        public Asset(Player owner)
        {
            Owner = owner;
        }
    }
}
