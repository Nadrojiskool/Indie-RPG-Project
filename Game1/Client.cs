using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Client : UdpClient
    {
        public static UdpClient client = new UdpClient();

        public bool isActive()
        {
            return this.Active;
        }
    }
}
