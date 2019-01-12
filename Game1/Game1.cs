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
using Game1.Client;
using Game1.Contract;
using Game1.Region;

namespace Game1
{
    public class Game1 : Game
    {
        protected static bool NoMap = true;
        protected static bool MainMenuOpen = true;
        public static double tileScale = .5;

        #region Graphics Variables
        public GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static SpriteFont font;
        protected static Texture2D inventory;
        protected static Texture2D idCard;
        protected static Texture2D idCardBack;
        protected static Texture2D buildMenu;
        protected static Texture2D workerList;
        protected static Texture2D player;
        protected static Texture2D enemy;
        protected static Texture2D BGFinalFantasy;
        protected static Texture2D house_kame;
        protected static Texture2D mine;
        protected static Texture2D cabin1;
        protected static Texture2D orbPillar;
        protected static Texture2D WallWoodHorizontal;
        protected static Texture2D WallWoodVertical;
        protected static Texture2D WallWoodCornerLeft;
        protected static Texture2D WallWoodCornerRight;
        protected static Texture2D WallWoodBackLeft;
        protected static Texture2D WallWoodBackRight;
        private Texture2D white;
        private Texture2D land;
        private Texture2D water;
        private Texture2D bush;
        private Texture2D deer;
        private Texture2D tree0;
        private Texture2D tree1;
        private Texture2D tree2;
        private Texture2D tree3;
        private Texture2D tree4;
        private Texture2D tree5;
        private Texture2D snow;
        private Texture2D snowTree;
        private Texture2D snowDeer;
        private Texture2D snowBush;
        private Texture2D nodeStone;
        private Texture2D nodeCopper;
        private Texture2D nodeTin;
        private Texture2D nodeIron;
        #endregion

        #region UI Variables
        public static int displayWidth;
        public static int displayHeight;
        protected static KeyboardState newState;
        protected static KeyboardState oldState;
        protected static MouseState newMouseState;
        protected static MouseState oldMouseState;
        protected static Stopwatch actionTimer = new Stopwatch();
        protected static Stopwatch cantBuild = new Stopwatch();
        protected static bool invOpen = false;
        protected static bool buildMenuOpen = false;
        protected static bool workerListOpen = false;
        protected static bool actionPending = false;
        protected static bool playerAuto = false;
        protected static int actionValue = 0;
        private int gestureHolder;
        #endregion

        #region Logic Variables
        protected static Land[,] landArray = new Land[1000, 1000];
        protected static Land[,] tileArray = new Land[400, 250];
        protected static int cameraLocationX = 0;
        protected static int cameraLocationY = 0;
        private int tilePointerX = 0;
        private int tilePointerY = 0;
        private static Random Random = new Random();
        private double rnd;
        #endregion

        private bool gameActive = true;
        public static byte[] receivedBytes = new byte[50000];
        protected static Keys[] KeysMovement = new Keys[] { Keys.D, Keys.S, Keys.A, Keys.W };
        public static int[,] MovementXY = new int[,] { { 0, 0 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        protected static List<Manor> LocalManors = new List<Manor>();
        public static int[] MovementSync = new int[2] { 0, 0 };

        public static Stopwatch ScanTiles = new Stopwatch();
        public static Stopwatch UpdateDestination = new Stopwatch();

        #region Build Menu Static Rectangle Grid Test Variables
        public static Rectangle buildRect1 = new Rectangle(600, 650, 100, 150);
        public static Rectangle buildRect2 = new Rectangle(750, 650, 50, 100);
        public static Rectangle buildRect3 = new Rectangle(800, 650, 50, 100);
        public static Rectangle buildRect4 = new Rectangle(850, 650, 50, 100);
        public static Rectangle buildRect5 = new Rectangle(900, 650, 50, 100);
        public static Rectangle buildRect6 = new Rectangle(950, 650, 50, 100);
        public static Rectangle buildRect7 = new Rectangle(1000, 650, 50, 100);
        public static Rectangle buildRect8 = new Rectangle(1050, 650, 100, 100);
        public static Rectangle buildRect9 = new Rectangle(1150, 650, 100, 100);
        public static Rectangle buildRect10 = new Rectangle(600, 980, 200, 80);
        public static Rectangle buildRect11 = new Rectangle(900, 980, 200, 80);
        #endregion

        public Rectangle[,] TileMap = new Rectangle[(int)(42 / tileScale), (int)(24 / tileScale)];
        
        public static string Username = "King Charles I";
        
        public Game1()
        {
            Content.RootDirectory = "Content";

            //            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //            {
            //                displayWidth = 1920;
            //                displayHeight = 1080;
            //            }

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = displayWidth;
            graphics.PreferredBackBufferHeight = displayHeight;
            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 

        public static int testHP = 0;
        public static int testVit = 0;
        public static int testPhy = 0;

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            TouchPanel.EnabledGestures = GestureType.Tap;

            Player.player = new Player((int)((20 * displayWidth / 1920) / tileScale), (int)((10 * displayHeight / 1080) / tileScale), Generate.Worker());
            Player.player.tileX = (int)((20 * displayWidth / 1920) / tileScale);
            Player.player.tileY = (int)((10 * displayHeight / 1080) / tileScale);
            Player.player.DrawX = (int)((20 * displayWidth / 1920) / tileScale) * (int)(50 * tileScale) + (int)(25 * tileScale);
            Player.player.DrawY = (int)((10 * displayHeight / 1080) / tileScale) * (int)(50 * tileScale) + (int)(25 * tileScale);
            Player.player.Stats = Generate.Worker();
            Set.CoreStats(Player.player);
            testHP = Player.player.Stats[1];
            testVit = Player.player.Stats[11];
            testPhy = Player.player.Stats[12];

            Show.Initialize();

            //var t = Task.Run(() => Control.Logic());

            //Net.UserList.Add(new User("Bob", Net.Endpoint));
            //var t2 = Task.Run(() => Net.Listener(Net.Client));

            //ScaleTileMap();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //framerate = Content.Load<SpriteFont>("Framerate");

            LoadSprites();
        }

        void LoadSprites()
        {

            /////////////////////////
            // Cache Sprite Assets //
            /////////////////////////

            #region Fonts
            font = Content.Load<SpriteFont>("SpriteFont");
            #endregion

            #region Sprite Base
            white = new Texture2D(GraphicsDevice, 1, 1);
            white.SetData(new[] { Color.White });
            land = Content.Load<Texture2D>("Land");
            water = Content.Load<Texture2D>("Water");
            player = Content.Load<Texture2D>("Player");
            enemy = Content.Load<Texture2D>("Enemy");
            bush = Content.Load<Texture2D>("Bush");
            deer = Content.Load<Texture2D>("Deer");
            tree0 = Content.Load<Texture2D>("Tree0");
            tree1 = Content.Load<Texture2D>("Tree1");
            tree2 = Content.Load<Texture2D>("Tree2");
            tree3 = Content.Load<Texture2D>("Tree3");
            tree4 = Content.Load<Texture2D>("Tree4");
            tree5 = Content.Load<Texture2D>("Tree5");
            snow = Content.Load<Texture2D>("Snow");
            snowTree = Content.Load<Texture2D>("SnowTree");
            snowDeer = Content.Load<Texture2D>("SnowDeer");
            snowBush = Content.Load<Texture2D>("SnowBush");
            nodeStone = Content.Load<Texture2D>("NodeStone");
            nodeCopper = Content.Load<Texture2D>("NodeCopper");
            nodeTin = Content.Load<Texture2D>("NodeTin");
            nodeIron = Content.Load<Texture2D>("NodeIron");
            BGFinalFantasy = Content.Load<Texture2D>("BGFinalFantasy");
            inventory = Content.Load<Texture2D>("Inventory");
            idCard = Content.Load<Texture2D>("IDCard");
            idCardBack = Content.Load<Texture2D>("IDCardBack");
            buildMenu = Content.Load<Texture2D>("ui_3");
            workerList = Content.Load<Texture2D>("ui_4");
            house_kame = Content.Load<Texture2D>("House (Kame)");
            mine = Content.Load<Texture2D>("Mine");
            cabin1 = Content.Load<Texture2D>("Cabin1");
            orbPillar = Content.Load<Texture2D>("Orb Pillar");
            WallWoodHorizontal = Content.Load<Texture2D>("Wall Wood");
            WallWoodVertical = Content.Load<Texture2D>("WallWoodVertical");
            WallWoodCornerLeft = Content.Load<Texture2D>("WallWoodCornerLeft");
            WallWoodCornerRight = Content.Load<Texture2D>("WallWoodCornerRight");
            WallWoodBackLeft = Content.Load<Texture2D>("WallWoodBackLeft");
            WallWoodBackRight = Content.Load<Texture2D>("WallWoodBackRight");
            #endregion

            #region Sprite Array
            DrawingBoard.Tiles[0, 1, 5] = land;
            DrawingBoard.Tiles[0, 2, 5] = snow;
            DrawingBoard.Tiles[1, 1, 5] = land;
            DrawingBoard.Tiles[1, 2, 5] = snow;
            DrawingBoard.Tiles[2, 1, 5] = water;
            DrawingBoard.Tiles[2, 2, 5] = water;
            DrawingBoard.Tiles[3, 1, 5] = bush;
            DrawingBoard.Tiles[3, 2, 5] = snowBush;
            DrawingBoard.Tiles[4, 1, 5] = deer;
            DrawingBoard.Tiles[4, 2, 5] = deer;
            DrawingBoard.Tiles[5, 1, 0] = tree0;
            DrawingBoard.Tiles[5, 1, 1] = tree1;
            DrawingBoard.Tiles[5, 1, 2] = tree2;
            DrawingBoard.Tiles[5, 1, 3] = tree3;
            DrawingBoard.Tiles[5, 1, 4] = tree4;
            DrawingBoard.Tiles[5, 1, 5] = tree5;
            DrawingBoard.Tiles[5, 2, 0] = snowTree;
            DrawingBoard.Tiles[5, 2, 1] = snowTree;
            DrawingBoard.Tiles[5, 2, 2] = snowTree;
            DrawingBoard.Tiles[5, 2, 3] = snowTree;
            DrawingBoard.Tiles[5, 2, 4] = snowTree;
            DrawingBoard.Tiles[5, 2, 5] = snowTree;
            DrawingBoard.Tiles[6, 1, 5] = nodeStone;
            DrawingBoard.Tiles[6, 2, 5] = nodeStone;
            DrawingBoard.Tiles[101, 1, 5] = WallWoodHorizontal;
            DrawingBoard.Tiles[101, 2, 5] = WallWoodHorizontal;
            DrawingBoard.Tiles[102, 1, 5] = WallWoodVertical;
            DrawingBoard.Tiles[102, 2, 5] = WallWoodVertical;
            DrawingBoard.Tiles[103, 1, 5] = WallWoodCornerLeft;
            DrawingBoard.Tiles[103, 2, 5] = WallWoodCornerLeft;
            DrawingBoard.Tiles[104, 1, 5] = WallWoodCornerRight;
            DrawingBoard.Tiles[104, 2, 5] = WallWoodCornerRight;
            DrawingBoard.Tiles[105, 1, 5] = WallWoodBackLeft;
            DrawingBoard.Tiles[105, 2, 5] = WallWoodBackLeft;
            DrawingBoard.Tiles[106, 1, 5] = WallWoodBackRight;
            DrawingBoard.Tiles[106, 2, 5] = WallWoodBackRight;
            DrawingBoard.Tiles[200, 1, 5] = cabin1;
            DrawingBoard.Tiles[200, 2, 5] = cabin1;
            DrawingBoard.Tiles[201, 1, 5] = house_kame;
            DrawingBoard.Tiles[201, 2, 5] = house_kame;
            DrawingBoard.Tiles[202, 1, 5] = mine;
            DrawingBoard.Tiles[202, 2, 5] = mine;

            DrawingBoard.HPBar[0] = Content.Load<Texture2D>("ui_bar 50px");
            DrawingBoard.HPBar[1] = Content.Load<Texture2D>("ui_bar_hp 50px");

            DrawingBoard.Blast[0] = Content.Load<Texture2D>("Blast (1)");
            DrawingBoard.Blast[1] = Content.Load<Texture2D>("Blast (2)");
            DrawingBoard.Blast[2] = Content.Load<Texture2D>("Blast (3)");
            DrawingBoard.Blast[3] = Content.Load<Texture2D>("Blast (4)");
            DrawingBoard.Blast[4] = Content.Load<Texture2D>("Blast (5)");
            DrawingBoard.Blast[5] = Content.Load<Texture2D>("Blast (6)");
            DrawingBoard.Blast[6] = Content.Load<Texture2D>("Blast (7)");
            DrawingBoard.Blast[7] = Content.Load<Texture2D>("Blast (8)");
            DrawingBoard.Blast[8] = Content.Load<Texture2D>("Blast (9)");
            DrawingBoard.Blast[9] = Content.Load<Texture2D>("Blast (10)");
            DrawingBoard.Blast[10] = Content.Load<Texture2D>("Blast (11)");
            DrawingBoard.Blast[11] = Content.Load<Texture2D>("Blast (12)");
            DrawingBoard.Blast[12] = Content.Load<Texture2D>("Blast (13)");
            DrawingBoard.Blast[13] = Content.Load<Texture2D>("Blast (14)");
            DrawingBoard.Blast[14] = Content.Load<Texture2D>("Blast (15)");
            DrawingBoard.Blast[15] = Content.Load<Texture2D>("Blast (16)");
            DrawingBoard.Blast[16] = Content.Load<Texture2D>("Blast (17)");
            DrawingBoard.Blast[17] = Content.Load<Texture2D>("Blast (18)");
            DrawingBoard.Blast[18] = Content.Load<Texture2D>("Blast (19)");
            DrawingBoard.Blast[19] = Content.Load<Texture2D>("Blast (20)");
            DrawingBoard.Blast[20] = Content.Load<Texture2D>("Blast (21)");
            DrawingBoard.Blast[21] = Content.Load<Texture2D>("Blast (22)");
            DrawingBoard.Blast[22] = Content.Load<Texture2D>("Blast (23)");
            DrawingBoard.Blast[23] = Content.Load<Texture2D>("Blast (24)");
            DrawingBoard.Blast[24] = Content.Load<Texture2D>("Blast (25)");
            DrawingBoard.Blast[25] = Content.Load<Texture2D>("Blast (26)");
            DrawingBoard.Blast[26] = Content.Load<Texture2D>("Blast (27)");
            DrawingBoard.Blast[27] = Content.Load<Texture2D>("Blast (28)");
            DrawingBoard.Blast[28] = Content.Load<Texture2D>("Blast (29)");
            DrawingBoard.Blast[29] = Content.Load<Texture2D>("Blast (30)");
            DrawingBoard.Blast[30] = Content.Load<Texture2D>("Blast (31)");
            DrawingBoard.Blast[31] = Content.Load<Texture2D>("Blast (32)");
            DrawingBoard.Blast[32] = Content.Load<Texture2D>("Blast (33)");
            DrawingBoard.Blast[33] = Content.Load<Texture2D>("Blast (34)");
            DrawingBoard.Blast[34] = Content.Load<Texture2D>("Blast (35)");
            DrawingBoard.Blast[35] = Content.Load<Texture2D>("Blast (36)");
            DrawingBoard.Blast[36] = Content.Load<Texture2D>("Blast (37)");
            DrawingBoard.Blast[37] = Content.Load<Texture2D>("Blast (38)");
            DrawingBoard.Blast[38] = Content.Load<Texture2D>("Blast (39)");
            DrawingBoard.Blast[39] = Content.Load<Texture2D>("Blast (40)");
            DrawingBoard.Blast[40] = Content.Load<Texture2D>("Blast (41)");
            DrawingBoard.Blast[41] = Content.Load<Texture2D>("Blast (42)");
            DrawingBoard.Blast[42] = Content.Load<Texture2D>("Blast (43)");
            DrawingBoard.Blast[43] = Content.Load<Texture2D>("Blast (44)");
            DrawingBoard.Blast[44] = Content.Load<Texture2D>("Blast (45)");
            DrawingBoard.Blast[45] = Content.Load<Texture2D>("Blast (46)");
            DrawingBoard.Blast[46] = Content.Load<Texture2D>("Blast (47)");
            DrawingBoard.Blast[47] = Content.Load<Texture2D>("Blast (48)");
            DrawingBoard.Blast[48] = Content.Load<Texture2D>("Blast (49)");
            DrawingBoard.Blast[49] = Content.Load<Texture2D>("Blast (50)");
            DrawingBoard.Blast[50] = Content.Load<Texture2D>("Blast (51)");
            DrawingBoard.Blast[51] = Content.Load<Texture2D>("Blast (52)");
            DrawingBoard.Blast[52] = Content.Load<Texture2D>("Blast (53)");
            DrawingBoard.Blast[53] = Content.Load<Texture2D>("Blast (54)");
            DrawingBoard.Blast[54] = Content.Load<Texture2D>("Blast (55)");
            DrawingBoard.Blast[55] = Content.Load<Texture2D>("Blast (56)");
            DrawingBoard.Blast[56] = Content.Load<Texture2D>("Blast (57)");
            DrawingBoard.Blast[57] = Content.Load<Texture2D>("Blast (58)");
            DrawingBoard.Blast[58] = Content.Load<Texture2D>("Blast (59)");
            DrawingBoard.Blast[59] = Content.Load<Texture2D>("Blast (60)");
            DrawingBoard.Blast[60] = Content.Load<Texture2D>("Blast (61)");
            DrawingBoard.Blast[61] = Content.Load<Texture2D>("Blast (62)");
            DrawingBoard.Blast[62] = Content.Load<Texture2D>("Blast (63)");
            DrawingBoard.Blast[63] = Content.Load<Texture2D>("Blast (64)");
            DrawingBoard.Blast[64] = Content.Load<Texture2D>("Blast (65)");
            DrawingBoard.Blast[65] = Content.Load<Texture2D>("Blast (66)");
            DrawingBoard.Blast[66] = Content.Load<Texture2D>("Blast (67)");
            DrawingBoard.Blast[67] = Content.Load<Texture2D>("Blast (68)");
            DrawingBoard.Blast[68] = Content.Load<Texture2D>("Blast (69)");
            DrawingBoard.Blast[69] = Content.Load<Texture2D>("Blast (70)");
            DrawingBoard.Blast[70] = Content.Load<Texture2D>("Blast (71)");
            DrawingBoard.Blast[71] = Content.Load<Texture2D>("Blast (72)");
            DrawingBoard.Blast[72] = Content.Load<Texture2D>("Blast (73)");
            DrawingBoard.Blast[73] = Content.Load<Texture2D>("Blast (74)");

            DrawingBoard.Enemies[0, 1, 0] = Content.Load<Texture2D>("Bug1");
            DrawingBoard.Enemies[0, 2, 0] = Content.Load<Texture2D>("Bug2");
            DrawingBoard.Enemies[0, 3, 0] = Content.Load<Texture2D>("Bug3");
            DrawingBoard.Enemies[0, 4, 0] = Content.Load<Texture2D>("Bug4");
            #endregion
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            base.UnloadContent();
            spriteBatch.Dispose();
            white.Dispose();

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            if (this.IsActive != gameActive)
            {
                if (this.IsActive == false) {
                    graphics.IsFullScreen = false;
                    graphics.ApplyChanges();
                    gameActive 
                        = false; }
                else if (this.IsActive == true) {
                    graphics.IsFullScreen = true;
                    graphics.ApplyChanges();
                    gameActive = true; }
            }

            ////////////////////////////////////////////
            // Check for Pressed Keys to Move 1 Space //
            //  -Touch & Keyboard Movement Controls-  //
            ////////////////////////////////////////////

            gestureHolder = 0;
            if (actionPending == false)
            {
                if (TouchPanel.IsGestureAvailable) {
                    if (MainMenuOpen) {
                        if (NoMap) {
                            /*Client.Job job = new Client.Job((byte)Client.Net.UserList[0].JobList.Count(), 5, Client.Net.UserList[0].Endpoint, Client.Net.Endpoint);
                            Console.WriteLine($"Starting Job ID {Client.Net.UserList[0].JobList.Count()}");
                            Client.Net.UserList[0].JobList.Add(job);
                            var t = Task.Run(() => Client.Net.JobManager(job));
                            NoMap = false;*/ }}
                    else {
                        Control.MovementTouch(); }}

                newMouseState = Mouse.GetState();
                if (/*oldMouseState.LeftButton == ButtonState.Released && */newMouseState.LeftButton == ButtonState.Pressed) {
                    Control.Click(); }
                if (/*oldMouseState.RightButton == ButtonState.Released && */newMouseState.RightButton == ButtonState.Pressed) {
                    Control.ClickRight(); }
                oldMouseState = newMouseState;

                newState = Keyboard.GetState();
                if (newState != oldState) {
                    Control.Keyboard(); }
                oldState = newState;
            }
            else { Control.GlobalCooldown(); }

            if (playerAuto == true) {
                Control.AutoAI(Player.player); }

            rnd = Random.Next(0, 10000);
            if (rnd < 50 && Player.player.resources[10] > Player.Workers.Count) { // INCREASED WORKER SPAWN RATE FROM 5
                Player.Workers.Add(new Unit(0, 0, Player.Workers.Count, Generate.Worker())); }
            
            if (Player.LocalWorkers.Count > 0) {
                foreach (Unit unit in Player.LocalWorkers) {
                    Control.UnitManager(unit); ; }}

            if (Player.LocalEnemies.Count > 0) {
                foreach (Unit unit in Player.LocalEnemies) {
                    Control.UnitManager(unit); }}

            if (Player.Enemies.Count > 0) {
                foreach (Unit unit in Player.Enemies) {
                    if (unit.Stats[1] < 1) {
                        Player.LocalEnemies.Remove(unit);
                        Set.CoreStats(unit);
                        unit.ActionID = 0;
                    }}}

            // Quarter Second Interval Recurring Logic
            if (ScanTiles.ElapsedMilliseconds > 250)
            {
                // Scan Local Tiles for Logic Updates
                for (int y = 0; y < 100; y++)
                {
                    for (int x = 0; x < 100; x++)
                    {
                        int x2 = Check.Range((cameraLocationX + x - 30), 0, 1000);
                        int y2 = Check.Range((cameraLocationY + y - 40), 0, 1000);
                        Land land = landArray[x2, y2];

                        // currently Residents are expected to be an enemy
                        if (land.IsResident)
                        {
                            // who will emerge from his Residence if player is closer than 20 tiles
                            if (Math.Abs(Player.player.X - x2) < 20 && Math.Abs(Player.player.Y - y2) < 20 && land.Resident.ActionID == 9)
                            {
                                Player.LocalEnemies.Add(land.Resident);
                                Player.LocalEnemies[Player.LocalEnemies.Count - 1].Y += 1;
                                Player.LocalEnemies[Player.LocalEnemies.Count - 1].LastMove = 2;
                                Player.LocalEnemies[Player.LocalEnemies.Count - 1].ActionID = 4;
                                // landArray[x2, y2].Resident.ActionID = 0; // WHEN ENABLED ALSO CHANGED VALUE IN LOCALENEMIES, ARE VARIABLES LINKED?? //
                            }
                        }
                        else if (land.land > 199)
                        {
                            Unit resident = new Unit(x2, y2, Player.Enemies.Count, Generate.Worker());
                            Set.CoreStats(resident);
                            resident.ActionID = 9;
                            landArray[x2, y2].Resident = resident;
                            landArray[x2, y2].IsResident = true;
                            Player.Enemies.Add(landArray[x2, y2].Resident);
                        }
                        
                    }
                }

                ScanTiles.Restart();
            }

            if (cantBuild.ElapsedMilliseconds > 2000) {
                cantBuild.Reset(); }

            if (UpdateDestination.ElapsedMilliseconds > 250)
                UpdateDestination.Restart();

                base.Update(gameTime);
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (MainMenuOpen) {
                Show.MainMenu(); }
            else {
                Show.Interface(); }
            spriteBatch.End();

            base.Draw(gameTime);
        }




        /*
         temporary deer rotation

        public void CreatureAI(float i, int x, int y) // Deer Rotation i 0,1000
        {
            angle = (float)landArray[x, y].rotate;

            if (i < 5)
            {
                landArray[x, y].rotate = (float?)Math.PI / 2.0f * random;
            }
            else if (i < 20 && i >= 5)
            {
                if (landArray[x, y].rotate == (float?)4 * Math.PI / 2.0f && landArray[x, Check.Max(y + 1, 1000)].land == 0)
                {
                    landArray[x, y + 1].rotate = landArray[x, y].rotate;
                    landArray[x, y + 1].land = 4;// landArray[x, y].mod;
                    landArray[x, y].rotate = null;
                    landArray[x, y].land = 0;
                }
                else if (landArray[x, y].rotate == (float?)3 * Math.PI / 2.0f && landArray[Check.Max(x + 1, 1000), y].land == 0)
                {
                    landArray[x + 1, y].rotate = landArray[x, y].rotate;
                    landArray[x + 1, y].land = 4;// landArray[x, y].mod;
                    landArray[x, y].rotate = null;
                    landArray[x, y].land = 0;
                }
                else if (landArray[x, y].rotate == (float?)2 * Math.PI / 2.0f && landArray[x, Check.Min(y - 1, 0)].land == 0)
                {
                    landArray[x, y - 1].rotate = landArray[x, y].rotate;
                    landArray[x, y - 1].land = 4;// landArray[x, y].mod;
                    landArray[x, y].rotate = null;
                    landArray[x, y].land = 0;
                }
                else if (landArray[x, y].rotate == (float?)Math.PI / 2.0f && landArray[Check.Min(x - 1, 0), y].land == 0)
                {
                    landArray[x - 1, y].rotate = landArray[x, y].rotate;
                    landArray[x - 1, y].land = 4;// landArray[x, y].mod;
                    landArray[x, y].rotate = null;
                    landArray[x, y].land = 0;
                }
            }
            else if (i < 25 && i >= 20)
            {

            }
        }
        */

        // Call to rescale TileMap[] Rectangles to the current tileScale //
        /*
        public void ScaleTileMap()
        {
            for (int y = 0; y < ((displayHeight / 50) / tileScale) + 2 / tileScale; y++)
            {
                for (int x = 0; x < ((displayWidth / 50) / tileScale) + 2 / tileScale; x++)
                {
                    TileMap[x, y].X = x * (int)(50 * tileScale);
                    TileMap[x, y].Y = y * (int)(50 * tileScale);
                    TileMap[x, y].Height = (int)(50 * tileScale);
                    TileMap[x, y].Width = (int)(50 * tileScale);
                }
            }
        }
        */
    }
}
