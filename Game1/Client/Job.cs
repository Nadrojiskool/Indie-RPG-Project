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
    public class Job
    {
        public List<byte[]> BiomeList = new List<byte[]>();
        public List<byte[]> LandList = new List<byte[]>();
        public byte ID;
        public byte Type;
        public IPEndPoint Employee;
        public IPEndPoint Employer;
        public Stopwatch ElapsedTime = new Stopwatch();
        public bool IsActive;
        public bool IsCompleted;

        public Job(byte id, byte type, IPEndPoint ep, IPEndPoint EP)
        {
            ID = id;
            Type = type;
            Employee = ep;
            Employer = EP;
        }
    }
}
