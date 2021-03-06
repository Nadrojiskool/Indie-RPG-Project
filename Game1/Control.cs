﻿using System;
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
            if (!MainMenuOpen && !invOpen && !buildMenuOpen && !workerListOpen && Show.CursorBuilding == 0) {
                if (Player.player.ToolBelt[1] != 0)
                {
                    Set.LandAtCursor(oldMouseState, 7);
                }
                else
                {
                    // Clear Land //
                    int spellX = ((newMouseState.X - Player.player.TileOffsetXY[0]) / CurrentTileSize + cameraLocationX) - 2;
                    int spellY = ((newMouseState.Y - Player.player.TileOffsetXY[1]) / CurrentTileSize + cameraLocationY) - 2;
                    Manipulator(spellX, spellY, 5, 5, 0, false);
                }}

            if (MainMenuOpen)
            {
                if (newMouseState.X > 860 && newMouseState.X < 1060)
                {
                    if (newMouseState.Y > 440 && newMouseState.Y < 490)
                    {

                    }
                    else if (newMouseState.Y > 640 && newMouseState.Y < 690)
                    {
                        var t = Task.Run(() => Generate.All());
                    }
                }
            }

            /*if (MainMenuOpen) {
                Client.Job job = new Client.Job((byte)Client.Net.UserList[0].JobList.Count(), 5, Client.Net.UserList[0].Endpoint, Client.Net.Endpoint);
                Console.WriteLine($"Starting Job ID {Client.Net.UserList[0].JobList.Count()}");
                Client.Net.UserList[0].JobList.Add(job);
                var t = Task.Run(() => Client.Net.JobManager(job)); }*/

            if (Show.ActiveDialogue) {
                Show.ActiveDialogue = false; }

            if (invOpen) {
                invOpen = false; }
            
            if (buildMenuOpen)
            {
                int x = Player.player.X + MovementXY[Player.player.LastMove, 0];
                int y = Player.player.Y + MovementXY[Player.player.LastMove, 1];

                // This build menu seriously needs standardized and refactored // Eventual Integration into Rectangle Grid
                if (buildRect1.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[5] = Player.player.resources[5] - 200;
                    Show.CursorBuilding = 201;
                }
                else if (buildRect2.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[5] = Player.player.resources[5] - 50;
                    Show.CursorBuilding = 101;
                }
                else if (buildRect3.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[5] = Player.player.resources[5] - 50;
                    Show.CursorBuilding = 102;
                }
                else if (buildRect4.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[5] = Player.player.resources[5] - 50;
                    Show.CursorBuilding = 103;
                }
                else if (buildRect5.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[5] = Player.player.resources[5] - 50;
                    Show.CursorBuilding = 104;
                }
                else if (buildRect6.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[5] = Player.player.resources[5] - 50;
                    Show.CursorBuilding = 105;
                }
                else if (buildRect7.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[5] = Player.player.resources[5] - 50;
                    Show.CursorBuilding = 106;
                }
                else if (buildRect8.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[6] >= 0)
                {
                    //Player.player.resources[6] = Player.player.resources[6] - 200;
                    Show.CursorBuilding = 202;
                }
                else if (buildRect9.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[6] = Player.player.resources[6] - 200;
                    Show.CursorBuilding = 200;
                }
                else if (buildRect10.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[6] = Player.player.resources[6] - 200;
                    Build.Camp(Player.player.X + MovementXY[Player.player.LastMove, 0], Player.player.Y + MovementXY[Player.player.LastMove, 1]);
                }
                else if (buildRect11.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[6] = Player.player.resources[6] - 200;
                    Generate.Village(50, Player.player.X + MovementXY[Player.player.LastMove, 0], Player.player.Y + MovementXY[Player.player.LastMove, 1]);
                }
                else if (buildRect12.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    //Player.player.resources[6] = Player.player.resources[6] - 200;
                    Show.CursorBuilding = 100;
                }
                else if (buildRect13.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    Generate.Bonfire(100, Player.player.X + MovementXY[Player.player.LastMove, 0], Player.player.Y + MovementXY[Player.player.LastMove, 1]);
                }
                else if (buildRect14.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 300;
                    Show.CursorTower = new Tower(10, 10, 3000);
                }
                else if (buildRect15.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 301;
                    Show.CursorTower = new Tower(3, 10, 2);
                }
                else if (buildRect16.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 302;
                    Show.CursorTower = new Tower(7, 5, 3);
                }
                else if (buildRect17.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 303;
                    Show.CursorTower = new Tower(10, 3, 5);
                }
                else if (buildRect18.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 304;
                    Show.CursorTower = new Tower(5, 5, 5);
                }
                else if (buildRect19.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 305;
                    Show.CursorTower = new Tower(2, 3, 10);
                }
                else if (buildRect20.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 306;
                    Show.CursorTower = new Tower(3, 5, 7);
                }
                else if (buildRect21.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 307;
                    Show.CursorTower = new Tower(15, 2, 8);
                }
                else if (buildRect22.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0 && !Player.Towers.ContainsKey(new GPS(x, y, 0)))
                {
                    Show.CursorBuilding = 308;
                    Show.CursorTower = new Tower(5, 18, 2);
                }
                else if (buildRect23.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    Build.Single(Player.Spawner.X, Player.Spawner.Y, 0);
                    Build.Single(x, y, 398);
                    Player.Spawner = new GPS(x, y, 0);
                }
                else if (buildRect24.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 0)
                {
                    Build.Single(Player.Goal.X, Player.Goal.Y, 0);
                    Build.Single(x, y, 399);
                    Player.Goal = new GPS(x, y, 0);
                }
                else
                {
                    cantBuild.Start();
                }

                buildMenuOpen = false;
            }
            else if (Show.CursorBuilding != 0)
            {
                int x = cameraLocationX + (int)Math.Ceiling((double)((newMouseState.X - Player.player.TileOffsetXY[0]) / CurrentTileSize));
                int y = cameraLocationY + (int)Math.Ceiling((double)((newMouseState.Y - Player.player.TileOffsetXY[1]) / CurrentTileSize));

                if (!Player.Towers.Keys.Contains(new GPS(x, y, 0)))
                {
                    // i needs refactored to add streamlined support for non-buildable (Tile Index 01) padding //
                    int[,] i = new int[1, 1] { { Show.CursorBuilding } };
                    Set.Land(i, x, y);
                    if (Show.CursorBuilding >= 300 && Show.CursorBuilding < 398)
                        Player.Towers.Add(new GPS(x, y, 0), Show.CursorTower);
                }
            }

            if (workerListOpen)
            {
                foreach (Unit unit in Player.LocalWorkers)
                {
                    unit.ActionID = 3;
                    Task.Delay(31);
                }
                workerListOpen = false;
            }
        }

        public static void ClickRight()
        {
            if (Show.CursorBuilding != 0)
                Show.CursorBuilding = 0;

            if (!MainMenuOpen && !invOpen && !buildMenuOpen && !workerListOpen)
            {
                // AOE Attack (5x5)
                /*foreach (Unit unit in Player.LocalEnemies)
                    Player.player.CheckAOE(unit, (newMouseState.X / CurrentTileSize + cameraLocationX) - 2, (newMouseState.Y / CurrentTileSize + cameraLocationY) - 2, 5, 5);
                Player.Animations.Add(new Animation((newMouseState.X / CurrentTileSize + cameraLocationX) - 2, (newMouseState.Y / CurrentTileSize + cameraLocationY) - 2, 0, 0));
                // Spawn Trees //
                /*int spellX = (newMouseState.X / (int)(50 * tileScale) + cameraLocationX) - 2;
                int spellY = (newMouseState.Y / (int)(50 * tileScale) + cameraLocationY) - 2;
                Manipulator(spellX, spellY, 5, 5, 5, false);*/
            }

            /*if (MainMenuOpen) {
                var t = Task.Run(() => Generate.All()); }*/
            
            if (Show.ActiveDialogue) {
                Show.ActiveDialogue = false; }

            if (invOpen) {
                /*invOpen = false;*/ }

            if (buildMenuOpen) {
                buildMenuOpen = false; }

            if (workerListOpen) {
                // SIMPLE FEATURE : ADD START BIAS // i.e. Workers favor mining in the direction placed
                if (Player.LocalWorkers.Count < Player.Workers.Count)
                {
                    Player.LocalWorkers.Add(Player.Workers[Player.LocalWorkers.Count]);
                    Player.LocalWorkers[Player.LocalWorkers.Count - 1].X = Player.player.X + MovementXY[Player.player.LastMove, 0];
                    Player.LocalWorkers[Player.LocalWorkers.Count - 1].Y = Player.player.Y + MovementXY[Player.player.LastMove, 1];
                    Player.LocalWorkers[Player.LocalWorkers.Count - 1].LastMove = Player.player.LastMove;
                    Player.LocalWorkers[Player.LocalWorkers.Count - 1].ActionID = 1;
                    //landArray[Player.LocalWorkers[Player.LocalWorkers.Count - 1].X, Player.LocalWorkers[Player.LocalWorkers.Count - 1].Y].IsOccupied = true;
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
                int x = cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0];
                int y = cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1];
                if (actionTimer.IsRunning)
                {
                    if (Player.player.ActionID == 254)
                    {
                        Player.player.ActionCache = (int)actionTimer.ElapsedMilliseconds;
                        Player.player.ActionID = 255;
                    }
                    else if (Player.player.ActionID == 255)
                    {
                        actionPending = false;
                        actionTimer.Reset();
                        actionValue = 0;
                        Player.player.ActionID = 0;
                        Player.player.ActionCache = 0;
                        Mine(0);
                    }
                }
                else if (Check.IsResource(x, y))
                {
                    actionPending = true;
                    Player.player.ActionID = 254;
                    actionTimer.Start();
                }
                else if (Player.WorldItems.ContainsKey(new GPS(x, y, 0)))
                {
                    Player.WorldItems.Remove(new GPS(x, y, 0));
                    Show.ActiveDialogue = true;
                    Player.player.ToolBelt[1]++;
                }
                else
                {
                    List<Unit> units = Player.LocalEnemies.Values.ToList();
                    foreach (Unit unit in units)
                    {
                        Player.player.CheckSlash(unit);
                    } } }

            else if (oldState.IsKeyUp(Keys.I) && newState.IsKeyDown(Keys.I)) {
                if (invOpen == true) {
                    invOpen = false; }
                else {
                    invOpen = true; }}

            else if (oldState.IsKeyUp(Keys.M) && newState.IsKeyDown(Keys.M)) {
                if (tileScale != .5) {
                    tileScale = .5;
                    CurrentTileSize = (int)(50 * tileScale);
                    cameraLocationX -= 31;
                    cameraLocationY -= 15;
                    Set.GridDimensions();
                }
                else {
                    tileScale = 2;
                    CurrentTileSize = (int)(50 * tileScale);
                    cameraLocationX += 31;
                    cameraLocationY += 15;
                    Set.GridDimensions();
                }
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


            else if (oldState.IsKeyUp(Keys.T) && newState.IsKeyDown(Keys.T))
            {
                // Performance testing zone
                for (int i = 0; i < 10; i++)
                {
                    DrawingBoard.Text.Add(Check.Benchmark().ToString());
                }
            }

            else {
                
            }
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
                    //landArray[Player.player.X, Player.player.Y].IsOccupied = false;
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

        public static void PlayerMovement(Unit unit)
        {
            if (cameraLocationX + MovementXY[unit.LastMove, 0] < 0 ||
                cameraLocationX + MovementXY[unit.LastMove, 0] >= MapWidth - (int)(40 / tileScale) ||
                cameraLocationY + MovementXY[unit.LastMove, 1] < 0 ||
                cameraLocationY + MovementXY[unit.LastMove, 1] >= MapHeight - (int)(20 / tileScale) ||
                (Player.player.X < (20 / tileScale) && Player.player.LastMove % 2 == 1) ||
                (Player.player.Y < (10 / tileScale) && Player.player.LastMove % 2 == 0) ||
                (Player.player.X > MapWidth - (20 / tileScale) - 1 && Player.player.LastMove % 2 == 1) ||
                (Player.player.Y > MapHeight - (10 / tileScale) - 1 && Player.player.LastMove % 2 == 0))
            {
                Player.player.X = Player.player.X + MovementXY[unit.LastMove, 0];
                Player.player.Y = Player.player.Y + MovementXY[unit.LastMove, 1];
            }
            else
            {
                cameraLocationX = cameraLocationX + MovementXY[unit.LastMove, 0];
                cameraLocationY = cameraLocationY + MovementXY[unit.LastMove, 1];
                Player.player.X = Player.player.X + MovementXY[unit.LastMove, 0];
                Player.player.Y = Player.player.Y + MovementXY[unit.LastMove, 1];
            }
        }
        
        public static void Movement(Unit unit)
        {
            unit.Rotation = (float)unit.LastMove * ((float)Math.PI / 2.0f);
            if (unit == Player.player)
            {
                if (cameraLocationX + MovementXY[unit.LastMove, 0] < 0 ||
                    cameraLocationX + MovementXY[unit.LastMove, 0] > MapWidth - (int)(20 / tileScale) ||
                    cameraLocationY + MovementXY[unit.LastMove, 1] < 0 ||
                    cameraLocationY + MovementXY[unit.LastMove, 1] > MapHeight - (int)(10 / tileScale) ||
                    (Player.player.X < (20 / tileScale) && Player.player.LastMove % 2 == 1) ||
                    (Player.player.Y < (10 / tileScale) && Player.player.LastMove % 2 == 0 ||
                    (Player.player.X > MapWidth - (20 / tileScale) - 1 && Player.player.LastMove % 2 == 1) ||
                    (Player.player.Y > MapHeight - (10 / tileScale) - 1 && Player.player.LastMove % 2 == 0)))
                {
                    Player.player.X = Player.player.X + MovementXY[unit.LastMove, 0];
                    Player.player.Y = Player.player.Y + MovementXY[unit.LastMove, 1];

                }
                else
                {
                    cameraLocationX = cameraLocationX + MovementXY[unit.LastMove, 0];
                    cameraLocationY = cameraLocationY + MovementXY[unit.LastMove, 1];
                    Player.player.X = Player.player.X + MovementXY[unit.LastMove, 0];
                    Player.player.Y = Player.player.Y + MovementXY[unit.LastMove, 1];
                }
            }
            else
            {
                unit.X = Check.Range(unit.X + MovementXY[unit.LastMove, 0], 0, MapWidth - (int)(20 / tileScale) - 1);
                unit.Y = Check.Range(unit.Y + MovementXY[unit.LastMove, 1], 0, MapHeight - (int)(10 / tileScale) - 1);

                // note: values don't check map boundary range
                unit.DestinationOffset[0] += MovementXY[unit.LastMove, 0];
                unit.DestinationOffset[1] += MovementXY[unit.LastMove, 1];

                if (unit.LeftOrRight != 0)
                {
                    unit.OriginOffset[0] += MovementXY[unit.LastMove, 0];
                    unit.OriginOffset[1] += MovementXY[unit.LastMove, 1];

                    sbyte sb = Math.Abs(unit.LeftOrRight);
                    if (unit.OriginOffset[1] == 0)
                    {
                        if (sb == 1)
                        {
                            if (unit.OriginOffset[0] > 0)
                            {
                                unit.LeftOrRight = 0;
                                unit.PathingCheckpoint[0] = 0;
                                unit.PathingCheckpoint[1] = 0;
                                unit.OriginOffset[0] = 0;
                                unit.OriginOffset[1] = 0;
                            }
                        }
                        else if (sb == 3)
                        {
                            if (unit.OriginOffset[0] < 0)
                            {
                                unit.LeftOrRight = 0;
                                unit.PathingCheckpoint[0] = 0;
                                unit.PathingCheckpoint[1] = 0;
                                unit.OriginOffset[0] = 0;
                                unit.OriginOffset[1] = 0;
                            }
                        }
                    }
                    else if (unit.OriginOffset[0] == 0)
                    {
                        if (sb == 2)
                        {
                            if (unit.OriginOffset[1] > 0)
                            {
                                unit.LeftOrRight = 0;
                                unit.PathingCheckpoint[0] = 0;
                                unit.PathingCheckpoint[1] = 0;
                                unit.OriginOffset[0] = 0;
                                unit.OriginOffset[1] = 0;
                            }
                        }
                        else if (sb == 4)
                        {
                            if (unit.OriginOffset[1] < 0)
                            {
                                unit.LeftOrRight = 0;
                                unit.PathingCheckpoint[0] = 0;
                                unit.PathingCheckpoint[1] = 0;
                                unit.OriginOffset[0] = 0;
                                unit.OriginOffset[1] = 0;
                            }
                        }
                    }
                    else if (unit.LastMove == sb)
                    {
                        if (unit.X == unit.PathingCheckpoint[0] && unit.Y == unit.PathingCheckpoint[1])
                        {
                            unit.LeftOrRight = 0;
                            unit.PathingCheckpoint[0] = 0;
                            unit.PathingCheckpoint[1] = 0;
                            unit.OriginOffset[0] = 0;
                            unit.OriginOffset[1] = 0;
                        }
                        else
                        {
                            unit.PathingCheckpoint[0] = unit.X;
                            unit.PathingCheckpoint[1] = unit.Y;
                        }
                    }
                }
            }
        }

        public static void GlobalCooldown()
        {
            if (actionTimer.ElapsedMilliseconds > 3000)
            {
                actionPending = false;
                actionTimer.Reset();
                actionValue = 0;
                Player.player.ActionID = 0;
                Player.player.ActionCache = 0;
                Mine(0);
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
                        Manipulator(cameraLocationX + Player.player.tileX + (Show.Objects[Math.Abs(a)].Width * MovementXY[Player.player.LastMove, 0]),
                            cameraLocationY + Player.player.tileY + (Show.Objects[Math.Abs(a)].Height * MovementXY[Player.player.LastMove, 1]),
                            Show.Objects[Math.Abs(a)].Width, Show.Objects[Math.Abs(a)].Height, 1, true);
                        landArray[cameraLocationX + Player.player.tileX + (Show.Objects[Math.Abs(a)].Width * MovementXY[Player.player.LastMove, 0]),
                            cameraLocationY + Player.player.tileY + (Show.Objects[Math.Abs(a)].Height * MovementXY[Player.player.LastMove, 1])].land = Math.Abs(a); }
                    else {
                        Manipulator(cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0],
                            cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1], Show.Objects[Math.Abs(a)].Width,
                            Show.Objects[Math.Abs(a)].Height, 1, true);
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
                unit.Stats[0]++;
                Player.player.resources[landArray[a, b].land]++;

                if (landArray[a, b].land == 2 || landArray[a, b].land == 3)
                {
                    unit.Stats[11]++;
                }
                else if (landArray[a, b].land == 4)
                {
                    unit.Stats[18]++;
                    unit.Stats[19]++;
                    unit.Stats[20]++;
                    unit.Stats[21]++;
                }
                else if (landArray[a, b].land == 5)
                {
                    unit.Stats[12]++;
                }
                else if (landArray[a, b].land == 6)
                {
                    unit.Stats[14]++;
                }
                else if (landArray[a, b].land > 6)
                {
                    unit.Stats[17]++;
                    if (landArray[a, b].land == 202)
                    {
                        unit.Depth = 1;
                    }
                }
                landArray[a, b].land = 0;
                landArray[a, b].frame = 5;
            }
        }

        public static void Manipulator(int x, int y, int width, int height, int landValue, bool inversion)
        {
            if (inversion == true)
            {
                for (int a = 0; a < height; a++)
                {
                    for (int b = 0; b < width; b++)
                    {
                        Land land = landArray[x - a, y - b];
                        if (land.land != landValue)
                        {
                            if (land.land != 0)
                            {
                                Gather(x - a, y - b, Player.player);
                            }
                            land.land = landValue;
                        }
                        Player.Domain.Add(land);
                        land.IsOwned = true;
                    }
                }
            }
            else
            {
                for (int a = 0; a < height; a++)
                {
                    for (int b = 0; b < width; b++)
                    {
                        Land land = landArray[x + a, y + b];
                        if (land.land != landValue)
                        {
                            if (land.land != 0)
                            {
                                Gather(x + a, y + b, Player.player);
                            }
                            land.land = landValue;
                        }
                        Player.Domain.Add(land);
                        land.IsOwned = true;
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
                            //landArray[unit.X, unit.Y].IsOccupied = false;
                            Movement(unit);
                            unit.ActionTime.Start();
                            unit.ActionDuration = 1000;
                            //landArray[unit.X, unit.Y].IsOccupied = true;
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
                            //landArray[unit.X, unit.Y].IsOccupied = false;
                            Movement(unit);
                            unit.ActionTime.Start();
                            unit.ActionDuration = 1000;
                            //landArray[unit.X, unit.Y].IsOccupied = true;
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
                            //landArray[unit.X, unit.Y].IsOccupied = false;
                            Movement(unit);
                            unit.ActionTime.Start();
                            unit.ActionDuration = 1000;
                            //landArray[unit.X, unit.Y].IsOccupied = true;
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
                            //landArray[unit.X, unit.Y].IsOccupied = false;
                            Movement(unit);
                            unit.ActionTime.Start();
                            unit.ActionDuration = 1000;
                            //landArray[unit.X, unit.Y].IsOccupied = true;
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

        public static void Pathing(Unit unit)
        {
            if (unit.ActionTime.IsRunning == false)
            {
                //List<sbyte[]> path = new List<sbyte[]>();
                
                if (unit.LeftOrRight != 0)
                {

                }
                else
                {
                    /*if (Math.Abs(unit.DestinationOffset[0]) == Math.Abs(unit.DestinationOffset[1]))
                    {
                        // Not Implemented Diagnal
                    }
                    else */
                    if (Math.Abs(unit.DestinationOffset[0]) >= Math.Abs(unit.DestinationOffset[1]))
                    {
                        if (unit.DestinationOffset[0] > 0)
                        {
                            Set.Rotation(unit, 3);
                        }
                        else if (unit.DestinationOffset[0] < 0)
                        {
                            Set.Rotation(unit, 1);
                        }
                    }
                    else if (Math.Abs(unit.DestinationOffset[1]) > Math.Abs(unit.DestinationOffset[0]))
                    {
                        if (unit.DestinationOffset[1] > 0)
                        {
                            Set.Rotation(unit, 4);
                        }
                        else if (unit.DestinationOffset[1] < 0)
                        {
                            Set.Rotation(unit, 2);
                        }
                    }
                }
                
                // Changing Map Tiles can cause units to get stuck in a loop where they cannot reach their destination
                // A timeout of 20 seconds or longer should be implemented to allow for large object pathing before resetting LeftOrRight to 0
                // Additionally, a single array of x and y can store the last x & y of the unit at direction 1 * (unit.LeftOrRight / Math.Abs(unit.LeftOrRight))
                // Then store a tracker which stores absolute spin, which can track absolute right of 4 and then loop to back to 1 and check against last LastMove(1)
                // This protects further against false trigger loops where there may be a left before returning to spin of 1
                // Furthermore, some implementation of absolute spin for further pathing cutt-off points may be beneficial

                int x = unit.X + MovementXY[unit.LastMove, 0];
                int y = unit.Y + MovementXY[unit.LastMove, 1];
                Land land = landArray[x, y];

                if (unit.ActionID == 3 && land.land > 2 && land.land < 100)
                {
                    if (!land.IsActive)
                    {
                        unit.ActionTime.Start();
                        unit.ActionDuration = 10000;
                        land.IsActive = true;
                    }
                    else
                    {
                        unit.DestinationOffset[0] = 0;
                        unit.DestinationOffset[1] = 0;
                        unit.LeftOrRight = 0;
                        unit.PathingCheckpoint[0] = 0;
                        unit.PathingCheckpoint[1] = 0;
                        unit.OriginOffset[0] = 0;
                        unit.OriginOffset[1] = 0;
                    }
                }
                else if (land.land < 1)
                {
                    if (!Player.LocalEnemies.ContainsKey(new GPS(x, y, 0)))
                    {
                        Player.LocalEnemies.Remove(new GPS(unit.X, unit.Y, 0));
                        Movement(unit);
                        Player.LocalEnemies.Add(new GPS(unit.X, unit.Y, 0), unit);
                    }
                    unit.ActionTime.Start();
                    unit.ActionDuration = 1000;
                    if (unit.LeftOrRight != 0)
                    {
                        int bias = (unit.LeftOrRight / Math.Abs(unit.LeftOrRight));
                        unit.LastMove = Check.LoopInt((unit.LastMove - bias), 1, 4);
                        //unit.PathingTotalRotation -= bias;
                    }
                }
                else
                {
                    if (unit.LeftOrRight != 0)
                    {
                        int bias = (unit.LeftOrRight / Math.Abs(unit.LeftOrRight));
                        unit.LastMove = Check.LoopInt((unit.LastMove + bias), 1, 4);
                        //unit.PathingTotalRotation += bias;
                    }
                    else
                    {
                        CheckPathing(unit); // AI NEEDS TIMEOUTS TO STOP PATHING LOOPS
                    }
                }
            }
            else
            {
                if (unit.ActionTime.ElapsedMilliseconds > unit.ActionDuration)
                {
                    if (unit.ActionTime.ElapsedMilliseconds >= 10000)
                    {
                        Gather(unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1], unit);
                        landArray[unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1]].IsActive = false;
                    }
                    unit.ActionTime.Reset();
                }
            }
        }

        public static void CheckPathing (Unit unit)
        {
            int left = PathingLeft(unit);
            unit.Pathed = null;
            int right = PathingRight(unit);
            unit.Pathed = null;

            if (left == 1000 && right == 1000)
            {
                unit.ActionTime.Start();
                unit.ActionDuration = 3000;
                return;
            }
            else if (left < right)
            {
                unit.LeftOrRight = Convert.ToSByte(-unit.LastMove);
            }
            else
            {
                unit.LeftOrRight = Convert.ToSByte(unit.LastMove);
            }
            unit.PathingCheckpoint[0] = unit.X;
            unit.PathingCheckpoint[1] = unit.Y;

            #region Generate Full Path (Not Implemented)
            //False Start, Not as Intended, Wanted "Blind Pathing"
            /*List<int[]> pathLeft = new List<int[]>();
            List<int[]> pathRight = new List<int[]>();
            int plane;
            if (unit.LastMove == 1 || unit.LastMove == 3)
            {
                plane = 0;
            }
            else
            {
                plane = 1;
            }

            while (true)
            {
                pathLeft.Add(PathLeft());
                int[] offset = pathLeft[pathLeft.Count];
                leftOffset[0] += offset[0];
                leftOffset[1] += offset[1];
                pathRight.Add(PathRight());

                offset = pathLeft[pathLeft.Count];
                rightOffset[0] += offset[0];
                rightOffset[1] += offset[1];

                if (leftOffset[plane] == 0 || rightOffset[plane] == 0)
                {
                    break;
                }
            }*/
            #endregion
        }

        public static int PathingLeft(Unit unit)
        {
            bool isStarted = false;
            int length = 0;
            int[] offset = new int[] { 0, 0 };
            int lastMove = Check.LoopInt((unit.LastMove - 1), 1, 4);
            unit.Pathed = new List<int[]>();

            while (true)
            {
                int x = (unit.X + MovementXY[lastMove, 0] + offset[0]);
                int y = (unit.Y + MovementXY[lastMove, 1] + offset[1]);

                if (landArray[x, y].land < 1)
                {
                    int[] xy = new int[2] { x, y };
                    if (unit.Pathed.Contains(xy))
                        return 1000;
                    else
                        unit.Pathed.Add(new int[2] { x, y });

                    offset[0] += MovementXY[lastMove, 0];
                    offset[1] += MovementXY[lastMove, 1];
                    length += 1;
                    lastMove = Check.LoopInt((lastMove + 1), 1, 4);

                    if (!isStarted)
                    {
                        isStarted = true;
                    }
                    else
                    {
                        // Future Note: Modify Scan Length by unit.Vision, reflected in, "Smarter Units," being able to Path further
                        if (length > 100)
                        {
                            return 1000;
                        }
                    }
                }
                else
                {
                    lastMove = Check.LoopInt((lastMove - 1), 1, 4);
                }

                if (isStarted == true)
                {
                    if (offset[1] == 0)
                    {
                        if (unit.LastMove == 1)
                        {
                            if (offset[0] > 0)
                            {
                                break;
                            }
                        }
                        else if (unit.LastMove == 3)
                        {
                            if (offset[0] < 0)
                            {
                                break;
                            }
                        }
                    }
                    else if (offset[0] == 0)
                    {
                        if (unit.LastMove == 2)
                        {
                            if (offset[1] > 0)
                            {
                                break;
                            }
                        }
                        else if (unit.LastMove == 4)
                        {
                            if (offset[1] < 0)
                            {
                                break;
                            }
                        }
                    }
                }

                x = (unit.X + MovementXY[lastMove, 0] + offset[0]);
                y = (unit.Y + MovementXY[lastMove, 1] + offset[1]);
                if (x < 0 || x > 1000 || y < 0 || y > 1000)
                {
                    return 1000;
                }
            }

            return (length);
        }

        public static int PathingRight(Unit unit)
        {
            bool isStarted = false;
            int length = 0;
            int[] offset = new int[] { 0, 0 };
            int lastMove = Check.LoopInt((unit.LastMove + 1), 1, 4);
            unit.Pathed = new List<int[]>();

            while (true)
            {
                int x = (unit.X + MovementXY[lastMove, 0] + offset[0]);
                int y = (unit.Y + MovementXY[lastMove, 1] + offset[1]);
                
                if (landArray[x, y].land < 1)
                {
                    int[] xy = new int[2] { x, y };
                    if (unit.Pathed.Contains(xy))
                        return 1000;
                    else
                        unit.Pathed.Add(new int[2] { x, y });

                    offset[0] += MovementXY[lastMove, 0];
                    offset[1] += MovementXY[lastMove, 1];
                    length += 1;
                    lastMove = Check.LoopInt((lastMove - 1), 1, 4);

                    if (!isStarted)
                    {
                        isStarted = true;
                    }
                    else
                    {
                        // Future Note: Modify Scan Length by unit.Vision, reflected in, "Smarter Units," being able to Path further
                        if (length > 100)
                        {
                            return 1000;
                        }
                    }
                }
                else
                {
                    lastMove = Check.LoopInt((lastMove + 1), 1, 4);
                }

                if (isStarted == true)
                {
                    if (offset[1] == 0)
                    {
                        if (unit.LastMove == 1)
                        {
                            if (offset[0] > 0)
                            {
                                break;
                            }
                        }
                        else if (unit.LastMove == 3)
                        {
                            if (offset[0] < 0)
                            {
                                break;
                            }
                        }
                    }
                    else if (offset[0] == 0)
                    {
                        if (unit.LastMove == 2)
                        {
                            if (offset[1] > 0)
                            {
                                break;
                            }
                        }
                        else if (unit.LastMove == 4)
                        {
                            if (offset[1] < 0)
                            {
                                break;
                            }
                        }
                    }
                }

                x = (unit.X + MovementXY[lastMove, 0] + offset[0]);
                y = (unit.Y + MovementXY[lastMove, 1] + offset[1]);
                if (x < 0 || x > 1000 || y < 0 || y > 1000)
                {
                    return 1000;
                }
            }

            return (length);
        }

        public static void UnitManager(Unit unit)
        {
            if (unit.ActionID == 1)
            {
                if (unit.DestinationOffset[1] == 0 && unit.DestinationOffset[0] == 0) {
                    FollowPlayer(unit); }
                else {
                    Pathing(unit); }
            }
            else if (unit.ActionID == 2)
            {
                unit.X = Player.player.X - MovementXY[Player.player.LastMove, 0];
                unit.Y = Player.player.Y - MovementXY[Player.player.LastMove, 1];
                Set.Rotation(unit, Player.player.LastMove);
            }
            else if (unit.ActionID == 3)
            {
                if (unit.DestinationOffset[1] == 0 && unit.DestinationOffset[0] == 0)
                {
                    ScanForResource(unit);
                }
                else
                {
                    Pathing(unit);
                }
            }
            else if (unit.ActionID == 4)
            {
                if (Math.Abs(Player.player.X - unit.X) <= 1 && Math.Abs(Player.player.Y - unit.Y) <= 1 && !unit.ActionTime.IsRunning)
                {
                    unit.Attack(Player.player);
                    unit.ActionTime.Start();
                    unit.ActionDuration = 2000 - (10 * unit.Stats[13]);
                    unit.DestinationOffset[0] = 0;
                    unit.DestinationOffset[1] = 0;
                }
                else if (UpdateDestination.ElapsedMilliseconds >= 250)
                {
                    unit.DestinationOffset[0] = (unit.X - Player.player.X - MovementXY[Player.player.LastMove, 0]);
                    unit.DestinationOffset[1] = (unit.Y - Player.player.Y - MovementXY[Player.player.LastMove, 1]);
                    //unit.LeftOrRight = 0;
                    //unit.OriginOffset[0] = 0;
                    //unit.OriginOffset[1] = 0;
                }
                else
                {
                    Pathing(unit);
                }
            }
            else
            {
                unit.ActionTime.Start();
                unit.ActionDuration = 3000;
            }
        }

        public static void FollowPlayer(Unit unit)
        {
            if (unit.X == Player.player.X - MovementXY[Player.player.LastMove, 0] && unit.Y == Player.player.Y - MovementXY[Player.player.LastMove, 1])
            {
                unit.ActionID = 2;
            }
            else
            {
                unit.DestinationOffset[0] = (unit.X - Player.player.X + MovementXY[Player.player.LastMove, 0]);
                unit.DestinationOffset[1] = (unit.Y - Player.player.Y + MovementXY[Player.player.LastMove, 1]);
            }
        }

        public static void ScanForResource(Unit unit)
        {
            int x = unit.X;
            int y = unit.Y;
            for (int a = 1; a < 25; a++)
            {
                x--;
                y--;
                if (CheckSetDestination(unit, Check.Range(x, 0, 1000), Check.Range(y, 0, 1000)))
                    return;

                for (int c = 0; c < (a * 2); c++)
                {
                    x++;
                    if (CheckSetDestination(unit, Check.Range(x, 0, 1000), Check.Range(y, 0, 1000)))
                        return;
                }

                for (int c = 0; c < (a * 2); c++)
                {
                    y++;
                    if (CheckSetDestination(unit, Check.Range(x, 0, 1000), Check.Range(y, 0, 1000)))
                        return;
                }

                for (int c = 0; c < (a * 2); c++)
                {
                    x--;
                    if (CheckSetDestination(unit, Check.Range(x, 0, 1000), Check.Range(y, 0, 1000)))
                        return;
                }

                for (int c = 0; c < (a * 2); c++)
                {
                    y--;
                    if (CheckSetDestination(unit, Check.Range(x, 0, 1000), Check.Range(y, 0, 1000)))
                        return;
                }
            }
        }

        public static void ScanLocalTiles(int x, int y, int lastMove, int id)
        {
            // lastMove bias currently not implemented
            // id tentatively pending actual functionality
            Land land;
            int x2 = 0;
            int y2 = 0;

            for (int a = 0; a < 25; a++)
            {
                x2 -= a;
                y2 -= a;

                for (int a2 = 0; a2 <= a; a++)
                {
                    land = landArray[x + x2, y + y2];
                    if (land.land == id)
                    {

                    }
                    else
                    {
                        if (a != 0)
                        {
                            for (int l = 1; l <= 4; l++)
                            {
                                for (int l2 = 0; l2 < a * 2; l2++)
                                {
                                    x2 += MovementXY[l, 0];
                                    y2 += MovementXY[l, 1];
                                    land = landArray[x + x2, y + y2];
                                    if (land.land == id)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool CheckSetDestination(Unit unit, int x, int y)
        {
            if (Check.IsResource(x, y) && !landArray[x, y].IsActive)
            {
                unit.DestinationOffset[0] = unit.X - x;
                unit.DestinationOffset[1] = unit.Y - y;
                return true;
            }
            else { return false; }
        }

        /*public static async void Logic()
        {
            while (true)
            {
                if (MovementSync[0] <= -25 ||
                    MovementSync[0] >= 25 ||
                    MovementSync[1] <= -25 ||
                    MovementSync[1] >= -25)
                {
                    MovementSync[0] = 0;
                    MovementSync[1] = 0;
                    //LocalizeElements();
                }
                else
                {
                    await Task.Delay(500);
                }
            }
        }*/

        /*static void LocalizeElements()
        {
            Player.LocalWorkers.Clear();
            Player.LocalEnemies.Clear();

            foreach (Unit unit in Player.Workers)
            {
                if (unit.X >= Player.player.X - 50 ||
                    unit.X <= Player.player.X + 50 ||
                    unit.Y >= Player.player.Y - 50 ||
                    unit.Y <= Player.player.Y + 50)
                {
                    Player.LocalWorkers.Add(unit);
                }
            }
            foreach (Unit unit in Player.Enemies)
            {
                if (unit.X >= Player.player.X - 50 ||
                    unit.X <= Player.player.X + 50 ||
                    unit.Y >= Player.player.Y - 50 ||
                    unit.Y <= Player.player.Y + 50)
                {
                    Player.LocalEnemies.Add(unit);
                }
            }
        }*/
        
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
