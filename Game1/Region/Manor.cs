using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Contract;

namespace Game1.Region
{
    public class Manor
    {
        List<Asset> Assets = new List<Asset>();
        Player Lord { get; set; }
        County Domain { get; set; }
        sbyte[] Capitol { get; set; }

        public void SetCapitol(sbyte x, sbyte y)
        {
            Capitol = new sbyte[2] { x, y };
        }
    }
}
