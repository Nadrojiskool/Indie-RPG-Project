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
        public static double tileScale = .5;
        public GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public SpriteFont font;
        private Texture2D white;
        private Texture2D land;
        private Texture2D water;
        public Texture2D player;
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
        private Texture2D inventory;
        private Texture2D idCard;
        private Texture2D idCardBack;
        private Texture2D buildMenu;
        private Texture2D workerList;
        private Texture2D house_kame;
        private Texture2D mine;
        private Texture2D orbPillar;
        private Texture2D WallWoodHorizontal;
        private Texture2D WallWoodVertical;
        private Texture2D WallWoodCornerLeft;
        private Texture2D WallWoodCornerRight;
        private Texture2D WallWoodBackLeft;
        private Texture2D WallWoodBackRight;
        public bool invOpen = false;
        public static bool buildMenuOpen = false;
        public static bool workerListOpen = false;
        private KeyboardState newState;
        private KeyboardState oldState;
        private MouseState newMouseState;
        private MouseState oldMouseState;
        private Land[,] landArray = new Land[1000, 1000];
        private Land[,] tileArray = new Land[400, 250];
        private int cameraLocationX = 0;
        private int cameraLocationY = 0;
        private int tilePointerX = 0;
        private int tilePointerY = 0;
        Random rnd = new Random();
        public float random = 0;
        private double month;
        private Land checkPosX;
        private Land checkPosY;
        private Land checkNegX;
        private Land checkNegY;
        private Land checkTile;
        private int counter;
        private int biomeHolder = 0;
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
        private Stopwatch actionTimer = new Stopwatch();
        private Stopwatch cantBuild = new Stopwatch();
        public bool actionPending = false;
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
        
        public static IPEndPoint Endpoint = new IPEndPoint(IPAddress.Parse("24.20.157.144"), 57000); // endpoint where server is listening
        public static UdpClient Client = new UdpClient();
        public static bool messageReceived = false;

        public struct UdpState
        {
            public IPEndPoint Endpoint;
            public UdpClient Client;
        }

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
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            TouchPanel.EnabledGestures = GestureType.Tap;
            Player.player = new Player((int)((20 * displayWidth / 1920) / tileScale), (int)((10 * displayHeight / 1080) / tileScale), GenerateWorker());
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
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //framerate = Content.Load<SpriteFont>("Framerate");

            LoadSprites();

            /*
             * The following statements will populate the land values of the default array 1000 x 1000 
             * as well as the tile cache array currently 400 x 250 which will
             * support the tile scale .1 (10x) on most resolution scales
             * ^^--pending optimization--^^
             * a roll is done to determine some land values as well as if the tile will be a new biome
             * and then check the previously drawn X & Y tiles for water or trees to increase clumping
             * before going on to generate full biomes, to scale 
             * the biomes are then expanded on with more complex near-by random tile generation 
             * done through multiple rescans of the default global land array for definition
             * default snow i = 40
             * default ore i = 5
             */

            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    month = rnd.Next(0, 10000);

                    //////////////////////////////////////
                    // Instantiate On-Screen Land Tiles //
                    //////////////////////////////////////

                    if (x < 400 && y < 250)
                    {
                        tileArray[x, y] = new Land(0, 0, 0);
                    }

                    /////////////////////////
                    // New Biome Initiator //
                    /////////////////////////

                    if (month < 5) { biomeHolder = 0; }
                    else { biomeHolder = 1; }

                    /////////////////////////////////////////////////////////////
                    // Randomly Generate and Instantiate Land Stored in Arrays //
                    /////////////////////////////////////////////////////////////

                    if (month <= 9390)
                    {
                        landArray[x, y] = new Land(1, biomeHolder, 0);
                        landArray[x, y].depth[0] = 0;
                    }
                    else if (month > 9390 && month <= 9400)
                    {
                        landArray[x, y] = new Land(1, biomeHolder, 0);
                        landArray[x, y].depth[0] = 2;
                    }
                    else if (month > 9400 && month <= 9800)
                    {
                        landArray[x, y] = new Land(1, biomeHolder, 0);
                        landArray[x, y].depth[0] = 3;
                    }
                    else if (month > 9800 && month <= 9900)
                    {
                        landArray[x, y] = new Land(1, biomeHolder, 0);
                        landArray[x, y].depth[0] = 4;
                    }
                    else if (month > 9900 && month <= 9995)
                    {
                        landArray[x, y] = new Land(1, biomeHolder, 0);
                        landArray[x, y].depth[0] = 5;
                    }
                    else if (month > 9995)
                    {
                        landArray[x, y] = new Land(1, biomeHolder, 0);
                        landArray[x, y].depth[0] = -6;
                    }

                    /////////////////////
                    // Realistic Water //
                    /////////////////////

                    if (landArray[CheckMin(x - 1, 0), y].depth[0] == 2 && month < 5500)
                    {
                        landArray[x, y].depth[0] = 2;

                    }
                    else if (landArray[x, CheckMin(y - 1, 0)].depth[0] == 2 && month < 5500)
                    {
                        landArray[x, y].depth[0] = 2;
                    }
                    else if (landArray[x, CheckMin(y - 1, 0)].depth[0] == 2 && landArray[CheckMin(x - 1, 0), y].depth[0] == 2 && month < 9920)
                    {
                        landArray[x, y].depth[0] = 2;
                    }

                    ///////////////////////
                    // Realistic Forests //
                    ///////////////////////

                    if (landArray[CheckMin(x - 1, 0), y].depth[0] == 5 && month < 3500)
                    {
                        landArray[x, y].depth[0] = 5;
                    }
                    else if (landArray[x, CheckMin(y - 1, 0)].depth[0] == 5 && month < 3500)
                    {
                        landArray[x, y].depth[0] = 5;
                    }
                    else if (landArray[x, CheckMin(y - 1, 0)].depth[0] == 5 && landArray[CheckMin(x - 1, 0), y].depth[0] == 5 && month < 9600)
                    {
                        landArray[x, y].depth[0] = 5;
                    }

                }
            }

            /////////////////////
            // Generate Biomes //
            /////////////////////

            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    if (landArray[x, y].biome == 0)
                    {
                        month = rnd.Next(2, 3);
                        if (month == 2)
                        {
                            landArray[x, y].biome = 2;
                        }
                        GenerateBiome(x - 1, y - 1, (int)month);
                    }
                    if (landArray[x, y].depth[0] == -6)
                    {
                        landArray[x, y].depth[0] = 6;
                        GenerateMod(x - 1, y - 1, 6);
                    }
                }
            }

            /////////////////////////////////////
            // Expanded Snow || Default i < 50 //
            /////////////////////////////////////

            for (int i = 0; i < 50; i++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    for (int x = 0; x < 1000; x++)
                    {
                        checkTile = landArray[x, y];
                        checkPosX = landArray[CheckMax(x + 1, 999), y];
                        checkPosY = landArray[x, CheckMax(y + 1, 999)];
                        checkNegX = landArray[CheckMin(x - 1, 0), y];
                        checkNegY = landArray[x, CheckMin(y - 1, 0)];
                        if (checkTile.biome != 2)
                        {
                            month = rnd.Next(0, 1000);
                            if (checkNegX.biome == 2)
                            {
                                month = month * 1.25;
                            }
                            if (checkNegY.biome == 2)
                            {
                                month = month * 1.25;
                            }
                            if (checkPosX.biome == 2)
                            {
                                month = month * 1.25;
                            }
                            if (checkPosY.biome == 2)
                            {
                                month = month * 1.25;
                            }
                            if (month > 1120)
                            {
                                landArray[x, y].biome = 2;
                            }
                        }
                    }
                }
            }

            ////////////////////////////////////
            // Sparse Nodes || Default i < 5 //
            ////////////////////////////////////

            for (int i = 0; i < 5; i++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    for (int x = 0; x < 1000; x++)
                    {
                        checkTile = landArray[x, y];
                        checkPosX = landArray[CheckMax(x + 1, 999), y];
                        checkPosY = landArray[x, CheckMax(y + 1, 999)];
                        checkNegX = landArray[CheckMin(x - 1, 0), y];
                        checkNegY = landArray[x, CheckMin(y - 1, 0)];
                        if (landArray[x, y].biome != 3)
                        {
                            month = rnd.Next(0, 1000);
                            if (landArray[CheckMin(x - 1, 0), y].depth[0] == 6 && month > 650)
                            {
                                landArray[x, y].depth[0] = 6;
                            }
                            if (landArray[x, CheckMin(y - 1, 0)].depth[0] == 6 && month > 650)
                            {
                                landArray[x, y].depth[0] = 6;
                            }
                            if (landArray[CheckMax(x + 1, 999), y].depth[0] == 6 && month > 650)
                            {
                                landArray[x, y].depth[0] = 6;
                            }
                            if (landArray[x, CheckMax(y + 1, 999)].depth[0] == 6 && month > 650)
                            {
                                landArray[x, y].depth[0] = 6;
                            }
                        }
                        if (checkTile.depth[0] != 5 && checkTile.depth[0] != 2)
                        {
                            month = rnd.Next(0, 1000);
                            if (checkNegX.depth[0] == 5)
                            {
                                month = month * 1.5;
                            }
                            if (checkNegY.depth[0] == 5)
                            {
                                month = month * 1.5;
                            }
                            if (checkPosX.depth[0] == 5)
                            {
                                month = month * 1.5;
                            }
                            if (checkPosY.depth[0] == 5)
                            {
                                month = month * 1.5;
                            }
                            if (month > 1050)
                            {
                                checkTile.depth[0] = 5;
                            }
                        }
                    }
                }
            }

        }

        void LoadSprites()
        {

            /////////////////////////
            // Cache Sprite Assets //
            /////////////////////////

            font = Content.Load<SpriteFont>("SpriteFont");

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
                if (this.IsActive == false)
                {
                    graphics.IsFullScreen = false;
                    graphics.ApplyChanges();
                    gameActive = false;
                }
                else if (this.IsActive == true)
                {
                    graphics.IsFullScreen = true;
                    graphics.ApplyChanges();
                    gameActive = true;
                }
            }

            ////////////////////////////////////////////
            // Check for Pressed Keys to Move 1 Space //
            //  -Touch & Keyboard Movement Controls-  //
            ////////////////////////////////////////////

            gestureHolder = 0;
            if (actionPending == false)
            {
                if (TouchPanel.IsGestureAvailable)
                {
                    MovementTouch();
                }

                newMouseState = Mouse.GetState();
                if (oldMouseState.LeftButton == ButtonState.Released && newMouseState.LeftButton == ButtonState.Pressed)
                {
                    HandlerClick();
                }
                if (oldMouseState.RightButton == ButtonState.Released && newMouseState.RightButton == ButtonState.Pressed)
                {
                    HandlerClickRight();
                }
                oldMouseState = newMouseState;

                newState = Keyboard.GetState();
                if (newState != oldState)
                {
                    HandlerKeys();
                }
                oldState = newState;
            }
            else
            {
                GlobalCooldown();
            }
            if (playerAuto == true)
            {
                AutoAI(Player.player);
            }

            month = rnd.Next(0, 10000);
            if (month < 5 && Player.player.resources[10] > Player.Units.Count)
            {
                // Units unit = new Units(0, 0, Player.Units.Count, GenerateWorker()); //
                Player.Units.Add(new Units(0, 0, Player.Units.Count, GenerateWorker()));
            }
            
            if (Units.Active.Count > 0)
            {
                for (int i = 0; i < Units.Active.Count; i++)
                {
                    AutoAI(Units.Active[i]);
                }
            }

            if (cantBuild.ElapsedMilliseconds > 2000)
            {
                cantBuild.Reset();
            }

            base.Update(gameTime);
        }

        void GlobalCooldown()
        {
            if (actionTimer.ElapsedMilliseconds > 100)
            {
                actionPending = false;
                actionTimer.Reset();
                Mine(actionValue);
                actionValue = 0;
            }
        }

        void Movement(Units unit)
        {
            unit.Rotation = (float)unit.LastMove * ((float)Math.PI / 2.0f);
            if (unit == Player.player)
            {
                cameraLocationX = CheckMinMax(cameraLocationX + MovementXY[unit.LastMove, 0], 0, 1000);
                cameraLocationY = CheckMinMax(cameraLocationY + MovementXY[unit.LastMove, 1], 0, 1000);
                Player.player.X = CheckMinMax(Player.player.X + MovementXY[unit.LastMove, 0], 40, 960);
                Player.player.Y = CheckMinMax(Player.player.Y + MovementXY[unit.LastMove, 1], 20, 980);
            }
            else
            {
                unit.X = CheckMinMax(unit.X + MovementXY[unit.LastMove, 0], 0, 1000);
                unit.Y = CheckMinMax(unit.Y + MovementXY[unit.LastMove, 1], 0, 1000);
            }
        }

        /// /////////////////////////////////
        // UNTESTED CODE, MOVEMENT CONTROLS//
        /// /////////////////////////////////

        void MovementTouch()
        {
            var gesture = TouchPanel.ReadGesture();
            if (gesture.Position.X > displayWidth * 1 / 4 && gesture.Position.X < displayWidth * 3 / 4 &&
                gesture.Position.Y > displayHeight * 1 / 4 && gesture.Position.Y < displayHeight * 3 / 4)
            {
                Mine(0);
            }
            else
            {
                if (gesture.Position.X > displayWidth * 3 / 4 && cameraLocationX < 1000 - (displayWidth / 50) / tileScale - 2 / tileScale)
                {
                    if (landArray[CheckMax(cameraLocationX + Player.player.tileX + 1, 1000), cameraLocationY + Player.player.tileY].depth[0] == 0)
                    {
                        cameraLocationX = cameraLocationX + 1;
                        Player.player.X++;
                    }
                    Player.player.Rotation = (float)Math.PI / 2.0f;
                    Player.player.LastMove = 1;
                }
                if (gesture.Position.X < displayWidth * 1 / 4 && cameraLocationX > 0)
                {
                    if (landArray[CheckMin(cameraLocationX + Player.player.tileX - 1, 0), cameraLocationY + Player.player.tileY].depth[0] == 0)
                    {
                        cameraLocationX = cameraLocationX - 1;
                        Player.player.X--;
                    }
                    Player.player.Rotation = 3 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 3;
                }
                if (gesture.Position.Y > displayHeight * 3 / 4 && cameraLocationY < 1000 - (displayHeight / 50) / tileScale - 2 / tileScale)
                {
                    if (landArray[cameraLocationX + Player.player.tileX, CheckMax(cameraLocationY + Player.player.tileY + 1, 1000)].depth[0] == 0)
                    {
                        cameraLocationY = cameraLocationY + 1;
                        Player.player.Y++;
                    }
                    Player.player.Rotation = 2 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 2;
                }
                if (gesture.Position.Y < displayHeight * 1 / 4 && cameraLocationY > 0)
                {
                    if (landArray[cameraLocationX + Player.player.tileX, CheckMin(cameraLocationY + Player.player.tileY - 1, 0)].depth[0] == 0)
                    {
                        cameraLocationY = cameraLocationY - 1;
                        Player.player.Y--;
                    }
                    Player.player.Rotation = 4 * ((float)Math.PI / 2.0f);
                    Player.player.LastMove = 4;
                }
            }
        }

        void HandlerClick()
        {
            if (invOpen == true)
            {

                invOpen = false;
            }
            else if (buildMenuOpen == true)
            {
                if (buildRect1.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 200)
                {
                    //Player.player.resources[5] = Player.player.resources[5] - 200;
                    Mine(201);
                }
                else if (buildRect2.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50)
                {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(101);
                }
                else if (buildRect3.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50)
                {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(102);
                }
                else if (buildRect4.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50)
                {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(103);
                }
                else if (buildRect5.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50)
                {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(104);
                }
                else if (buildRect6.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50)
                {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(105);
                }
                else if (buildRect7.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[5] >= 50)
                {
                    Player.player.resources[5] = Player.player.resources[5] - 50;
                    Mine(106);
                }
                else if (buildRect8.Contains(newMouseState.X, newMouseState.Y) && Player.player.resources[6] >= 0)
                {
                    //Player.player.resources[6] = Player.player.resources[6] - 200;
                    Mine(202);
                }
                else
                {
                    cantBuild.Start();
                }
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
                if (Units.Active.Count < Player.Units.Count)
                {
                    Units.Active.Add(Player.Units[Units.Active.Count]);
                    Units.Active[Units.Active.Count - 1].X = Player.player.X + MovementXY[Player.player.LastMove, 0];
                    Units.Active[Units.Active.Count - 1].Y = Player.player.Y + MovementXY[Player.player.LastMove, 1];
                    landArray[Units.Active[Units.Active.Count - 1].X, Units.Active[Units.Active.Count - 1].Y].IsOccupied = true;
                }
                workerListOpen = false;
            }
            if (invOpen == false && buildMenuOpen == false)
            {
                int spellX = (newMouseState.X / (int)(50 * tileScale) + cameraLocationX) - 2;
                int spellY = (newMouseState.Y / (int)(50 * tileScale) + cameraLocationY) - 2;
                TileManipulator(spellX, spellY, 5, 5, 5, false);
            }
        }

        void HandlerKeys()
        {
            if (newState.IsKeyDown(Keys.LeftShift))
            {
                HandlerKeyShift();
            }
            else if (newState.IsKeyDown(Keys.LeftControl))
            {
                HandlerKeyControl();
            }
            else if (oldState.IsKeyUp(Keys.Space) && newState.IsKeyDown(Keys.Space))
            {
                if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].depth[0] == 202)
                {
                    Player.player.depth++;
                }
                else
                {
                    Player.player.depth = 0;
                    actionPending = true;
                    actionTimer.Start();
                }
            }
            else if (oldState.IsKeyUp(Keys.I) && newState.IsKeyDown(Keys.I))
            {
                if (invOpen == true)
                {
                    invOpen = false;
                }
                else
                {
                    invOpen = true;
                }
            }
            else if (oldState.IsKeyUp(Keys.M) && newState.IsKeyDown(Keys.M))
            {
                if (tileScale != .5)
                {
                    tileScale = .5;
                }
                else
                {
                    tileScale = 2;
                }
                //ScaleTileMap();
            }
            else if (oldState.IsKeyUp(Keys.B) && newState.IsKeyDown(Keys.B))
            {
                if (buildMenuOpen == true)
                {
                    buildMenuOpen = false;
                }
                else
                {
                    buildMenuOpen = true;
                }
                //Mine(7);
            }
            else if (oldState.IsKeyUp(Keys.E) && newState.IsKeyDown(Keys.E))
            {
                if (workerListOpen == true)
                {
                    workerListOpen = false;
                }
                else
                {
                    workerListOpen = true;
                }
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
                        if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].depth[Player.player.depth] == 0)
                        {
                            Movement(Player.player);
                        }
                    }
                }
            }
        }

        void HandlerKeyShift()
        {
            if (newState.IsKeyDown(Keys.L))
            {
                DataLoad(true);
            }
        }

        void HandlerKeyControl()
        {
            if (newState.IsKeyDown(Keys.S))
            {
                DataSave();
            }
            else if (newState.IsKeyDown(Keys.L))
            {
                DataLoad(false);
            }
            else if (oldState.IsKeyUp(Keys.A) && newState.IsKeyDown(Keys.A))
            {
                if (playerAuto == false)
                {
                    playerAuto = true;
                }
                else
                {
                    Player.player.AutoX = 0;
                    Player.player.AutoY = 0;
                    landArray[Player.player.X, Player.player.Y].IsOccupied = false;
                    landArray[Player.player.X + MovementXY[Player.player.LastMove, 0], Player.player.Y + MovementXY[Player.player.LastMove, 1]].IsActive = false;
                    playerAuto = false;
                }
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
                    informationToWriteMod[counter + x] = landArray[x, y].depth[0].ToString();
                    if (counter == 0)
                    {
                        informationToWritePlayerResources[x] = Player.player.resources[x].ToString();
                        if (x < 200) { informationToWritePlayerStats[x] = Player.player.stats[x].ToString(); }
                    }
                }
                counter = counter + 1000;
            }

            //Player.player.Workers.CopyTo(informationToWritePlayerWorkers);

            counter = 0;
            for (int y = 0; y < Player.Units.Count; y++)
            {
                array = Player.Units[y].stats;
                for (int x = 0; x < 200; x++)
                {
                    informationToWritePlayerWorkers[counter + x] = array[x].ToString();
                }
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
                UdpState state = new UdpState();
                state.Endpoint = Endpoint;
                state.Client = Client;
                string Username = "King Charles I";

                try
                {
                    Client.Connect(Endpoint);
                }
                catch
                {
                    Client.Connect(Endpoint);
                }

                Client.Send(Encoding.Default.GetBytes(Username), Encoding.Default.GetBytes(Username).Count());
                var receivedData = Client.Receive(ref Endpoint);
                Console.Write($"Connection Established! {Endpoint}\n");

                ReceiveData();

                while (!messageReceived)
                {
                    Thread.Sleep(500);
                }

                messageReceived = false;
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
                    landArray[x, y].depth[0] = Int32.Parse(informationToWriteMod[counter + x]);
                    landArray[x, y].IsActive = false;
                    if (landArray[x, y].depth[0] == 4) { landArray[x, y].rotate = 0; }
                    else if (landArray[x, y].depth[0] == 5) { landArray[x, y].frame = 5; }
                    else { landArray[x, y].rotate = null; }
                    if (counter == 0)
                    {
                        Player.player.resources[x] = Int32.Parse(informationToWritePlayerResources[x]);
                        if (x < 200) { Player.player.stats[x] = Int32.Parse(informationToWritePlayerStats[x]); }
                    }
                }
                counter = counter + 1000;
            }
            counter = 0;
            Player.Units.Clear();
            Units.Active.Clear();
            informationToWritePlayerWorkers = File.ReadAllLines("C:/Users/2/Desktop/test1workers.txt");
            for (int y = 0; y < informationToWritePlayerWorkers.Length / 200; y++)
            {
                for (int x = 0; x < 200; x++)
                {
                    array[x] = Int32.Parse(informationToWritePlayerWorkers[counter + x]);
                }
                Player.Units.Add(new Units(0, 0, Player.Units.Count, array));
                counter = counter + 200;
            }
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            DrawTiles(cameraLocationX, cameraLocationY);
            DrawingBoard.DrawObjects(player, Player.player, tileScale, Player.player.Rotation, new Rectangle(0, 0, 50, 50));
            if (invOpen == true)
            {
                DrawingBoard.DrawObjects(inventory, Object.objects[901], 5, 0, new Rectangle(0, 0, 122, 174));
                if (newMouseState.RightButton == ButtonState.Pressed)
                {
                    DrawingBoard.DrawObjects(idCardBack, Object.objects[904], 1, 0, new Rectangle(0, 0, 1200, 732));
                    spriteBatch.DrawString(font, $"Gathering: {Player.player.stats[11]} ", new Vector2(370, 450), Color.Black);
                    spriteBatch.DrawString(font, $"Wood Cutting: {Player.player.stats[12]} ", new Vector2(370, 500), Color.Black);
                    spriteBatch.DrawString(font, $"Mining: {Player.player.stats[14]}", new Vector2(370, 550), Color.Black);
                    spriteBatch.DrawString(font, $"Construction: {Player.player.stats[17] }", new Vector2(370, 600), Color.Black);
                    spriteBatch.DrawString(font, $"Hunting: {Player.player.stats[18]}", new Vector2(370, 650), Color.Black);
                    spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[11] / 50}", new Vector2(750, 450), Color.Gold);
                    spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[12] / 500}", new Vector2(750, 500), Color.Gold);
                    spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[14] / 50}", new Vector2(750, 550), Color.Gold);
                    spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[17] / 20}", new Vector2(750, 600), Color.Gold);
                    spriteBatch.DrawString(font, $"LVL  {(int)Player.player.stats[18] / 10}", new Vector2(750, 650), Color.Gold);
                }
                else
                {
                    DrawingBoard.DrawObjects(idCard, Object.objects[903], 1, 0, new Rectangle(0, 0, 1200, 732));
                    spriteBatch.DrawString(font, $"Experience: {Player.player.stats[0]}", new Vector2(50, 450), Color.DarkViolet);
                    spriteBatch.DrawString(font, $"Workers: {Player.Units.Count} / {Player.player.resources[10]}", new Vector2(50, 500), Color.Blue);
                    spriteBatch.DrawString(font, $"Gold: {Player.player.gold}", new Vector2(50, 550), Color.DarkGoldenrod);
                    spriteBatch.DrawString(font, $"Water: {Player.player.resources[2]}", new Vector2(50, 600), Color.DarkBlue);
                    spriteBatch.DrawString(font, $"Vines: {Player.player.resources[3]}", new Vector2(50, 650), Color.ForestGreen);
                    spriteBatch.DrawString(font, $"Meat: {Player.player.resources[4]}", new Vector2(50, 700), Color.Crimson);
                    spriteBatch.DrawString(font, $"Lumber: {Player.player.resources[5]}", new Vector2(50, 750), Color.SaddleBrown);
                    spriteBatch.DrawString(font, $"Vitality: {Player.player.stats[1]}", new Vector2(370, 350), Color.Black);
                    spriteBatch.DrawString(font, $"Physique: {Player.player.stats[2]}", new Vector2(370, 400), Color.Black);
                    spriteBatch.DrawString(font, $"Agility: {Player.player.stats[3]}", new Vector2(370, 450), Color.Black);
                    spriteBatch.DrawString(font, $"Combat: {Player.player.stats[4]}", new Vector2(370, 500), Color.Black);
                    spriteBatch.DrawString(font, $"Magic: {Player.player.stats[5]}", new Vector2(370, 550), Color.Black);
                    spriteBatch.DrawString(font, $"Knowledge: {Player.player.stats[6]}", new Vector2(370, 600), Color.Black);
                    spriteBatch.DrawString(font, $"Leadership: {Player.player.stats[7]}", new Vector2(370, 650), Color.Black);
                    spriteBatch.DrawString(font, $"Sociability: {Player.player.stats[8]}", new Vector2(370, 700), Color.Black);
                    spriteBatch.DrawString(font, $"Expertise: {Player.player.stats[9]}", new Vector2(980, 220), Color.Gold);
                }
            }
            if (buildMenuOpen == true)
            {
                Build.Run();
                DrawingBoard.DrawObjects(buildMenu, Object.objects[902], 1, 0, new Rectangle(0, 0, 846, 535));
                DrawingBoard.DrawObjects(house_kame, Object.objects[201], 1, 0, new Rectangle(0, 0, 100, 150));
                DrawingBoard.DrawObjects(WallWoodHorizontal, Object.objects[101], 1, 0, new Rectangle(0, 0, 50, 100));
                DrawingBoard.DrawObjects(WallWoodVertical, Object.objects[102], 1, 0, new Rectangle(0, 0, 50, 100));
                DrawingBoard.DrawObjects(WallWoodCornerLeft, Object.objects[103], 1, 0, new Rectangle(0, 0, 50, 100));
                DrawingBoard.DrawObjects(WallWoodCornerRight, Object.objects[104], 1, 0, new Rectangle(0, 0, 50, 100));
                DrawingBoard.DrawObjects(WallWoodBackLeft, Object.objects[105], 1, 0, new Rectangle(0, 0, 50, 100));
                DrawingBoard.DrawObjects(WallWoodBackRight, Object.objects[106], 1, 0, new Rectangle(0, 0, 50, 100));
                DrawingBoard.DrawObjects(mine, Object.objects[202], 1, 0, new Rectangle(0, 0, 100, 100));
            }
            if (workerListOpen == true)
            {
                int[] array = new int[10];
                DrawingBoard.DrawObjects(workerList, Object.objects[905], 1, 0, new Rectangle(0, 0, 510, 825));
                for (int i = 0; i < Player.Units.Count; i++)
                {
                    array = Player.Units[i].stats;
                    spriteBatch.Draw(player, new Vector2(80, 80 + i * 50), Color.White);
                    spriteBatch.DrawString(font, $": {array[0]} | {array[1]} | {array[2]} | {array[3]} | {array[4]} | {array[5]} | " +
                        $"{array[6]} | {array[7]} | {array[8]} | {array[9]}", new Vector2(135, 80 + i * 50), Color.Black);
                }
            }
            DrawText();
            spriteBatch.End();
            // DrawText(displayHeight.ToString(), 0, 0);
            // DrawText(displayWidth.ToString(), 250, 250);

            base.Draw(gameTime);
        }
        
        public void DrawText(/*string text, int width, int height*/)
        {
            if (actionPending == true)
            {
                spriteBatch.DrawString(font, $"{actionTimer.ElapsedMilliseconds/1000}", new Vector2(1000, 500), Color.Red);
            }
            if (cantBuild.IsRunning == true)
            {
                string output = "Not Enough Resources!";
                Vector2 FontOrigin = font.MeasureString(output) / 2;
                spriteBatch.DrawString(font, output, new Vector2(1000, 450), Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            }
            spriteBatch.DrawString(font, $"{ Player.Units.Count }", new Vector2(50, 50), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ Player.player.X }", new Vector2(50, 150), Color.DarkViolet);
            spriteBatch.DrawString(font, $"{ Player.player.Y }", new Vector2(150, 150), Color.DarkViolet);
            if (Units.Active.Count > 0)
            {
                int[] array = Units.Active[0].stats;
                spriteBatch.DrawString(font, $"{ array[0] }", new Vector2(50, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ array[1] }", new Vector2(100, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ array[2] }", new Vector2(150, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ array[3] }", new Vector2(200, 100), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Units.Active[0].X }", new Vector2(50, 200), Color.DarkViolet);
                spriteBatch.DrawString(font, $"{ Units.Active[0].Y }", new Vector2(150, 200), Color.DarkViolet);
            }
        }

        /* DrawTiles;
         * Input Values: cameraLocationX, cameraLocation Y;
         * This function will run a number of times that is less than
         * the display height divided by default tile rendering dimension 50
         * divided by the scale of tile drawings to get number of tiles on screen
         * then add overflow number of 2 divided by tile scale to draw past screen broders (+2)
         * Steps: Determine global tile at current position camera x & y plus
         * the point at which in this function (int x or y) it has already drawn to
         * then check for biome before using tile scale and location data to draw tile
         */

        public void DrawTiles(int tempTilePointerX, int tempTilePointerY)
        {
            for (int y = 0; y < ((displayHeight / 50) / tileScale) + 2 / tileScale; y++)
            {
                for (int x = 0; x < ((displayWidth / 50) / tileScale) + 2 / tileScale; x++)
                {
                    if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 2)// && tileArray[x, y].land != 2)
                    {
                        spriteBatch.Draw(water, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                    }
                    else if (Player.player.depth > 0)
                    {
                        spriteBatch.Draw(white, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.Black);
                    }
                    else if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 1)
                    {
                        spriteBatch.Draw(land, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                    }
                    else if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 2)
                    {
                        spriteBatch.Draw(snow, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                    }
                    else if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 3)
                    {
                        spriteBatch.Draw(nodeStone, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(player, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                    }
                    //tileArray[x, y].biome = landArray[tempTilePointerX + x, tempTilePointerY + y].biome;
                    //tileArray[x, y].land = landArray[tempTilePointerX + x, tempTilePointerY + y].land;
                    //tileArray[x, y].mod = landArray[tempTilePointerX + x, tempTilePointerY + y].mod;
                }
            }

            for (int y = 0; y < ((displayHeight / 50) / tileScale) + 2/tileScale; y++)
            {
                for (int x = 0; x < ((displayWidth / 50) / tileScale) + 2/tileScale; x++)
                {
                    month = rnd.Next(0, 10000);
                    if (month == 5 || month == 9995 && landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 5)
                    {
                        if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame < 5 && month > 5000)
                        {
                            landArray[tempTilePointerX + x, tempTilePointerY + y].frame = CheckMax(landArray[tempTilePointerX + x, tempTilePointerY + y].frame+1, 5);
                            landArray[tempTilePointerX + x + 1, tempTilePointerY + y].frame = CheckMax(landArray[tempTilePointerX + x + 1, tempTilePointerY + y].frame+1, 5);
                            landArray[tempTilePointerX + x, tempTilePointerY + y + 1].frame = CheckMax(landArray[tempTilePointerX + x, tempTilePointerY + y + 1].frame+1, 5);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame > 0 && month < 5000)
                        {
                            int rand = rnd.Next(3, 6);
                            for (int i = 1; i < rand; i++)
                            {
                                landArray[tempTilePointerX + x, tempTilePointerY + y].frame = CheckMin(landArray[tempTilePointerX + x, tempTilePointerY + y].frame - 1, 0);
                                landArray[tempTilePointerX + x + i, tempTilePointerY + y].frame = CheckMin(landArray[tempTilePointerX + x + i, tempTilePointerY + y].frame - 1, 0);
                                landArray[tempTilePointerX + x, tempTilePointerY + y + i].frame = CheckMin(landArray[tempTilePointerX + x, tempTilePointerY + y + i].frame - 1, 0);
                                landArray[tempTilePointerX + x + i, tempTilePointerY + y + i].frame = CheckMin(landArray[tempTilePointerX + x + i, tempTilePointerY + y + i].frame - 1, 0);
                            }
                        }
                    }
                    if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 1)
                    {
                        if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 3)// && tileArray[x, y].land != 3)
                        {
                            spriteBatch.Draw(bush, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 4)// && tileArray[x, y].land != 4)
                        {
                            Vector2 location = new Vector2(x * (int)(50 * tileScale) + (int)(25 * tileScale) + 1, y * (int)(50 * tileScale) + (int)(25 * tileScale) + 1);
                            Rectangle sourceRectangle = new Rectangle(0, 0, 50, 50);
                            Vector2 origin = new Vector2(25, 25);
                            CreatureAI(Random(rnd.Next(0, 1000)), tempTilePointerX + x, tempTilePointerY + y);
                            spriteBatch.Draw(deer, location, sourceRectangle, Color.White, angle, origin, 1.0f * (float)tileScale, SpriteEffects.None, 1);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 5)// && tileArray[x, y].land != 5)
                        {
                            if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 5)
                            {
                                spriteBatch.Draw(tree5, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 4)
                            {
                                spriteBatch.Draw(tree4, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 3)
                            {
                                spriteBatch.Draw(tree3, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 2)
                            {
                                spriteBatch.Draw(tree2, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 1)
                            {
                                spriteBatch.Draw(tree1, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                            else if (landArray[tempTilePointerX + x, tempTilePointerY + y].frame == 0)
                            {
                                spriteBatch.Draw(tree0, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                            }
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 6)// && tileArray[x, y].land != 5)
                        {
                            spriteBatch.Draw(nodeStone, new Rectangle((x * (int)(50 * tileScale)), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 101)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodHorizontal, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 102)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodVertical, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 103)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodCornerLeft, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 104)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodCornerRight, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 105)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodBackLeft, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 106)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodBackRight, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 201)// && tileArray[x, y].land != 7)
                        {
                            spriteBatch.Draw(house_kame, new Rectangle((x - 1) * (int)(50 * tileScale), (y - 2) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(150 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 202)// && tileArray[x, y].land != 7)
                        {
                            spriteBatch.Draw(mine, new Rectangle((x - 1) * (int)(50 * tileScale), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                    }
                    else if (landArray[tempTilePointerX + x, tempTilePointerY + y].biome == 2)
                    {
                        if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 3)// && tileArray[x, y].land != 3)
                        {
                            spriteBatch.Draw(snowBush, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 4)// && tileArray[x, y].land != 4)
                        {
                            Vector2 location = new Vector2(x * (int)(50 * tileScale) + (int)(25 * tileScale) + 1, y * (int)(50 * tileScale) + (int)(25 * tileScale) + 1);
                            Rectangle sourceRectangle = new Rectangle(0, 0, 50, 50);
                            Vector2 origin = new Vector2(25, 25);
                            CreatureAI(Random(rnd.Next(0, 1000)), tempTilePointerX + x, tempTilePointerY + y);
                            spriteBatch.Draw(snowDeer, location, sourceRectangle, Color.White, angle, origin, 1.0f * (float)tileScale, SpriteEffects.None, 1);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 5)// && tileArray[x, y].land != 5)
                        {
                            spriteBatch.Draw(snowTree, new Rectangle((x * (int)(50 * tileScale) - (int)(50 / 2 * tileScale)), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[Player.player.depth] == 6)// && tileArray[x, y].land != 5)
                        {
                            spriteBatch.Draw(nodeStone, new Rectangle((x * (int)(50 * tileScale)), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 101)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodHorizontal, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 102)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodVertical, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 103)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodCornerLeft, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 104)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodCornerRight, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 105)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodBackLeft, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 106)// && tileArray[x, y].land != 6)
                        {
                            spriteBatch.Draw(WallWoodBackRight, new Rectangle(((x * (int)(50 * tileScale))), (y - 1) * (int)(50 * tileScale), (int)(50 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 201)// && tileArray[x, y].land != 7)
                        {
                            spriteBatch.Draw(house_kame, new Rectangle((x - 1) * (int)(50 * tileScale), (y - 2) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(150 * tileScale)), Color.White);
                        }
                        else if (landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0] == 202)// && tileArray[x, y].land != 7)
                        {
                            spriteBatch.Draw(mine, new Rectangle((x - 1) * (int)(50 * tileScale), (y - 1) * (int)(50 * tileScale), (int)(100 * tileScale), (int)(100 * tileScale)), Color.White);
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(player, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                    }
                    if (landArray[tempTilePointerX + x, tempTilePointerY + y].IsOccupied == true)
                    {
                        spriteBatch.Draw(player, new Rectangle(x * (int)(50 * tileScale), y * (int)(50 * tileScale), (int)(50 * tileScale), (int)(50 * tileScale)), Color.White);
                    }
                    //tileArray[x, y].biome = landArray[tempTilePointerX + x, tempTilePointerY + y].biome;
                    //tileArray[x, y].land = landArray[tempTilePointerX + x, tempTilePointerY + y].land;
                    //tileArray[x, y].depth[0] = landArray[tempTilePointerX + x, tempTilePointerY + y].depth[0];
                }
            }
        }

            /* GenerateBiome;
             * Input Values: cameraLocationX, cameraLocationY, Land[].biome;
             * Generates random biome scale depending on biome ID
             * while making sure to stay within default array bounds
             * by checking for array scale value which is this max i & ii in the function
             * it will run a number of times i while less than the scale of biome
             * and set tile biome values to the passed biome ID b until to scale
             */ 

        public void GenerateBiome(int tempCameraX, int tempCameraY, int d)
        {
            if (d == 2)
            {
                month = rnd.Next(5, 17);
            }
            if ((tempCameraX > month && tempCameraX < 1000 - month) && (tempCameraY > month && tempCameraY < 1000 - month))
            {
                for (int i = 0; i < month; i++)
                {
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraX++;
                        landArray[tempCameraX, tempCameraY].biome = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraY++;
                        landArray[tempCameraX, tempCameraY].biome = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraX--;
                        landArray[tempCameraX, tempCameraY].biome = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraY--;
                        landArray[tempCameraX, tempCameraY].biome = d;
                    }
                    tempCameraX--;
                    tempCameraY--;
                }
            }
        }
        public void GenerateMod(int tempCameraX, int tempCameraY, int d)
        {
            if (d == 6)
            {
                month = rnd.Next(1, 5);
            }
            if ((tempCameraX > month && tempCameraX < 1000 - month) && (tempCameraY > month && tempCameraY < 1000 - month))
            {
                for (int i = 0; i < month; i++)
                {
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraX++;
                        landArray[tempCameraX, tempCameraY].depth[0] = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraY++;
                        landArray[tempCameraX, tempCameraY].depth[0] = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraX--;
                        landArray[tempCameraX, tempCameraY].depth[0] = d;
                    }
                    for (int ii = 0; ii < i * 2; ii++)
                    {
                        tempCameraY--;
                        landArray[tempCameraX, tempCameraY].depth[0] = d;
                    }
                    tempCameraX--;
                    tempCameraY--;
                }
            }
        }

        public void ExpandBiome(int d)
        {

        }

        public void Mine(int a)
        {
            if (landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].depth[0] != 1)
            {
                if (a > 0)
                {
                    actionPending = true;
                    actionValue = -a;
                    actionTimer.Start();
                }
                else if (a < 0)
                {
                    if (Math.Abs(a) == 201 || Math.Abs(a) == 202)
                    {
                        Player.player.resources[10] = Player.player.resources[10] + 4;
                    }
                    if (MovementXY[Player.player.LastMove, 0] > 0 || MovementXY[Player.player.LastMove, 1] > 0)
                    {
                        TileManipulator(cameraLocationX + Player.player.tileX + (Object.objects[Math.Abs(a)].Width * MovementXY[Player.player.LastMove, 0]), cameraLocationY + Player.player.tileY + (Object.objects[Math.Abs(a)].Height * MovementXY[Player.player.LastMove, 1]), Object.objects[Math.Abs(a)].Width, Object.objects[Math.Abs(a)].Height, 1, true);
                        landArray[cameraLocationX + Player.player.tileX + (Object.objects[Math.Abs(a)].Width * MovementXY[Player.player.LastMove, 0]), cameraLocationY + Player.player.tileY + (Object.objects[Math.Abs(a)].Height * MovementXY[Player.player.LastMove, 1])].depth[0] = Math.Abs(a);
                    }
                    else
                    {
                        TileManipulator(cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1], Object.objects[Math.Abs(a)].Width, Object.objects[Math.Abs(a)].Height, 1, true);
                        landArray[cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1]].depth[0] = Math.Abs(a);
                    }
                }
                else // if (a == 0)
                {
                    Gather(cameraLocationX + Player.player.tileX + MovementXY[Player.player.LastMove, 0], cameraLocationY + Player.player.tileY + MovementXY[Player.player.LastMove, 1], Player.player);
                }
            }
        }

        public void AutoAI(Units unit)
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
                        if (landArray[unit.X, unit.Y + 1].depth[0] < 3
                            || landArray[unit.X, unit.Y + 1].depth[0] > 99)
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
                        if (landArray[unit.X, unit.Y - 1].depth[0] < 3
                            || landArray[unit.X, unit.Y - 1].depth[0] > 99)
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
                        if (landArray[unit.X + 1, unit.Y].depth[0] < 3
                            || landArray[unit.X + 1, unit.Y].depth[0] > 99)
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
                        if (landArray[unit.X - 1, unit.Y].depth[0] < 3
                            || landArray[unit.X - 1, unit.Y].depth[0] > 99)
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
                        if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].depth[0] > 2 && landArray[x, y].depth[0] < 100 && landArray[x, y].IsActive == false)
                        {
                            unit.AutoX = x;
                            unit.AutoY = y;
                            return;
                        }
                        for (int c = 0; c < (a * 2); c++)
                        {
                            x++;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].depth[0] > 2 && landArray[x, y].depth[0] < 100 && landArray[x, y].IsActive == false)
                            {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return;
                            }
                        }
                        for (int c = 0; c < (a * 2); c++)
                        {
                            y++;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].depth[0] > 2 && landArray[x, y].depth[0] < 100 && landArray[x, y].IsActive == false)
                            {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return;
                            }
                        }
                        for (int c = 0; c < (a * 2); c++)
                        {
                            x--;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].depth[0] > 2 && landArray[x, y].depth[0] < 100 && landArray[x, y].IsActive == false)
                            {
                                unit.AutoX = x;
                                unit.AutoY = y;
                                return;
                            }
                        }
                        for (int c = 0; c < (a * 2); c++)
                        {
                            y--;
                            if (x > 0 && x < 1000 && y > 0 && y < 1000 && landArray[x, y].depth[0] > 2 && landArray[x, y].depth[0] < 100 && landArray[x, y].IsActive == false)
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

        public void Gather(int a, int b, Units unit)
        {
            if (landArray[a, b].depth[0] != 0/* && landArray[a, b].mod != 1*/)
            {
                unit.stats[0]++;
                Player.player.resources[landArray[a, b].depth[0]]++;
                if (landArray[a, b].depth[0] == 2 || landArray[a, b].depth[0] == 3)
                {
                    unit.stats[11]++;
                }
                else if (landArray[a, b].depth[0] == 4)
                {
                    unit.stats[18]++;
                    unit.stats[19]++;
                    unit.stats[20]++;
                    unit.stats[21]++;
                }
                else if (landArray[a, b].depth[0] == 5)
                {
                    unit.stats[12]++;
                }
                else if (landArray[a, b].depth[0] == 6)
                {
                    unit.stats[14]++;
                }
                else if (landArray[a, b].depth[0] > 6)
                {
                    unit.stats[17]++;
                    if (landArray[a, b].depth[0] == 202)
                    {
                        unit.depth = 1;
                    }
                }
                landArray[a, b].depth[0] = 0;
            }
        }

        public void TileManipulator(int x, int y, int width, int height, int mod, bool inversion)
        {
            if (inversion == true)
            {
                for (int a = 0; a < height; a++)
                {
                    for (int b = 0; b < width; b++)
                    {
                        if (landArray[x - a, y - b].depth[0] != mod)
                        {
                            if (landArray[x - a, y - b].depth[0] != 0/* && landArray[x + a, y + b].mod != 1 */)
                            {
                                Gather(x - a, y - b, Player.player);
                            }
                            landArray[x - a, y - b].depth[0] = mod;
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
                        if (landArray[x + a, y + b].depth[0] != mod)
                        {
                            if (landArray[x + a, y + b].depth[0] != 0/* && landArray[x + a, y + b].mod != 1 */)
                            {
                                Gather(x + a, y + b, Player.player);
                            }
                            landArray[x + a, y + b].depth[0] = mod;
                        }
                    }
                }
            }
        }

        /*
         * temporary deer rotation
         */

        public void CreatureAI(float i, int x, int y) // Deer Rotation i 0,1000
        {
            angle = (float)landArray[x, y].rotate;

            if (i < 5)
            {
                landArray[x, y].rotate = (float?)Math.PI / 2.0f * random;
            }
            else if (i < 20 && i >= 5)
            {
                if (landArray[x, y].rotate == (float?)4 * Math.PI / 2.0f && landArray[x, CheckMax(y + 1, 1000)].depth[0] == 0)
                {
                    landArray[x, y + 1].rotate = landArray[x, y].rotate;
                    landArray[x, y + 1].depth[0] = 4;// landArray[x, y].mod;
                    landArray[x, y].rotate = null;
                    landArray[x, y].depth[0] = 0;
                }
                else if (landArray[x, y].rotate == (float?)3 * Math.PI / 2.0f && landArray[CheckMax(x + 1, 1000), y].depth[0] == 0)
                {
                    landArray[x + 1, y].rotate = landArray[x, y].rotate;
                    landArray[x + 1, y].depth[0] = 4;// landArray[x, y].mod;
                    landArray[x, y].rotate = null;
                    landArray[x, y].depth[0] = 0;
                }
                else if (landArray[x, y].rotate == (float?)2 * Math.PI / 2.0f && landArray[x, CheckMin(y - 1, 0)].depth[0] == 0)
                {
                    landArray[x, y - 1].rotate = landArray[x, y].rotate;
                    landArray[x, y - 1].depth[0] = 4;// landArray[x, y].mod;
                    landArray[x, y].rotate = null;
                    landArray[x, y].depth[0] = 0;
                }
                else if (landArray[x, y].rotate == (float?)Math.PI / 2.0f && landArray[CheckMin(x - 1, 0), y].depth[0] == 0)
                {
                    landArray[x - 1, y].rotate = landArray[x, y].rotate;
                    landArray[x - 1, y].depth[0] = 4;// landArray[x, y].mod;
                    landArray[x, y].rotate = null;
                    landArray[x, y].depth[0] = 0;
                }
            }
            else if (i < 25 && i >= 20)
            {

            }
        }

        public int[] GenerateWorker()
        {
            int[] array = new int[200];
            array[0] = 0;
            for (int i = 1; i < 10; i++)
            {
                month = rnd.Next(0, 4);
                array[i] = (int)month;
            }
            return(array);
        }

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

        // Check value against a minimum, if below return min //
        public int CheckMin(int value, int min)
        {
            if (value < min)
            {
                value = min;
            }
            return (value);
        }
        // Check value against a maximum, if above return max //
        public int CheckMax(int value, int max)
        {
            if (value > max)
            {
                value = max;
            }
            return (value);
        }
        // Could also be called "CheckRange" //
        public int CheckMinMax(int value, int min, int max)
        {
            value = CheckMin(value, min);
            value = CheckMax(value, max);
            return (value);
        }
        // Cached value random for compatibility or expanded features, 
        // currently seems unneccessary and appears to work when removed from current applications
        public float Random(float i)
        {
            random = i;
            return (i);
        }

        public void ConvertString(Land i)
        {

        }

        public static async Task ReceiveData()
        {
            for (int i = 0; i < 20; i++)
            {
                receivedBytes = await Task.Run(() => GrabPacket());
                for (int ii = 0; ii < 50000; ii++)
                {
                    informationToWriteBiome[(i * 50000) + ii] = receivedBytes[ii].ToString();
                }
                Client.Send(new byte[] { 1 }, 1);
            }

            for (int i = 0; i < 20; i++)
            {
                receivedBytes = await Task.Run(() => GrabPacket());
                for (int ii = 0; ii < 50000; ii++)
                {
                    informationToWriteMod[(i * 50000) + ii] = receivedBytes[ii].ToString();
                }
                Client.Send(new byte[] { 1 }, 1);
            }

            messageReceived = true;
        }

        public static byte[] GrabPacket()
        {
            byte[] b = new byte[50000];
            b = Client.Receive(ref Endpoint);
            return b;
        }
    }
}
