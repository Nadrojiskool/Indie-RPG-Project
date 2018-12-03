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

namespace Game1
{
    public class Control : Game1
    {
        public void Monitor()
        {
            /* TWO OPTIONS:
             * Pull State from Main thread and handle input
             * Con - Async await solves computation of active monitoring?
             * OR
             * Library for input processing (CURRENT)
             * Con - Requires more Public Variables && Access direct between UI and Main thread
             */
        }
        
        public static void Click()
        {
            if (!invOpen && !buildMenuOpen && !MainMenuOpen) {
                int spellX = (newMouseState.X / (int)(50 * tileScale) + cameraLocationX) - 2;
                int spellY = (newMouseState.Y / (int)(50 * tileScale) + cameraLocationY) - 2;
                Manipulator(spellX, spellY, 5, 5, 0, false); }

            if (MainMenuOpen) {
                Client.Job job = new Client.Job((byte)Client.Net.UserList[0].JobList.Count(), 5, Client.Net.UserList[0].Endpoint, Client.Net.Endpoint);
                Console.WriteLine($"Starting Job ID {Client.Net.UserList[0].JobList.Count()}");
                Client.Net.UserList[0].JobList.Add(job);
                var t = Task.Run(() => Client.Net.JobManager(job)); }

            if (invOpen) {
                invOpen = false; }

            if (buildMenuOpen) {
                if (buildRect1.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 200) {
                    //Player.player.resources[5] = Player.player.resources[5] - 200;
                    Mine(201); }
                else if (buildRect2.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50) {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(101); }
                else if (buildRect3.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50) {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(102); }
                else if (buildRect4.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50) {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(103); }
                else if (buildRect5.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50) {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(104); }
                else if (buildRect6.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50) {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(105); }
                else if (buildRect7.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50) {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(106); }
                else if (buildRect8.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[6] >= 0) {
                    //Player.player.resources[6] = Player.player.resources[6] - 200;
                    Mine(202); }
                else {
                    cantBuild.Start(); }

                buildMenuOpen = false;
            }
        }

        public static void ClickRight()
        {
            if (!invOpen && !buildMenuOpen && !MainMenuOpen) {
                int spellX = (newMouseState.X / (int)(50 * tileScale) + cameraLocationX) - 2;
                int spellY = (newMouseState.Y / (int)(50 * tileScale) + cameraLocationY) - 2;
                Manipulator(spellX, spellY, 5, 5, 5, false); }

            if (MainMenuOpen) {
                var t = Task.Run(() => Generate.All()); }

            if (invOpen) {
                /*invOpen = false;*/ }

            if (buildMenuOpen) {
                /*if (buildRect1.Contains(newMouseState.X, newMouseState.Y))
                {
                    Mine(201);
                }
                else if (buildRect2.Contains(newMouseState.X, newMouseState.Y))
                {
                    Mine(101);
                }*/
                buildMenuOpen = false; }

            if (workerListOpen) {
                // NOTE TO SELF // ADD START BIAS // i.e. Workers favor mining in the direction placed
                if (Unit.Active.Count < Player.Units.Count)
                {
                    Unit.Active.Add(Player.Units[Unit.Active.Count]);
                    Unit.Active[Unit.Active.Count - 1].X = Player.player.X + MovementXY[Player.player.LastMove, 0];
                    Unit.Active[Unit.Active.Count - 1].Y = Player.player.Y + MovementXY[Player.player.LastMove, 1];
                    landArray[Unit.Active[Unit.Active.Count - 1].X, Unit.Active[Unit.Active.Count - 1].Y].IsOccupied = true;
                }
                workerListOpen = false; }
        }

        public static void Keyboard()
        {
            if (newState.IsKeyDown(Keys.LeftShift)) {
                Shift(); }

            else if (newState.IsKeyDown(Keys.LeftControl)) {
                Ctrl(); }

            else if (oldState.IsKeyUp(Keys.Space) && newState.IsKeyDown(Keys.Space)) {
                if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0],
                    cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].land == 202)
                    { Player.player.depth++; }
                else {
                    Player.player.depth = 0;
                    actionPending = true;
                    actionTimer.Start(); }}

            else if (oldState.IsKeyUp(Keys.I) && newState.IsKeyDown(Keys.I)) {
                if (invOpen == true) {
                    invOpen = false; }
                else {
                    invOpen = true; }}

            else if (oldState.IsKeyUp(Keys.M) && newState.IsKeyDown(Keys.M)) {
                if (tileScale != .5) {
                    tileScale = .5; }
                else {
                    tileScale = 2; }
                /*ScaleTileMap();*/ }

            else if (oldState.IsKeyUp(Keys.B) && newState.IsKeyDown(Keys.B))
            {
                if (buildMenuOpen == true) {
                    buildMenuOpen = false; }
                else {
                    buildMenuOpen = true; }}

            else if (oldState.IsKeyUp(Keys.E) && newState.IsKeyDown(Keys.E)) {
                if (workerListOpen == true) {
                    workerListOpen = false; }
                else {
                    workerListOpen = true; }}

            else {
                Keys[] keysHolder = newState.GetPressedKeys();
                int keyCheck;
                for (int i = 0; i < keysHolder.Length; i++)
                {
                    keyCheck = Array.IndexOf(KeysMovement, keysHolder[i]);
                    if (keyCheck >= 0 /*&& oldState.IsKeyUp(keysHolder[i])*/) {
                        Player.player.LastMove = keyCheck + 1;
                        Player.player.Rotation = (float)Player.player.LastMove * ((float)Math.PI / 2.0f);
                        if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0],
                            cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].land == 0)
                            { Movement(Player.player); }}
                }}
        }

        void Alt()
        {

        }

        static void Ctrl()
        {
            if (newState.IsKeyDown(Keys.S)) {
                Data.Save(); }
            else if (newState.IsKeyDown(Keys.L)) {
                Data.Load(); }
            else if (oldState.IsKeyUp(Keys.A) && newState.IsKeyDown(Keys.A)) {
                if (playerAuto == false) {
                    playerAuto = true; }
                else {
                    Player.player.AutoX = 0;
                    Player.player.AutoY = 0;
                    landArray[Player.player.X, Player.player.Y].IsOccupied = false;
                    landArray[Player.player.X + MovementXY[Player.player.LastMove, 0], Player.player.Y + MovementXY[Player.player.LastMove, 1]].IsActive = false;
                    playerAuto = false; }}
        }

        static void Shift()
        {
            if (newState.IsKeyDown(Keys.L)) {
                Client.Job job = new Client.Job((byte)Client.Net.UserList[0].JobList.Count(), 5, Client.Net.UserList[0].Endpoint, Client.Net.Endpoint);
                Console.WriteLine($"Starting Job ID {Client.Net.UserList[0].JobList.Count()}");
                Client.Net.UserList[0].JobList.Add(job);
                var t = Task.Run(() => Client.Net.JobManager(job)); }
        }

        void Touch()
        {

        }
        
        static void Movement(Unit unit)
        {
            unit.Rotation = (float)unit.LastMove * ((float)Math.PI / 2.0f);
            if (unit == Player.player)
            {
                cameraLocationX = Check.Range(cameraLocationX + MovementXY[unit.LastMove, 0], 0, 1000);
                cameraLocationY = Check.Range(cameraLocationY + MovementXY[unit.LastMove, 1], 0, 1000);
                Player.player.X = Check.Range(Player.player.X + MovementXY[unit.LastMove, 0], 40, 960);
                Player.player.Y = Check.Range(Player.player.Y + MovementXY[unit.LastMove, 1], 20, 980);
            }
            else
            {
                unit.X = Check.Range(unit.X + MovementXY[unit.LastMove, 0], 0, 1000);
                unit.Y = Check.Range(unit.Y + MovementXY[unit.LastMove, 1], 0, 1000);
            }
        }

        public static void GlobalCooldown()
        {
            if (actionTimer.ElapsedMilliseconds > 100)
            {
                actionPending = false;
                actionTimer.Reset();
                Mine(actionValue);
                actionValue = 0;
            }
        }

        public static void Mine(int a)
        {
            if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].land != 1)
            {
                if (a > 0) {
                    actionPending = true;
                    actionValue = -a;
                    actionTimer.Start(); }

                else if (a < 0) {
                    if (Math.Abs(a) == 201 || Math.Abs(a) == 202) {
                        Player.player.resources[10] = Player.player.resources[10] + 4; }

                    if (MovementXY[Player.player.LastMove, 0] > 0 || MovementXY[Player.player.LastMove, 1] > 0) {
                        Manipulator(cameraLocationX + Player.player.tileX + (Object.Objects[Math.Abs(a)].Width * MovementXY[Player.player.LastMove, 0]),
                            cameraLocationY + Player.player.tileY + (Object.Objects[Math.Abs(a)].Height * MovementXY[Player.player.LastMove, 1]),
                            Object.Objects[Math.Abs(a)].Width, Object.Objects[Math.Abs(a)].Height, 1, true);
                        landArray[cameraLocationX + Player.player.tileX + (Object.Objects[Math.Abs(a)].Width * MovementXY[Player.player.LastMove, 0]),
                            cameraLocationY + Player.player.tileY + (Object.Objects[Math.Abs(a)].Height * MovementXY[Player.player.LastMove, 1])].land = Math.Abs(a); }
                    else {
                        Manipulator(cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0],
                            cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1], Object.Objects[Math.Abs(a)].Width,
                            Object.Objects[Math.Abs(a)].Height, 1, true);
                        landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0],
                            cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].land = Math.Abs(a); }}

                else {
                    Gather(cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0],
                        cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1], Player.player); }
            }
        }

        public static void Gather(int a, int b, Unit unit)
        {
            if (landArray[a, b].land != 0)
            {
                unit.stats[0]++;
                Player.player.resources[landArray[a, b].land]++;

                if (landArray[a, b].land == 2 || landArray[a, b].land == 3)
                {
                    unit.stats[11]++;
                }
                else if (landArray[a, b].land == 4)
                {
                    unit.stats[18]++;
                    unit.stats[19]++;
                    unit.stats[20]++;
                    unit.stats[21]++;
                }
                else if (landArray[a, b].land == 5)
                {
                    unit.stats[12]++;
                }
                else if (landArray[a, b].land == 6)
                {
                    unit.stats[14]++;
                }
                else if (landArray[a, b].land > 6)
                {
                    unit.stats[17]++;
                    if (landArray[a, b].land == 202)
                    {
                        unit.depth = 1;
                    }
                }
                landArray[a, b].land = 0;
            }
        }

        public static void Manipulator(int x, int y, int width, int height, int land, bool inversion)
        {
            if (inversion == true)
            {
                for (int a = 0; a < height; a++)
                {
                    for (int b = 0; b < width; b++)
                    {
                        if (landArray[x - a, y - b].land != land)
                        {
                            if (landArray[x - a, y - b].land != 0)
                            {
                                Gather(x - a, y - b, Player.player);
                            }
                            landArray[x - a, y - b].land = land;
                        }
                    }
                }
            }
            else
            {
                for (int a = 0; a < height; a++)
                {
                    for (int b = 0; b < width; b++)
                    {
                        if (landArray[x + a, y + b].land != land)
                        {
                            if (landArray[x + a, y + b].land != 0)
                            {
                                Gather(x + a, y + b, Player.player);
                            }
                            landArray[x + a, y + b].land = land;
                        }
                    }
                }
            }
        }
        
        public static void AutoAI(Unit unit)
        {
            if (unit.ActionTime.IsRunning == false)
            {
                if (unit.AutoX > 0)
                {
                    if (unit.Y < unit.AutoY)
                    {
                        if (unit.Rotation != 2 * (float)Math.PI / 2.0f)
                        {
                            unit.Rotation = 2 * (float)Math.PI / 2.0f;
                            unit.LastMove = 2;
                        }

                        if (landArray[unit.X, unit.Y + 1].land < 3
                            || landArray[unit.X, unit.Y + 1].land > 99)
                        {
                            landArray[unit.X, unit.Y].IsOccupied = false;
                            Movement(unit);
                            unit.ActionTime.Start();
                            unit.ActionDuration = 1000;
                            landArray[unit.X, unit.Y].IsOccupied = true;
                        }
                        else
                        {
                            unit.AutoX = 0;
                            unit.ActionTime.Start();
                            unit.ActionDuration = 10000;
                            landArray[unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1]].IsActive = true;
                        }
                        return;
                    }
                    else if (unit.Y > unit.AutoY)
                    {
                        if (unit.Rotation != 4 * (float)Math.PI / 2.0f)
                        {
                            unit.Rotation = 4 * (float)Math.PI / 2.0f;
                            unit.LastMove = 4;
                        }

                        if (landArray[unit.X, unit.Y - 1].land < 3
                            || landArray[unit.X, unit.Y - 1].land > 99)
                        {
                            landArray[unit.X, unit.Y].IsOccupied = false;
                            Movement(unit);
                            unit.ActionTime.Start();
                            unit.ActionDuration = 1000;
                            landArray[unit.X, unit.Y].IsOccupied = true;
                        }
                        else
                        {
                            unit.AutoX = 0;
                            unit.ActionTime.Start();
                            unit.ActionDuration = 10000;
                            landArray[unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1]].IsActive = true;
                        }
                        return;
                    }
                    else if (unit.X < unit.AutoX)
                    {
                        if (unit.Rotation != (float)Math.PI / 2.0f)
                        {
                            unit.Rotation = (float)Math.PI / 2.0f;
                            unit.LastMove = 1;
                        }

                        if (landArray[unit.X + 1, unit.Y].land < 3
                            || landArray[unit.X + 1, unit.Y].land > 99)
                        {
                            landArray[unit.X, unit.Y].IsOccupied = false;
                            Movement(unit);
                            unit.ActionTime.Start();
                            unit.ActionDuration = 1000;
                            landArray[unit.X, unit.Y].IsOccupied = true;
                        }
                        else
                        {
                            unit.AutoX = 0;
                            unit.ActionTime.Start();
                            unit.ActionDuration = 10000;
                            landArray[unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1]].IsActive = true;
                        }
                        return;
                    }
                    else if (unit.X > unit.AutoX)
                    {
                        if (unit.Rotation != 3 * (float)Math.PI / 2.0f)
                        {
                            unit.Rotation = 3 * (float)Math.PI / 2.0f;
                            unit.LastMove = 3;
                        }

                        if (landArray[unit.X - 1, unit.Y].land < 3
                            || landArray[unit.X - 1, unit.Y].land > 99)
                        {
                            landArray[unit.X, unit.Y].IsOccupied = false;
                            Movement(unit);
                            unit.ActionTime.Start();
                            unit.ActionDuration = 1000;
                            landArray[unit.X, unit.Y].IsOccupied = true;
                        }
                        else
                        {
                            unit.AutoX = 0;
                            unit.ActionTime.Start();
                            unit.ActionDuration = 10000;
                            landArray[unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1]].IsActive = true;
                        }
                        return;
                    }
                }
                else if (unit.AutoX == 0)
                {
                    int x = unit.X;
                    int y = unit.Y;
                    for (int a = 1; a < 25; a++)
                    {
                        x--;
                        y--;
                        if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false)
                        {
                            unit.AutoX = x;
                            unit.AutoY = y;
                            return;
                        }

                        for (int c = 0; c < (a * 2); c++)
                        {
                            x++;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false)
                            {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return;
                            }
                        }

                        for (int c = 0; c < (a * 2); c++)
                        {
                            y++;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false)
                            {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return;
                            }
                        }

                        for (int c = 0; c < (a * 2); c++)
                        {
                            x--;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false)
                            {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return;
                            }
                        }

                        for (int c = 0; c < (a * 2); c++)
                        {
                            y--;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false)
                            {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                if (unit.ActionTime.ElapsedMilliseconds > unit.ActionDuration)
                {
                    if (unit.ActionTime.ElapsedMilliseconds > 10000)
                    {
                        Gather(unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1], unit);
                        landArray[unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1]].IsActive = false;
                    }
                    unit.ActionTime.Reset();
                }
            }
        }
        
        /// /////////////////////////////////
        // UNTESTED CODE, MOVEMENT CONTROLS//
        /// /////////////////////////////////

        public static void MovementTouch()
        {
            var gesture = TouchPanel.ReadGesture();

            if (gesture.Position.X > displayWidth * 1 / 4 && gesture.Position.X < displayWidth * 3 / 4 &&
                gesture.Position.Y > displayHeight * 1 / 4 && gesture.Position.Y < displayHeight * 3 / 4)
            { Mine(0); }
            else
            {
                if (gesture.Position.X > displayWidth * 3 / 4 && cameraLocationX < 1000 - (displayWidth / 50) / tileScale - 2 / tileScale)
                {
                    if (landArray[Check.Max(cameraLocationX + Player.player.tileX + 1, 1000), cameraLocationY + Player.player.tileY].land == 0)
                    {
                        cameraLocationX = cameraLocationX + 1;
                        Player.player.X++;
                    }
                    Player.player.Rotation = (float)Math.PI / 2.0f;
                    Player.player.LastMove = 1;
                }

                if (gesture.Position.X < displayWidth * 1 / 4 && cameraLocationX > 0)
                {
                    if (landArray[Check.Min(cameraLocationX + Player.player.tileX - 1, 0), cameraLocationY + Player.player.tileY].land == 0)
                    {
                        cameraLocationX = cameraLocationX - 1;
                        Player.player.X--;
                    }
                    Player.player.Rotation = 3 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 3;
                }

                if (gesture.Position.Y > displayHeight * 3 / 4 && cameraLocationY < 1000 - (displayHeight / 50) / tileScale - 2 / tileScale)
                {
                    if (landArray[cameraLocationX + Player.player.tileX, Check.Max(cameraLocationY + Player.player.tileY + 1, 1000)].land == 0)
                    {
                        cameraLocationY = cameraLocationY + 1;
                        Player.player.Y++;
                    }
                    Player.player.Rotation = 2 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 2;
                }

                if (gesture.Position.Y < displayHeight * 1 / 4 && cameraLocationY > 0)
                {
                    if (landArray[cameraLocationX + Player.player.tileX, Check.Min(cameraLocationY + Player.player.tileY - 1, 0)].land == 0)
                    {
                        cameraLocationY = cameraLocationY - 1;
                        Player.player.Y--;
                    }
                    Player.player.Rotation = 4 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 4;
                }
            }
        }
    }
}
