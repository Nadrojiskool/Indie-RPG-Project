using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Game1.Client
{
    public class User
    {
        public string Username;
        public IPEndPoint Endpoint;
        public List<Job> JobList = new List<Job>();

        public User(string username, IPEndPoint endpoint)
        {
            Username = username;
            Endpoint = endpoint;
        }
    }
}
