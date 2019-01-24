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
    public class Net : Game1
    {
        public static bool AcceptConnections = true;
        public static bool messageReceived = false;
        public static bool messageConfirmation = false;
        public static int connectedUsers = 0;
        public static List<User> UserList = new List<User>();
        public static UdpClient Client = new UdpClient(56000);
        public static IPEndPoint Endpoint = new IPEndPoint(IPAddress.Parse("24.20.157.144"), 57000); // endpoint where server is listening
        public static byte delay = 10;
        
        public static async Task JobManager(Job job)
        {
            job.IsActive = true;
            while (job.IsActive)
            {
                if (job.Type == 2)
                {
                    job.ElapsedTime.Start();
                    Console.WriteLine($"Sending Confirmation for Job ID {job.ID}...");
                    Speaker(new byte[] { 2, job.ID }, Client, Endpoint);
                    if (job.ElapsedTime.ElapsedMilliseconds > 5000)
                    {
                        job.IsActive = false;
                        return;
                    }
                    await Task.Delay(1000);
                }
                else if (job.Type == 5)
                {
                    if (job.BiomeList.Count == 25 && job.LandList.Count == 25)
                    {
                        byte[] biome = new byte[1000000];
                        byte[] land = new byte[1000000];
                        int count = 0;
                        foreach (byte[] b in job.BiomeList)
                        {
                            System.Buffer.BlockCopy(b, 0, biome, count, b.Length);
                            count = count + b.Length;
                        }
                        count = 0;
                        foreach (byte[] b in job.LandList)
                        {
                            System.Buffer.BlockCopy(b, 0, land, count, b.Length);
                            count = count + b.Length;
                        }

                        for (int y = 0; y < 1000; y++)
                        {
                            for (int x = 0; x < 1000; x++)
                            {
                                landArray[x, y] = new Land();
                                landArray[x, y].biome = biome[(y * 1000) + x];
                                landArray[x, y].land = land[(y * 1000) + x];
                            }
                        }
                        Console.WriteLine($"Byte List Full!");
                        MainMenuOpen = false;
                        LogicClock40.Start();
                        LogicClock100.Start();
                        LogicClock250.Start();

                        Job j = new Job(job.ID, 2, job.Employee, job.Employer);
                        UserList[0].JobList.Add(j);
                        JobManager(j);
                        job.IsCompleted = true;
                        job.IsActive = false;
                        return;
                    }
                    Speaker(new byte[] { 5, job.ID }, Client, Endpoint);
                }
                await Task.Delay(1000);
            }
        }

        public static async Task Listener(UdpClient client)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 56000);
            Stopwatch elapsed = new Stopwatch();
            byte[] packet;
            elapsed.Start();
            while (AcceptConnections == true)
            {
                DataProcessor(packet = await Task.Run(() => client.Receive(ref endpoint)), endpoint);
            }
        }

        public static async Task DataProcessor(byte[] packet, IPEndPoint endpoint)
        {
            Console.WriteLine($"Received Packet Type {packet[0]} ID {packet[1]}");
            if (packet[0] == 2)
            {

            }
            else if (packet[0] == 5)
            {
                foreach (Job job in UserList[0].JobList)
                {
                    if (job.ID == packet[1])
                    {
                        if (!job.BiomeList.Any(b => b.Length == packet.Length - 2))
                        {
                            byte[] packet2 = new byte[packet.Length - 2];
                            Array.Copy(packet, 2, packet2, 0, packet2.Length);
                            job.BiomeList.Add(packet2);
                            job.BiomeList.Sort((a, b) => a.Length.CompareTo(b.Length));
                        }
                    }
                }
            }
            else if (packet[0] == 6)
            {
                foreach (Job job in UserList[0].JobList)
                {
                    if (job.ID == packet[1])
                    {
                        if (!job.LandList.Any(b => b.Length == packet.Length - 2))
                        {
                            byte[] packet2 = new byte[packet.Length - 2];
                            Array.Copy(packet, 2, packet2, 0, packet2.Length);
                            job.LandList.Add(packet2);
                            job.LandList.Sort((a, b) => a.Length.CompareTo(b.Length));
                        }
                    }
                }
            }
        }

        public static async Task Speaker(byte[] packet, UdpClient client, IPEndPoint endpoint)
        {
            client.Send(packet, packet.Length, endpoint);

            if (packet[0] == 2)
            {
                Console.WriteLine($"Sent Confirmation ({packet[0]})");
            }
            else if (packet[0] == 5)
            {
                Console.WriteLine($"Sent Request Type {packet[0]} ID {packet[1]} ");
            }
            else
            {

            }
        }
    }
}
