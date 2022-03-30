using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/*
 * TO DO: 
 * >IMPLEMENT FIPPS FONT (https://www.dafont.com/fipps.font)
 * > Add parry mechanic (bullet can be stationary for now)
 * > Bullet stats and manager class
 * > Collision detection for the stage
 * >
 */


namespace EnterTheOnegeon
{
    // base fsm, other enums to be added include Help + Pause
    enum GameState
    {
        Title,
        Game,
        Score
    }

    /// <summary>
    /// Contains all code to run Enter the Onegeon
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
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
        int score;

        Random random = new Random();
        
        // player fields
        Texture2D playerAsset;
        Player player;

        // bullet fields
        Texture2D bulletAsset;
        List<Bullet> bulletList;

        // enemy fields
        Texture2D enemyAsset;
        EnemyManager enemyManager;

        // text/font fields
        SpriteFont verdana;
        SpriteFont fipps;

        //background fields
        Texture2D dungeon;
        Texture2D coverArt;
        Texture2D scoreBoard;

        // button fields
        Texture2D T_Button;
        Button strtButt;
        Button menuButt;
        Button quitButt;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            totalGameTime = 0;
            score = 0;
            base.Initialize();
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new Camera();

            _currentKbState = Keyboard.GetState();
            _prevKbState = _currentKbState;

            // initialize background texture
            coverArt = Content.Load<Texture2D>("coverArt");
            dungeon = Content.Load<Texture2D>("dungeon");
            scoreBoard = Content.Load<Texture2D>("scoreBoard");

            // initialize player and its asset
            playerAsset = Content.Load<Texture2D>("player");

            // for now, i put the location of the sprite near the bottom of the screen
            player = new Player(playerAsset, new Rectangle(400, 350, 32, 64));

            //Loading for bullets
            bulletAsset = Content.Load<Texture2D>("Bullet");
            bulletList = new List<Bullet>();

            // loading enemy and initializing a list
            enemyAsset = Content.Load<Texture2D>("badguy");
            enemyManager = new EnemyManager(_graphics, enemyAsset);

            // load font
            verdana = Content.Load<SpriteFont>("Verdana15");
            fipps = Content.Load<SpriteFont>("fipps15");

            // load button texture and create all buttons
            T_Button = Content.Load<Texture2D>("T_Button");
            strtButt = new Button(
                fipps, 
                T_Button, 
                "Start", 
                new Rectangle(30, screenHeight - 90, 150, 75), 
                Color.Gold);
            quitButt = new Button(
                fipps, 
                T_Button, 
                "Quit", 
                new Rectangle(screenWidth - 180, screenHeight - 90, 150, 75), Color.Gold);
            menuButt = new Button(
                fipps, 
                T_Button, 
                "Menu", 
                new Rectangle(30, screenHeight - 90, 150, 75), 
                Color.Gold);

        }

        protected override void Update(GameTime gameTime)
        {

            _prevKbState = _currentKbState;
            _currentKbState = Keyboard.GetState();
            _mState = Mouse.GetState();

            switch (gameState)
            {
                #region Title State
                case GameState.Title:
                    //Reset all the lists and player whenever going to title for now
                    player = new Player(playerAsset, new Rectangle(1910, 1550, 32, 64));
                    bulletList = new List<Bullet>();
                    enemyManager = new EnemyManager(_graphics, enemyAsset);
                    totalGameTime = 0;
                    tempTime = 0;
                    score = 0;

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
                    break;
                #endregion
                #region Game State
                case GameState.Game:

                    // Scoreboard when player dies
                    if(!player.Active)
                    {
                        gameState = GameState.Score;
                    }


                    // players movement
                    player.Update(gameTime);

                    // cameras movement
                    camera.Follow(player);

                    // bullet spawning when mouse clicked
                    if (_mState.LeftButton == ButtonState.Pressed && _prevMState.LeftButton == ButtonState.Released && player.BulletCount > 0)
                    {
                        bulletList.Add(
                            new Bullet(
                                bulletAsset, 
                                new Rectangle(
                                    player.CenterX - bulletAsset.Width / 2, 
                                    player.CenterY - bulletAsset.Height / 2, 
                                    10, 
                                    10),
                                gameTime, 
                                new Vector2(
                                    _mState.X - camera.Transform.Translation.X, 
                                    _mState.Y - camera.Transform.Translation.Y), 
                                5));

                        player.BulletCount--;
                    }

                    // enemy spawning and updating
                    enemyManager.Update(gameTime, player);

                    //Bullets testing
                    foreach (Bullet b in bulletList)
                    {
                        b.Update(gameTime);
                        foreach(TestEnemy en in enemyManager.GetTestEnemies())
                        {
                            if(b.CollideWith(en))
                            {
                                b.HitEnemy(en);
                            }
                        }
                    }

                    // Remove bullets after they have killed set amount of enemies
                    // (see "passed" field in bullet.cs)
                    for (int i = bulletList.Count - 1; i >= 0; i--)
                    {
                        if (!bulletList[i].Active)
                            bulletList.RemoveAt(i);
                    }

                    /*// Remove enemies that are shot
                    for (int i = enemyList.Count - 1; i >= 0; i--)
                    {
                        if (!enemyList[i].Active)
                        {
                            enemyList.RemoveAt(i);
                            score += 100;
                        }
                            
                    }*/

                    //Adding to the timer
                    totalGameTime += gameTime.ElapsedGameTime.TotalSeconds;
                    tempTime += gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                #endregion
                #region Scoreboard State
                case GameState.Score:
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
            }

            //dev shortcuts
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                gameState = GameState.Title;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                gameState = GameState.Game;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                gameState = GameState.Score;
            }

            _prevMState = _mState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumPurple);

            _spriteBatch.Begin(transformMatrix: camera.Transform);

            // switch to control what is being drawn to the screen at each part of our fsm
            switch (gameState)      
            {
                #region Title State
                case GameState.Title:       
                    _spriteBatch.Draw(
                        coverArt, 
                        new Rectangle(
                            -(int)camera.Transform.Translation.X,
                            -(int)camera.Transform.Translation.Y, 
                            1920, 
                            1080),
                        Color.White);

                    strtButt.Draw(_spriteBatch);
                    quitButt.Draw(_spriteBatch);

                    _spriteBatch.DrawString(
                        fipps,
                        "ENTER THE ONEGEON",
                        new Vector2(-540, -150),
                        Color.White);
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

                    // Player
                    player.Draw(_spriteBatch);

                    /*foreach(Enemy en in enemyList)
                    {
                        en.Draw(_spriteBatch);
                    }*/
                    enemyManager.Draw(_spriteBatch, fipps);

                    //Showing some of the player stuff temporarily
                    /*// Score UI
                    _spriteBatch.DrawString(
                        fipps,
                        String.Format("Score: {0}", score),
                        new Vector2(350, 10),
                        Color.White);

                    // Timer UI
                    _spriteBatch.DrawString(
                        fipps,
                        String.Format("Time Elapsed: {0:F3}", totalGameTime),
                        new Vector2(580, 10),
                        Color.White);*/

                    // Bullet UI
                    _spriteBatch.Draw(
                        bulletAsset, 
                        new Rectangle(
                            400 - (int)camera.Transform.Translation.X, 
                            70 - (int)camera.Transform.Translation.Y, 
                            30, 
                            30), 
                        Color.White);

                    _spriteBatch.DrawString(
                        fipps,
                        String.Format("x{0}", player.BulletCount),
                        new Vector2(
                            4420 - (int)camera.Transform.Translation.X,
                            70 - (int)camera.Transform.Translation.Y),
                        Color.White);

                    // Player iframes
                    _spriteBatch.DrawString(
                        fipps,
                        String.Format("Iframe time: {0:F3}", player.IFrameTimeLeft),
                        new Vector2(
                            400 - (int)camera.Transform.Translation.X,
                            100 - (int)camera.Transform.Translation.Y),
                        Color.White);

                    // Show  1st enemy position
                    /*if(enemyList.Count > 0)
                    {
                        _spriteBatch.DrawString(
                            fipps,
                            String.Format("Enemy: {0}, {1}", enemyList[0].X, enemyList[0].Y),
                            new Vector2(
                                350,
                                60),
                            Color.White);
                    }
                    */

                    foreach (Bullet b in bulletList)
                    {
                        b.Draw(_spriteBatch);
                    }

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
                    _spriteBatch.Draw(
                        scoreBoard, 
                        new Rectangle(
                            -(int)camera.Transform.Translation.X,
                            -(int)camera.Transform.Translation.Y,
                            screenWidth, 
                            screenHeight), 
                        Color.White);

                    _spriteBatch.DrawString(
                        fipps,
                        enemyManager.Score.ToString(),
                        new Vector2(
                            screenWidth / 2 - camera.Transform.Translation.X,
                            screenHeight / 2 - camera.Transform.Translation.Y), 
                        Color.White);

                    menuButt.Draw(_spriteBatch);

                    quitButt.Draw(_spriteBatch);
                    break;
                #endregion

            }

            #region Debug hotkey text
            _spriteBatch.DrawString(
                fipps,
                "Press 1 for menu",
                new Vector2(-(int)camera.Transform.Translation.X + 10,
                            -(int)camera.Transform.Translation.Y + 40),
                Color.White);

            _spriteBatch.DrawString(
                fipps,
                "Press 2 for game",
                new Vector2(-(int)camera.Transform.Translation.X + 10,
                            -(int)camera.Transform.Translation.Y + 70),
                Color.White);

            _spriteBatch.DrawString(
                fipps,
                "Press 3 for score",
                new Vector2(-(int)camera.Transform.Translation.X + 10,
                            -(int)camera.Transform.Translation.Y + 100),
                Color.White);

            #endregion

            _spriteBatch.End();

            base.Draw(gameTime);
        }


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
    }
}
