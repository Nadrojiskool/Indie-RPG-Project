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
using System.Net.PeerToPeer;

namespace Game1
{
    public class Game1 : Game
    {
        public static int objland;
        public static int objbiome;
        public static int objframe;
        public static int objx;
        public static int objy;
        public static int objwidth;
        public static int objheight;
        public static double tileScale = .5;
        public GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static SpriteFont font;
        protected static Texture2D inventory;
        protected static Texture2D idCard;
        protected static Texture2D idCardBack;
        protected static Texture2D buildMenu;
        protected static Texture2D workerList;
        protected static Texture2D player;
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
        protected static Texture2D house_kame;
        protected static Texture2D mine;
        protected static Texture2D orbPillar;
        protected static Texture2D WallWoodHorizontal;
        protected static Texture2D WallWoodVertical;
        protected static Texture2D WallWoodCornerLeft;
        protected static Texture2D WallWoodCornerRight;
        protected static Texture2D WallWoodBackLeft;
        protected static Texture2D WallWoodBackRight;
        protected static bool invOpen = false;
        protected static bool buildMenuOpen = false;
        protected static bool workerListOpen = false;
        private KeyboardState newState;
        private KeyboardState oldState;
        protected static MouseState newMouseState;
        protected static MouseState oldMouseState;
        protected static Land[,] landArray = new Land[1000, 1000];
        protected static Land[,] tileArray = new Land[400, 250];
        protected static int cameraLocationX = 0;
        protected static int cameraLocationY = 0;
        private int tilePointerX = 0;
        private int tilePointerY = 0;
        //Random rnd = new Random();
        static Random Random = new Random();
        public float random = 0;
        private double month;
        private int counter;
        private float angle = 0;
        private int gestureHolder;
        public static int displayWidth;
        public static int displayHeight;
        //private String[] informationToWriteLand;
        public static byte[] receivedBytes = new byte[50000];
        public static String[] informationToWriteBiome;
        public static String[] informationToWriteMod;
        private String[] informationToWritePlayerResources;
        private String[] informationToWritePlayerStats;
        private String[] informationToWritePlayerWorkers;
        private Keys[] KeysMovement = new Keys[] { Keys.D, Keys.S, Keys.A, Keys.W };
        protected static Stopwatch actionTimer = new Stopwatch();
        protected static Stopwatch cantBuild = new Stopwatch();
        protected static bool actionPending = false;
        public int actionValue = 0;
        public bool playerAuto = false;
        private bool gameActive = true;

        int[,] MovementXY = new int[,] { { 0, 0 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        public Rectangle buildRect1 = new Rectangle(600, 650, 100, 150);
        public static Rectangle buildRect2 = new Rectangle(750, 650, 50, 100);
        public static Rectangle buildRect3 = new Rectangle(800, 650, 50, 100);
        public static Rectangle buildRect4 = new Rectangle(850, 650, 50, 100);
        public static Rectangle buildRect5 = new Rectangle(900, 650, 50, 100);
        public static Rectangle buildRect6 = new Rectangle(950, 650, 50, 100);
        public static Rectangle buildRect7 = new Rectangle(1000, 650, 50, 100);
        public static Rectangle buildRect8 = new Rectangle(1050, 650, 100, 100);
        
        public Rectangle[,] TileMap = new Rectangle[(int)(42 / tileScale), (int)(24 / tileScale)];
        
        public static IPEndPoint ServerEndpoint = new IPEndPoint(IPAddress.Parse("24.20.157.144"), 57000); // endpoint where server is listening
        public static UdpClient Client = new UdpClient(56000);
        public static bool messageReceived = false;
        public static bool messageStarted = false;
        public static bool messageCompleted = false;
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

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            TouchPanel.EnabledGestures = GestureType.Tap;
            Player.player = new Player((int)((20 * displayWidth / 1920) / tileScale), (int)((10 * displayHeight / 1080) / tileScale), Generate.Worker());
            Player.player.tileX = (int)((20 * displayWidth / 1920) / tileScale);
            Player.player.tileY = (int)((10 * displayHeight / 1080) / tileScale);
            Player.player.DrawX = (int)((20 * displayWidth / 1920) / tileScale) * (int)(50 * tileScale) + (int)(25 * tileScale);
            Player.player.DrawY = (int)((10 * displayHeight / 1080) / tileScale) * (int)(50 * tileScale) + (int)(25 * tileScale);
            Object.Initialize();

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

            Generate.All(landArray);
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
            inventory = Content.Load<Texture2D>("Inventory");
            idCard = Content.Load<Texture2D>("IDCard");
            idCardBack = Content.Load<Texture2D>("IDCardBack");
            buildMenu = Content.Load<Texture2D>("ui_3");
            workerList = Content.Load<Texture2D>("ui_4");
            house_kame = Content.Load<Texture2D>("House (Kame)");
            mine = Content.Load<Texture2D>("Mine");
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
            DrawingBoard.Tiles[201, 1, 5] = house_kame;
            DrawingBoard.Tiles[201, 2, 5] = house_kame;
            DrawingBoard.Tiles[202, 1, 5] = mine;
            DrawingBoard.Tiles[202, 2, 5] = mine;
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
                    gameActive = false; }
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
                    MovementTouch(); }

                newMouseState = Mouse.GetState();
                if (oldMouseState.LeftButton == ButtonState.Released && newMouseState.LeftButton == ButtonState.Pressed) {
                    HandlerClick(); }
                if (oldMouseState.RightButton == ButtonState.Released && newMouseState.RightButton == ButtonState.Pressed) {
                    HandlerClickRight(); }
                oldMouseState = newMouseState;

                newState = Keyboard.GetState();
                if (newState != oldState) {
                    HandlerKeys(); }
                oldState = newState;
            }
            else {  GlobalCooldown(); }

            if (playerAuto == true) {
                AutoAI(Player.player); }

            month = Random.Next(0, 10000);
            if (month < 5 && Player.player.resources[10] > Player.Units.Count) {
                Player.Units.Add(new Unit(0, 0, Player.Units.Count, Generate.Worker())); }
            
            if (Unit.Active.Count > 0) {
                for (int i = 0; i < Unit.Active.Count; i++) {
                    AutoAI(Unit.Active[i]); } }

            if (cantBuild.ElapsedMilliseconds > 2000) {
                cantBuild.Reset(); }

            base.Update(gameTime);
        }

        void GlobalCooldown()
        {
            if (actionTimer.ElapsedMilliseconds > 100) {
                actionPending = false;
                actionTimer.Reset();
                Mine(actionValue);
                actionValue = 0; }
        }

        void Movement(Unit unit)
        {
            unit.Rotation = (float)unit.LastMove * ((float)Math.PI / 2.0f);
            if (unit == Player.player) {
                cameraLocationX = Check.Range(cameraLocationX + MovementXY[unit.LastMove, 0], 0, 1000);
                cameraLocationY = Check.Range(cameraLocationY + MovementXY[unit.LastMove, 1], 0, 1000);
                Player.player.X = Check.Range(Player.player.X + MovementXY[unit.LastMove, 0], 40, 960);
                Player.player.Y = Check.Range(Player.player.Y + MovementXY[unit.LastMove, 1], 20, 980); }
            else {
                unit.X = Check.Range(unit.X + MovementXY[unit.LastMove, 0], 0, 1000);
                unit.Y = Check.Range(unit.Y + MovementXY[unit.LastMove, 1], 0, 1000); }
        }

        /// /////////////////////////////////
        // UNTESTED CODE, MOVEMENT CONTROLS//
        /// /////////////////////////////////

        void MovementTouch()
        {
            var gesture = TouchPanel.ReadGesture();
            if (gesture.Position.X > displayWidth * 1 / 4 && gesture.Position.X < displayWidth * 3 / 4 &&
                gesture.Position.Y > displayHeight * 1 / 4 && gesture.Position.Y < displayHeight * 3 / 4)
                { Mine(0); }
            else
            {
                if (gesture.Position.X > displayWidth * 3 / 4 && cameraLocationX < 1000 - (displayWidth / 50) / tileScale - 2 / tileScale) {
                    if (landArray[Check.Max(cameraLocationX + Player.player.tileX + 1, 1000), cameraLocationY + Player.player.tileY].land == 0) {
                        cameraLocationX = cameraLocationX + 1;
                        Player.player.X++; }
                    Player.player.Rotation = (float)Math.PI / 2.0f;
                    Player.player.LastMove = 1; }

                if (gesture.Position.X < displayWidth * 1 / 4 && cameraLocationX > 0) {
                    if (landArray[Check.Min(cameraLocationX + Player.player.tileX - 1, 0), cameraLocationY + Player.player.tileY].land == 0) {
                        cameraLocationX = cameraLocationX - 1;
                        Player.player.X--; }
                    Player.player.Rotation = 3 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 3; }

                if (gesture.Position.Y > displayHeight * 3 / 4 && cameraLocationY < 1000 - (displayHeight / 50) / tileScale - 2 / tileScale) {
                    if (landArray[cameraLocationX + Player.player.tileX, Check.Max(cameraLocationY + Player.player.tileY + 1, 1000)].land == 0) {
                        cameraLocationY = cameraLocationY + 1;
                        Player.player.Y++; }
                    Player.player.Rotation = 2 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 2; }

                if (gesture.Position.Y < displayHeight * 1 / 4 && cameraLocationY > 0) {
                    if (landArray[cameraLocationX + Player.player.tileX, Check.Min(cameraLocationY + Player.player.tileY - 1, 0)].land == 0) {
                        cameraLocationY = cameraLocationY - 1;
                        Player.player.Y--; }
                    Player.player.Rotation = 4 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 4; }
            }
        }

        void HandlerClick()
        {
            if (invOpen == true) {
                invOpen = false; }
            else if (buildMenuOpen == true)
            {
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
                else { cantBuild.Start(); }

                buildMenuOpen = false;
            }
            else if (invOpen == false && buildMenuOpen == false)
            {
                int spellX = (newMouseState.X / (int)(50 * tileScale) + cameraLocationX) - 2;
                int spellY = (newMouseState.Y / (int)(50 * tileScale) + cameraLocationY) - 2;
                TileManipulator(spellX, spellY, 5, 5, 0, false);
            }
        }

        void HandlerClickRight()
        {
            if (invOpen == false && buildMenuOpen == false)
            {
                int spellX = (newMouseState.X / (int)(50 * tileScale) + cameraLocationX) - 2;
                int spellY = (newMouseState.Y / (int)(50 * tileScale) + cameraLocationY) - 2;
                TileManipulator(spellX, spellY, 5, 5, 5, false);
            }
            if (invOpen == true)
            {
                //invOpen = false;
            }
            if (buildMenuOpen == true)
            {
                /*if (buildRect1.Contains(newMouseState.X, newMouseState.Y))
                {
                    Mine(201);
                }
                else if (buildRect2.Contains(newMouseState.X, newMouseState.Y))
                {
                    Mine(101);
                }*/
                buildMenuOpen = false;
            }
            if (workerListOpen == true)
            {
                // NOTE TO SELF // ADD START BIAS // i.e. Workers favor mining in the direction placed
                if (Unit.Active.Count < Player.Units.Count)
                {
                    Unit.Active.Add(Player.Units[Unit.Active.Count]);
                    Unit.Active[Unit.Active.Count - 1].X = Player.player.X + MovementXY[Player.player.LastMove, 0];
                    Unit.Active[Unit.Active.Count - 1].Y = Player.player.Y + MovementXY[Player.player.LastMove, 1];
                    landArray[Unit.Active[Unit.Active.Count - 1].X, Unit.Active[Unit.Active.Count - 1].Y].IsOccupied = true;
                }
                workerListOpen = false;
            }
        }

        void HandlerKeys()
        {
            if (newState.IsKeyDown(Keys.LeftShift)) {
                HandlerKeyShift(); }
            else if (newState.IsKeyDown(Keys.LeftControl)) {
                HandlerKeyControl(); }
            else if (oldState.IsKeyUp(Keys.Space) && newState.IsKeyDown(Keys.Space))
            {
                if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], 
                    cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].land == 202)
                    {  Player.player.depth++; }
                else {
                    Player.player.depth = 0;
                    actionPending = true;
                    actionTimer.Start(); }
            }
            else if (oldState.IsKeyUp(Keys.I) && newState.IsKeyDown(Keys.I))
            {
                if (invOpen == true) {
                    invOpen = false; }
                else { invOpen = true; }
            }
            else if (oldState.IsKeyUp(Keys.M) && newState.IsKeyDown(Keys.M))
            {
                if (tileScale != .5) {
                    tileScale = .5; }
                else { tileScale = 2; }
                //ScaleTileMap();
            }
            else if (oldState.IsKeyUp(Keys.B) && newState.IsKeyDown(Keys.B))
            {
                if (buildMenuOpen == true) {
                    buildMenuOpen = false; }
                else { buildMenuOpen = true; }
                //Mine(7);
            }
            else if (oldState.IsKeyUp(Keys.E) && newState.IsKeyDown(Keys.E))
            {
                if (workerListOpen == true) {
                    workerListOpen = false; }
                else { workerListOpen = true; }
            }
            else
            {
                Keys[] keysHolder = newState.GetPressedKeys();
                int keyCheck;
                for (int i = 0; i < keysHolder.Length; i++)
                {
                    keyCheck = Array.IndexOf(KeysMovement, keysHolder[i]);
                    if (keyCheck >= 0 /*&& oldState.IsKeyUp(keysHolder[i])*/)
                    {
                        Player.player.LastMove = keyCheck + 1;
                        Player.player.Rotation = (float)Player.player.LastMove * ((float)Math.PI / 2.0f);
                        if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], 
                            cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].land == 0)
                            { Movement(Player.player); }
                    }
                }
            }
        }

        void HandlerKeyShift()
        {
            if (newState.IsKeyDown(Keys.L)) {
                DataLoad(true); }
        }

        void HandlerKeyControl()
        {
            if (newState.IsKeyDown(Keys.S)) {
                DataSave(); }
            else if (newState.IsKeyDown(Keys.L)) {
                DataLoad(false); }
            else if (oldState.IsKeyUp(Keys.A) && newState.IsKeyDown(Keys.A))
            {
                if (playerAuto == false) {
                    playerAuto = true; }
                else {
                    Player.player.AutoX = 0;
                    Player.player.AutoY = 0;
                    landArray[Player.player.X, Player.player.Y].IsOccupied = false;
                    landArray[Player.player.X + MovementXY[Player.player.LastMove, 0], Player.player.Y + MovementXY[Player.player.LastMove, 1]].IsActive = false;
                    playerAuto = false; }
            }
            //informationToWriteLand = null;
            informationToWriteBiome = null;
            informationToWriteMod = null;
            informationToWritePlayerResources = null;
            informationToWritePlayerStats = null;
            informationToWritePlayerWorkers = null;
        }

        void DataSave()
        {
            //informationToWriteLand = new String[1000000];
            informationToWriteBiome = new String[1000000];
            informationToWriteMod = new String[1000000];
            informationToWritePlayerResources = new String[1000];
            informationToWritePlayerStats = new String[200];
            informationToWritePlayerWorkers = new String[Player.Units.Count * 200];
            int[] array = new int[200];
            counter = 0;
            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    //informationToWriteLand[counter + x] = landArray[x, y].land.ToString();
                    informationToWriteBiome[counter + x] = landArray[x, y].biome.ToString();
                    informationToWriteMod[counter + x] = landArray[x, y].land.ToString();
                    if (counter == 0) {
                        informationToWritePlayerResources[x] = Player.player.resources[x].ToString();
                        if (x < 200) {
                            informationToWritePlayerStats[x] = Player.player.stats[x].ToString(); }}
                }
                counter = counter + 1000;
            }

            //Player.player.Workers.CopyTo(informationToWritePlayerWorkers);

            counter = 0;
            for (int y = 0; y < Player.Units.Count; y++)
            {
                array = Player.Units[y].stats;
                for (int x = 0; x < 200; x++) {
                    informationToWritePlayerWorkers[counter + x] = array[x].ToString(); }
                counter = counter + 200;
            }

            //File.WriteAllLines("C:/Users/2/Desktop/test1land.txt", informationToWriteLand); // Change the file path here to where you want it.
            File.WriteAllLines("C:/Users/2/Desktop/test1biome.txt", informationToWriteBiome);
            File.WriteAllLines("C:/Users/2/Desktop/test1mod.txt", informationToWriteMod);
            File.WriteAllLines("C:/Users/2/Desktop/test1resources.txt", informationToWritePlayerResources);
            File.WriteAllLines("C:/Users/2/Desktop/test1stats.txt", informationToWritePlayerStats);
            File.WriteAllLines("C:/Users/2/Desktop/test1workers.txt", informationToWritePlayerWorkers);
        }

        void DataLoad(bool isServer)
        {
            //informationToWriteLand = new String[1000000];
            //informationToWriteLand = File.ReadAllLines("C:/Users/2/Desktop/test1land.txt"); // Change the file path here to where you want it.
            informationToWriteBiome = new String[1000000];
            informationToWriteMod = new String[1000000];
            if (!isServer)
            {
                informationToWriteBiome = File.ReadAllLines("C:/Users/2/Desktop/test1biome.txt");
                informationToWriteMod = File.ReadAllLines("C:/Users/2/Desktop/test1mod.txt");
            }
            else
            {
                messageCompleted = false;

                Client.Send(new byte[] { 10, 1 }, 2);
                ReceiveDataLoad();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (!messageStarted)
                {
                    if (stopwatch.ElapsedMilliseconds > 1000) {
                        Client.Send(new byte[] { 10, 1 }, 2);
                        stopwatch.Restart(); }
                }
                stopwatch.Reset();

                while (!messageCompleted) {
                    Thread.Sleep(50); }

                messageStarted = false;
                messageCompleted = false;
                isServer = false;
            }
            informationToWritePlayerResources = new String[1000];
            informationToWritePlayerResources = File.ReadAllLines("C:/Users/2/Desktop/test1resources.txt");
            informationToWritePlayerStats = new String[200];
            informationToWritePlayerStats = File.ReadAllLines("C:/Users/2/Desktop/test1stats.txt");
            int[] array = new int[200];
            counter = 0;
            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    //landArray[x, y].land = Int32.Parse(informationToWriteLand[counter + x]);
                    landArray[x, y].biome = Int32.Parse(informationToWriteBiome[counter + x]);
                    landArray[x, y].land = Int32.Parse(informationToWriteMod[counter + x]);
                    landArray[x, y].IsActive = false;

                    if (landArray[x, y].land == 4) {
                        landArray[x, y].rotate = 0; }
                    else if (landArray[x, y].land == 5) {
                        landArray[x, y].frame = 5; }
                    else {
                        landArray[x, y].rotate = null; }

                    if (counter == 0) {
                        Player.player.resources[x] = Int32.Parse(informationToWritePlayerResources[x]);
                        if (x < 200) { Player.player.stats[x] = Int32.Parse(informationToWritePlayerStats[x]); }}
                }
                counter = counter + 1000;
            }
            counter = 0;
            Player.Units.Clear();
            Unit.Active.Clear();
            informationToWritePlayerWorkers = File.ReadAllLines("C:/Users/2/Desktop/test1workers.txt");
            for (int y = 0; y < informationToWritePlayerWorkers.Length / 200; y++)
            {
                for (int x = 0; x < 200; x++) {
                    array[x] = Int32.Parse(informationToWritePlayerWorkers[counter + x]); }

                Player.Units.Add(new Unit(0, 0, Player.Units.Count, array));
                counter = counter + 200;
            }
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Show.Interface();

            base.Draw(gameTime);
        }

        public void Mine(int a)
        {
            if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].land != 1)
            {
                if (a > 0) {
                    actionPending = true;
                    actionValue = -a;
                    actionTimer.Start(); }
                else if (a < 0)
                {
                    if (Math.Abs(a) == 201 || Math.Abs(a) == 202) {
                        Player.player.resources[10] = Player.player.resources[10] + 4; }

                    if (MovementXY[Player.player.LastMove, 0] > 0 || MovementXY[Player.player.LastMove, 1] > 0) {
                        TileManipulator(cameraLocationX + Player.player.tileX + (Object.Objects[Math.Abs(a)].Width * MovementXY[Player.player.LastMove, 0]), 
                            cameraLocationY + Player.player.tileY + (Object.Objects[Math.Abs(a)].Height * MovementXY[Player.player.LastMove, 1]), 
                            Object.Objects[Math.Abs(a)].Width, Object.Objects[Math.Abs(a)].Height, 1, true);
                        landArray[cameraLocationX + Player.player.tileX + (Object.Objects[Math.Abs(a)].Width * MovementXY[Player.player.LastMove, 0]), 
                            cameraLocationY + Player.player.tileY + (Object.Objects[Math.Abs(a)].Height * MovementXY[Player.player.LastMove, 1])].land = Math.Abs(a); }
                    else {
                        TileManipulator(cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], 
                            cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1], Object.Objects[Math.Abs(a)].Width, 
                            Object.Objects[Math.Abs(a)].Height, 1, true);
                        landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], 
                            cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].land = Math.Abs(a); }
                }
                else {
                    Gather(cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], 
                        cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1], Player.player); }
            }
        }

        public void AutoAI(Unit unit)
        {
            if (unit.ActionTime.IsRunning == false)
            {
                if (unit.AutoX > 0)
                {
                    if (unit.Y < unit.AutoY)
                    {
                        if (unit.Rotation != 2 * (float)Math.PI / 2.0f) {
                            unit.Rotation = 2 * (float)Math.PI / 2.0f;
                            unit.LastMove = 2; }

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
                        if (unit.Rotation != 4 * (float)Math.PI / 2.0f) {
                            unit.Rotation = 4 * (float)Math.PI / 2.0f;
                            unit.LastMove = 4; }

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
                        if (unit.Rotation != (float)Math.PI / 2.0f) {
                            unit.Rotation = (float)Math.PI / 2.0f;
                            unit.LastMove = 1; }

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
                        if (unit.Rotation != 3 * (float)Math.PI / 2.0f) {
                            unit.Rotation = 3 * (float)Math.PI / 2.0f;
                            unit.LastMove = 3; }

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
                        if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false) {
                            unit.AutoX = x;
                            unit.AutoY = y;
                            return; }

                        for (int c = 0; c < (a * 2); c++) {
                            x++;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false) {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return; }}

                        for (int c = 0; c < (a * 2); c++) {
                            y++;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false) {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return; }}

                        for (int c = 0; c < (a * 2); c++) {
                            x--;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false) {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return; }}

                        for (int c = 0; c < (a * 2); c++) {
                            y--;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].land > 2 && landArray[x, y].land < 100 && landArray[x, y].IsActive == false) {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return; }}
                    }
                }
            }
            else
            {
                if (unit.ActionTime.ElapsedMilliseconds > unit.ActionDuration) {
                    if (unit.ActionTime.ElapsedMilliseconds > 10000) {
                        Gather(unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1], unit);
                        landArray[unit.X + MovementXY[unit.LastMove, 0], unit.Y + MovementXY[unit.LastMove, 1]].IsActive = false; }
                    unit.ActionTime.Reset(); }
            }
        }

        public void Gather(int a, int b, Unit unit)
        {
            if (landArray[a, b].land != 0)
            {
                unit.stats[0]++;
                Player.player.resources[landArray[a, b].land]++;

                if (landArray[a, b].land == 2 || landArray[a, b].land == 3) {
                    unit.stats[11]++; }
                else if (landArray[a, b].land == 4) {
                    unit.stats[18]++;
                    unit.stats[19]++;
                    unit.stats[20]++;
                    unit.stats[21]++; }
                else if (landArray[a, b].land == 5) {
                    unit.stats[12]++; }
                else if (landArray[a, b].land == 6) {
                    unit.stats[14]++; }
                else if (landArray[a, b].land > 6) {
                    unit.stats[17]++;
                    if (landArray[a, b].land == 202) {
                        unit.depth = 1; }}
                landArray[a, b].land = 0;
            }
        }

        public void TileManipulator(int x, int y, int width, int height, int land, bool inversion)
        {
            if (inversion == true) {
                for (int a = 0; a < height; a++) {
                    for (int b = 0; b < width; b++) {
                        if (landArray[x - a, y - b].land != land) {
                            if (landArray[x - a, y - b].land != 0) {
                                Gather(x - a, y - b, Player.player); }
                            landArray[x - a, y - b].land = land; }}}}
            else {
                for (int a = 0; a < height; a++) {
                    for (int b = 0; b < width; b++) {
                        if (landArray[x + a, y + b].land != land) {
                            if (landArray[x + a, y + b].land != 0) {
                                Gather(x + a, y + b, Player.player); }
                            landArray[x + a, y + b].land = land;}}}}
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

        public static async Task ReceiveDataLoad()
        {
            for (int i = 0; i < 20; i++)
            {
                messageReceived = false;

                PacketWaiter();

                messageStarted = true;
                Client.Send(new byte[] { 2 }, 1);

                while (!messageReceived)
                {
                    Thread.Sleep(50);
                }

                for (int ii = 0; ii < 50000; ii++)
                {
                    informationToWriteBiome[(i * 50000) + ii] = receivedBytes[ii].ToString();
                }

                messageReceived = false;
            }

            for (int i = 0; i < 20; i++)
            {
                messageReceived = false;

                PacketWaiter();

                Client.Send(new byte[] { 2 }, 1);

                while (!messageReceived)
                {
                    Thread.Sleep(50);
                }

                for (int ii = 0; ii < 50000; ii++)
                {
                    informationToWriteMod[(i * 50000) + ii] = receivedBytes[ii].ToString();
                }

                messageReceived = false;
            }

            messageCompleted = true;
        }

        public static async Task PacketWaiter()
        {
            receivedBytes = await Task.Run(() => GrabPacket());
            messageReceived = true;
        }

        public static byte[] GrabPacket()
        {
            byte[] b = new byte[50000];
            b = Client.Receive(ref ServerEndpoint);
            return b;
        }
    }
}
