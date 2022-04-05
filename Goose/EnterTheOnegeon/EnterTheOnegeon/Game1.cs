using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/*
 * TO DO: 
 * > Add parry mechanic (bullet can be stationary for now)
 *      - I'm thinking if we want an animated walk cycle, we could cut the player 
 *      sprite in half.  Doing this will allow us to animate the legs walking 
 *      (bottom half) independently from the arms parrying (top half)
 *      - Or we could just make two walk animations, one while not parrying and 
 *      one while parrying
 * > Positioning and making the shop look better
 * > Upgrade for invincibility timer and contact damage
 * > Add iframes for enemies
 *
 * 
 * Not a TODO, but useful stuff
 * 3840, 2176 <-- dimensions of the dungeon
 * BulletStats can be added to another BulletStats and also there's a tostring for it
 * BulletStats constructor, size, spd, passes, dmg
 */


namespace EnterTheOnegeon
{
    // base fsm, other enums to be added include Help + Pause
    enum GameState
    {
        Title,
        Game,
        Score,
        Help,
        Pause
    }

    enum DebugMode
    {
        On, Off
    }

    /// <summary>
    /// Contains all code to run Enter the Onegeon
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _spriteBatch2;
        private GameState gameState = GameState.Title;

        // camera that follows sprite
        private Camera camera;

        // static variables can not be changed once declared
        public static int screenHeight = 1080;
        public static int screenWidth = 1920;

        // handles keyboard input
        KeyboardState _currentKbState;
        KeyboardState _prevKbState;

        //handles mouse input
        MouseState _mState;
        MouseState _prevMState;

        //Temp Game fields
        double totalGameTime;
        double tempTime;

        Random random = new Random();

        // player fields
        Texture2D playerAsset;
        Player player;

        // bullet fields
        Texture2D bulletAsset;
        BulletManager bulletManager;

        // enemy fields
        Texture2D enemyAsset;
        EnemyManager enemyManager;



        // text/font fields
        SpriteFont fipps;
        SpriteFont titleFont;

        //background fields
        Texture2D dungeon;
        Texture2D coverArt;
        Texture2D scoreBoard;
        Texture2D pause;

        // button fields
        Texture2D T_Button;
        Button strtButt;
        Button menuButt;
        Button quitButt;
        Button debugButt;
        Button helpButt;

        // debug mode fields
        DebugMode debug;
        Texture2D buttonOn;
        Texture2D buttonOff;

        // Text size for centering
        Vector2 textSize;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            totalGameTime = 0;
            base.Initialize();
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            debug = DebugMode.Off;
            _graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatch2 = new SpriteBatch(GraphicsDevice);

            camera = new Camera();

            _currentKbState = Keyboard.GetState();
            _prevKbState = _currentKbState;

            // initialize background texture
            coverArt = Content.Load<Texture2D>("coverArt");
            dungeon = Content.Load<Texture2D>("dungeon");
            scoreBoard = Content.Load<Texture2D>("scoreBoard");
            pause = Content.Load<Texture2D>("Pause");

            // initialize player and its asset
            playerAsset = Content.Load<Texture2D>("player");

            // for now, i put the location of the sprite near the bottom of the screen
            player = new Player(playerAsset, new Rectangle(400, 350, 32, 64));

            //Loading for bullets
            bulletAsset = Content.Load<Texture2D>("Bullet");
            bulletManager = new BulletManager(bulletAsset);

            // loading enemy and initializing a list
            enemyAsset = Content.Load<Texture2D>("Enemy");
            enemyManager = new EnemyManager(_graphics, enemyAsset, player);

            // load font
            fipps = Content.Load<SpriteFont>("fipps15");
            titleFont = Content.Load<SpriteFont>("titleFont");

            buttonOn = Content.Load<Texture2D>("buttonOn");
            buttonOff = Content.Load<Texture2D>("buttonOff");

            // load button texture and create all buttons
            T_Button = Content.Load<Texture2D>("T_Button");
            strtButt = new Button(
                fipps,
                T_Button,
                "Start",
                new Rectangle(30, screenHeight - 130, 150, 75),
                Color.Gold);
            quitButt = new Button(
                fipps,
                T_Button,
                "Quit",
                new Rectangle(screenWidth - 180, screenHeight - 130, 150, 75), Color.Gold);
            menuButt = new Button(
                fipps,
                T_Button,
                "Menu",
                new Rectangle(30, screenHeight - 130, 150, 75),
                Color.Gold);
            debugButt = new Button(
                fipps,
                buttonOff,
                "Debug",
                new Rectangle(screenWidth - 120, 50, 50, 50),
                Color.Gold);
            helpButt = new Button(
                fipps,
                T_Button,
                "Controls",
                new Rectangle((screenWidth / 2) - 75,screenHeight - 130, 150, 75),
                Color.Gold
                );
            // modify "Debug" text location
            debugButt.textPos.X = screenWidth - 250;
        }

        protected override void Update(GameTime gameTime)
        {

            _prevKbState = _currentKbState;
            _currentKbState = Keyboard.GetState();
            _prevMState = _mState;
            _mState = Mouse.GetState();

            switch (gameState)
            {
                #region Title State
                case GameState.Title:
                    //Reset all the lists and player whenever going to title for now
                    player = new Player(playerAsset, new Rectangle(1910, 1550, 32, 64));
                    bulletManager = new BulletManager(bulletAsset);
                    enemyManager = new EnemyManager(_graphics, enemyAsset, player);
                    totalGameTime = 0;
                    tempTime = 0;

                    // Start button clicked
                    if (_mState.X < strtButt.ButtRect.X + strtButt.ButtRect.Width &&
                        _mState.X > strtButt.ButtRect.X &&
                        _mState.Y < strtButt.ButtRect.Y + strtButt.ButtRect.Height &&
                        _mState.Y > strtButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        gameState = GameState.Game;
                    }
                    else if (_currentKbState.IsKeyDown(Keys.Enter))
                    {
                        gameState = GameState.Game;
                    }

                    // Quit clicked
                    else if (_mState.X < quitButt.ButtRect.X + quitButt.ButtRect.Width &&
                        _mState.X > quitButt.ButtRect.X &&
                        _mState.Y < quitButt.ButtRect.Y + quitButt.ButtRect.Height &&
                        _mState.Y > quitButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        Exit();
                    }

                    // Debug clicked
                    else if (_mState.X < debugButt.ButtRect.X + debugButt.ButtRect.Width &&
                        _mState.X > debugButt.ButtRect.X &&
                        _mState.Y < debugButt.ButtRect.Y + debugButt.ButtRect.Height &&
                        _mState.Y > debugButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed &&
                        debug == DebugMode.Off)
                    {
                        debug = DebugMode.On;
                    }

                    // Debug clicked again
                    else if (_mState.X < debugButt.ButtRect.X + debugButt.ButtRect.Width &&
                        _mState.X > debugButt.ButtRect.X &&
                        _mState.Y < debugButt.ButtRect.Y + debugButt.ButtRect.Height &&
                        _mState.Y > debugButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed &&
                        debug == DebugMode.On)
                    {
                        debug = DebugMode.Off;
                    }

                    // Help Clicked
                    else if (_mState.X < helpButt.ButtRect.X + helpButt.ButtRect.Width &&
                        _mState.X > helpButt.ButtRect.X &&
                        _mState.Y < helpButt.ButtRect.Y + helpButt.ButtRect.Height &&
                        _mState.Y > helpButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        gameState = GameState.Help;
                    }
                    break;
                #endregion
                #region Game State
                case GameState.Game:

                    // Scoreboard when player dies
                    if (!player.Active)
                    {
                        gameState = GameState.Score;
                    }

                    // players movement
                    player.Update(
                        gameTime,
                        _mState,
                        _prevMState,
                        _currentKbState,
                        _prevKbState);

                    // cameras movement
                    camera.Follow(player);

                    // enemy spawning and updating
                    enemyManager.Update(gameTime, player);
                    bulletManager.Update(gameTime, _mState, _prevMState, player, enemyManager);

                    //Adding to the timer
                    totalGameTime += gameTime.ElapsedGameTime.TotalSeconds;
                    tempTime += gameTime.ElapsedGameTime.TotalSeconds;

                    // if debug mode is on
                    if (debug == DebugMode.On)
                    {
                        // infinite bullets and health
                        player.BulletCount = 100;
                        player.Health = 4;
                    }

                    // if debug mode is off return stats to normal
                    if (debug == DebugMode.Off)
                    {
                        player.BulletCount = player.BulletCount;
                        player.Health = player.Health;
                    }
                    // enter the pause state if escape is pressed
                    if (_currentKbState.IsKeyUp(Keys.Escape) && _prevKbState.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameState.Pause;
                    }
                    break;
                #endregion
                #region Scoreboard State
                case GameState.Score:
                    textSize = titleFont.MeasureString(enemyManager.Score.ToString());

                    // quit button pressed
                    if (_mState.X < quitButt.ButtRect.X + quitButt.ButtRect.Width &&
                        _mState.X > quitButt.ButtRect.X &&
                        _mState.Y < quitButt.ButtRect.Y + quitButt.ButtRect.Height &&
                        _mState.Y > quitButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        Exit();
                    }

                    // menu button pressed
                    else if (_mState.X < menuButt.ButtRect.X + menuButt.ButtRect.Width &&
                        _mState.X > menuButt.ButtRect.X &&
                        _mState.Y < menuButt.ButtRect.Y + menuButt.ButtRect.Height &&
                        _mState.Y > menuButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        gameState = GameState.Title;
                    }
                    
                    break;
                #endregion
                #region Help State
                case GameState.Help:
                    // menu button pressed
                    if (_mState.X < menuButt.ButtRect.X + menuButt.ButtRect.Width &&
                        _mState.X > menuButt.ButtRect.X &&
                        _mState.Y < menuButt.ButtRect.Y + menuButt.ButtRect.Height &&
                        _mState.Y > menuButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        gameState = GameState.Title;
                    }

                    break;
                #endregion
                #region Pause State
                case GameState.Pause:
                    if (_currentKbState.IsKeyUp(Keys.Escape) && _prevKbState.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameState.Game;
                    }

                    // menu button pressed
                    else if (_mState.X < menuButt.ButtRect.X + menuButt.ButtRect.Width &&
                        _mState.X > menuButt.ButtRect.X &&
                        _mState.Y < menuButt.ButtRect.Y + menuButt.ButtRect.Height &&
                        _mState.Y > menuButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        gameState = GameState.Title;
                    }
                    // quit button pressed
                    if (_mState.X < quitButt.ButtRect.X + quitButt.ButtRect.Width &&
                        _mState.X > quitButt.ButtRect.X &&
                        _mState.Y < quitButt.ButtRect.Y + quitButt.ButtRect.Height &&
                        _mState.Y > quitButt.ButtRect.Y &&
                        _mState.LeftButton == ButtonState.Released &&
                        _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        Exit();
                    }
                    break;
                #endregion
            }

            #region Debug hotkeys
            if (Keyboard.GetState().IsKeyDown(Keys.D1) && debug == DebugMode.On)
            {
                gameState = GameState.Title;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2) && debug == DebugMode.On)
            {
                gameState = GameState.Game;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3) && debug == DebugMode.On)
            {
                gameState = GameState.Score;
            }
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(174, 135, 137));

            _spriteBatch.Begin(transformMatrix: camera.Transform);

            // for stationary images
            // title/score screen, buttons etc.
            _spriteBatch2.Begin();

            // switch to control what is being drawn to the screen at each part of our fsm
            switch (gameState)
            {

                #region Title State
                case GameState.Title:
                    _spriteBatch2.Draw(
                        coverArt,
                        new Rectangle(
                            0,
                            0,
                            1920,
                            1080),
                        Color.White);

                    strtButt.Draw(_spriteBatch2);
                    quitButt.Draw(_spriteBatch2);
                    debugButt.Draw(_spriteBatch2);
                    helpButt.Draw(_spriteBatch2);
                    break;
                #endregion
                #region Game State

                case GameState.Game:
                    // Game background
                    _spriteBatch.Draw(
                        dungeon,
                        new Rectangle(
                            0,
                            0,
                            3840,
                            2176),
                        Color.White);

                    
                    // Enemy manager draws all enemies, score and time
                    enemyManager.Draw(_spriteBatch, fipps);

                    // Player
                    player.Draw(_spriteBatch);
                    // Bullet UI
                    _spriteBatch.Draw(
                        bulletAsset,
                        new Rectangle(
                            100 - (int)camera.Transform.Translation.X,
                            70 - (int)camera.Transform.Translation.Y,
                            30,
                            30),
                        Color.White);

                    _spriteBatch.DrawString(
                        fipps,
                        String.Format("x{0}", player.BulletCount),
                        new Vector2(
                            140 - (int)camera.Transform.Translation.X,
                            65 - (int)camera.Transform.Translation.Y),
                        Color.White);


                    bulletManager.Draw(_spriteBatch);

                    //Drawing the line from player to cursor
                    for (int i = 0; i < 20; i++)
                    {
                        _spriteBatch.Draw(
                            dungeon,

                            new Rectangle(
                                player.CenterX + ((_mState.X - (int)camera.Transform.Translation.X - player.CenterX) * i / 20),
                                player.CenterY + ((_mState.Y - (int)camera.Transform.Translation.Y - player.CenterY) * i / 20),
                                4,
                                4),
                            Color.Black);
                    }
                    break;
                #endregion

                #region Scoreboard State
                case GameState.Score:
                    _spriteBatch2.Draw(
                        scoreBoard,
                        new Rectangle(
                           0, 0,
                            screenWidth,
                            screenHeight),
                        Color.White);

                    _spriteBatch2.DrawString(
                        titleFont,
                        enemyManager.Score.ToString(),
                        new Vector2(
                            screenWidth / 2 - textSize.X,
                            screenHeight / 2),
                        Color.White);

                    menuButt.Draw(_spriteBatch2);

                    quitButt.Draw(_spriteBatch2);
                    break;
                #endregion
                #region Help State
                case GameState.Help:
                    _spriteBatch2.Draw(
                        coverArt,
                        new Rectangle(
                            0,
                            0,
                            1920,
                            1080),
                        Color.White);
                    _spriteBatch2.Draw(
                        pause,
                        new Rectangle(
                            0,
                            0,
                            1920,
                            1080),
                            Color.White);
                    _spriteBatch2.DrawString(
                        titleFont,
                        "CONTROLS",
                        new Vector2(20, 20),
                        Color.Gold
                        );
                    _spriteBatch2.DrawString(
                        fipps,
                        "WASD- Movement (W = up, A = left, S = down, D = right)",
                        new Vector2(20, 200),
                        Color.White
                        );
                    _spriteBatch2.DrawString(
                        fipps,
                        "Left Click - Shoot",
                        new Vector2(20, 300),
                        Color.White
                        );
                    _spriteBatch2.DrawString(
                        fipps,
                        "Right Click - Parry (to be added)",
                        new Vector2(20, 400),
                        Color.White
                        );
                    _spriteBatch2.DrawString(
                        fipps,
                        "Escape - Pause/Play",
                        new Vector2(20, 500),
                        Color.White
                        );
                    menuButt.Draw(_spriteBatch2);

                    break;
                #endregion
                #region Pause State
                // draw the game with the pause sprite overlaid but none of the updates for game will
                // be going on, so nothing will move
                case GameState.Pause:
                    _spriteBatch.Draw(
                        dungeon,
                        new Rectangle(
                            0,
                            0,
                            3840,
                            2176),
                        Color.White);


                    // Enemy manager draws all enemies, score and time
                    enemyManager.Draw(_spriteBatch, fipps);

                    // Player
                    player.Draw(_spriteBatch);
                    // Bullet UI
                    _spriteBatch.Draw(
                        bulletAsset,
                        new Rectangle(
                            100 - (int)camera.Transform.Translation.X,
                            70 - (int)camera.Transform.Translation.Y,
                            30,
                            30),
                        Color.White);

                    _spriteBatch.DrawString(
                        fipps,
                        String.Format("x{0}", player.BulletCount),
                        new Vector2(
                            140 - (int)camera.Transform.Translation.X,
                            65 - (int)camera.Transform.Translation.Y),
                        Color.White);


                    bulletManager.Draw(_spriteBatch);
                    //Drawing pause overlay
                    _spriteBatch2.Draw(
                       pause,
                       new Rectangle(
                           0,
                           0,
                           1920,
                           1080),
                           Color.White);
                    _spriteBatch2.DrawString(
                       titleFont,
                       "Pause",
                       new Vector2(20, 20),
                       Color.Gold
                       );
                    _spriteBatch2.DrawString(
                       fipps,
                       "Press the Escape key to resume",
                       new Vector2(20, 135),
                       Color.White
                       );
                    _spriteBatch2.DrawString(
                       titleFont,
                       "Player Stats",
                       new Vector2(20, 200),
                       Color.Gold
                       );
                    _spriteBatch2.DrawString(
                       fipps,
                       "Damage: " + player.BStats.Damage.ToString(),
                       new Vector2(20, 350),
                       Color.White
                       );
                    _spriteBatch2.DrawString(
                       fipps,
                       "Bullet Speed: " + player.BStats.Speed.ToString(),
                       new Vector2(20, 400),
                       Color.White
                       );
                    _spriteBatch2.DrawString(
                       fipps,
                       "Bullet Size: " + player.BStats.Size.ToString(),
                       new Vector2(20, 450),
                       Color.White
                       );
                    _spriteBatch2.DrawString(
                       fipps,
                       "Penetration: " + player.BStats.Passes.ToString(),
                       new Vector2(20, 500),
                       Color.White
                       );
                    _spriteBatch2.DrawString(
                       fipps,
                       "Player Speed: " + player.Speed,
                       new Vector2(20, 550),
                       Color.White
                       );
                    _spriteBatch2.DrawString(
                       fipps,
                       "Player Health: " + player.Health + "/" + player.MaxHealth,
                       new Vector2(20, 600),
                       Color.White
                       );
                    menuButt.Draw(_spriteBatch2);
                    break;
                #endregion
            }

            switch (debug)
            {
                // light up button
                // display hotkeys for moving states
                case DebugMode.On:
                    if (gameState == GameState.Title)
                    {
                        debugButt.buttText = buttonOn;
                        #region Debug hotkey text
                        _spriteBatch2.DrawString(
                            fipps,
                            "Press 1 for menu",
                            new Vector2(100,
                                        200),
                            Color.White);

                        _spriteBatch2.DrawString(
                            fipps,
                            "Press 2 for game",
                            new Vector2(100,
                                        240),
                            Color.White);

                        _spriteBatch2.DrawString(
                            fipps,
                            "Press 3 for score",
                            new Vector2(100,
                                        280),
                            Color.White);

                        #endregion

                    }
                    if (gameState == GameState.Game)
                    {
                        // Player iframes
                        _spriteBatch.DrawString(
                            fipps,
                            String.Format("Iframe time: {0:F3}", player.IFrameTimeLeft),
                            new Vector2(
                                100 - (int)camera.Transform.Translation.X,
                                100 - (int)camera.Transform.Translation.Y),
                            Color.White);

                        #region Debug hotkey text
                        _spriteBatch.DrawString(
                            fipps,
                            "Press 1 for menu",
                            new Vector2(-(int)camera.Transform.Translation.X + 100,
                                        -(int)camera.Transform.Translation.Y + 200),
                            Color.White);

                        _spriteBatch.DrawString(
                            fipps,
                            "Press 2 for game",
                            new Vector2(-(int)camera.Transform.Translation.X + 100,
                                        -(int)camera.Transform.Translation.Y + 240),
                            Color.White);

                        _spriteBatch.DrawString(
                            fipps,
                            "Press 3 for score",
                            new Vector2(-(int)camera.Transform.Translation.X + 100,
                                        -(int)camera.Transform.Translation.Y + 280),
                            Color.White);

                        #endregion
                    }
                    if (gameState == GameState.Score)
                    {
                        #region Debug hotkey text
                        _spriteBatch2.DrawString(
                            fipps,
                            "Press 1 for menu",
                            new Vector2(100,
                                        200),
                            Color.White);

                        _spriteBatch2.DrawString(
                            fipps,
                            "Press 2 for game",
                            new Vector2(100,
                                        240),
                            Color.White);

                        _spriteBatch2.DrawString(
                            fipps,
                            "Press 3 for score",
                            new Vector2(100,
                                        280),
                            Color.White);

                        #endregion
                    }

                    break;
                case DebugMode.Off:
                    if (gameState == GameState.Title)
                    {
                        debugButt.buttText = buttonOff;
                    }
                    break;

            }

            _spriteBatch.End();
            _spriteBatch2.End();

            base.Draw(gameTime);
        }

        #region Randpoint unused 
        /*
        // Gets a random point off screen
        public Point RandPoint()
        {
            int randX;
            int randY;
            // 50/50 to decide to change x or y offscreen
            //enemy comes in from left or right
            if (random.Next(2) > 0)
            {
                randY = random.Next(0 - enemyAsset.Height, GraphicsDevice.Viewport.Height);
                randX = random.Next(2);
                // enemy spawns east of viewport
                if(randX == 1)
                {
                    randX = GraphicsDevice.Viewport.Width;
                }
                // enemy spawns west of viewport
                else
                {
                    // 50 is the height of the enemy object, the height of the object !=
                    // the height of the sprite.
                    randX = 0 - 50;
                }
            }
            //enemy comes in from top or bottom
            else
            {
                randX = random.Next(0 - enemyAsset.Width, GraphicsDevice.Viewport.Width);
                randY = random.Next(2);
                // enemy spawns south of viewport 
                if (randY == 1)
                {
                    randY = GraphicsDevice.Viewport.Height;
                }
                // enemy spawns north of viewport
                else
                {
                    // 50 is the height of the enemy object, the height of the object !=
                    // the height of the sprite.
                    randY = 0 - 50;
                }
            }
            return new Point(randX, randY);
        }
        */
        #endregion
    }
}