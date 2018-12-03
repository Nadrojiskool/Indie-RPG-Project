using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Contract
{
    public class Title : Asset
    {
        // Needs dated and Signed by Lord then Stored by Owner
        public Title(Player owner) : base(owner)
        {
            this.Owner = owner;
        }
    }
}
