using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

/*
 * TO DO: 
 * >IMPLEMENT FIPPS FONT (https://www.dafont.com/fipps.font)
 * >
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
        List<Enemy> enemyList;

        // text fields
        SpriteFont verdana;

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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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
            enemyList = new List<Enemy>();

            // load font
            verdana = Content.Load<SpriteFont>("Verdana15");

            // load button texture and create all buttons
            T_Button = Content.Load<Texture2D>("T_Button");
            strtButt = new Button(verdana, T_Button, "Start", new Rectangle(30, GraphicsDevice.Viewport.Height - 90, 150, 75), Color.Gold);
            quitButt = new Button(verdana, T_Button, "Quit", new Rectangle(GraphicsDevice.Viewport.Width - 180, GraphicsDevice.Viewport.Height - 90, 150, 75), Color.Gold);
            menuButt = new Button(verdana, T_Button, "Menu", new Rectangle(30, GraphicsDevice.Viewport.Height - 90, 150, 75), Color.Gold);

        }

        protected override void Update(GameTime gameTime)
        {
            _prevKbState = _currentKbState;
            _currentKbState = Keyboard.GetState();
            _mState = Mouse.GetState();

            switch (gameState)
            {
                // -----------------
                // ---Title state---
                // -----------------
                case GameState.Title:
                    //Reset all the lists and player whenever going to title for now
                    player = new Player(playerAsset, new Rectangle(400, 350, 32, 64));
                    bulletList = new List<Bullet>();
                    enemyList = new List<Enemy>();
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

                    // temp dev shortcut to move to game state
                    else if (Keyboard.GetState().IsKeyDown(Keys.D2))     
                    {
                        gameState = GameState.Game;
                    }

                    // temp dev shortcut to move to score state
                    else if (Keyboard.GetState().IsKeyDown(Keys.D3))     
                    {
                        gameState = GameState.Score;
                    }
                    break;
                // ---------------------
                // ---Main game state---
                // ---------------------
                case GameState.Game:
                    if(!player.Active)
                    {
                        gameState = GameState.Score;
                    }

                    //Enemy spawning logic here
                    if (totalGameTime < 20)
                    {
                        if (tempTime > 2)
                        {
                            SpawnEnemy(3);
                            tempTime = 0;
                        }
                    }
                    else if (totalGameTime < 40)
                    {
                        if (tempTime > 2)
                        {
                            SpawnEnemy(5);
                            tempTime = 0;
                        }
                    }
                    else
                    {
                        if (tempTime > 2)
                        {
                            SpawnEnemy(10);
                            tempTime = 0;
                        }
                    }

                    // players movement
                    player.Move();

                    MouseState mouse = Mouse.GetState();
                    IsMouseVisible = true;

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
                                new Vector2(
                                    _mState.X, 
                                    _mState.Y), 
                                1, 
                                gameTime));

                        player.BulletCount--;
                    }
                    _prevKbState = Keyboard.GetState();

                    // enemy updating
                    foreach (Enemy en in enemyList)
                    {
                        
                        ((TestEnemy)en).Update(player);
                        if(en.CollideWith(player))
                        {
                            en.HitPlayer(player);
                        }
                    }

                    //Bullets testing
                    foreach (Bullet b in bulletList)
                    {
                        b.Update(gameTime);
                        foreach(Enemy en in enemyList)
                        {
                            if(b.CollideWith(en))
                            {
                                b.HitEnemy(en);
                                player.BulletCount++;
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

                    // Remove enemies that are shot
                    for (int i = enemyList.Count - 1; i >= 0; i--)
                    {
                        if (!enemyList[i].Active)
                        {
                            enemyList.RemoveAt(i);
                            score += 100;
                        }
                            
                    }

                    //Adding to the timer
                    totalGameTime += gameTime.ElapsedGameTime.TotalSeconds;
                    tempTime += gameTime.ElapsedGameTime.TotalSeconds;

                    //temp dev shortcut until buttons are implimented
                    if (Keyboard.GetState().IsKeyDown(Keys.D1))
                    {
                        gameState = GameState.Title;
                    }

                    //temp dev shortcut until buttons are implimented
                    if (Keyboard.GetState().IsKeyDown(Keys.D3))
                    { 
                        gameState = GameState.Score;
                    }
                    break;

                // ----------------------
                // ---Scoreboard state---
                // ----------------------
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

                    //temp dev shortcut until buttons are implimented
                    else if (Keyboard.GetState().IsKeyDown(Keys.D2))
                    { 
                        gameState = GameState.Game;
                    }

                    //temp dev shortcut until buttons are implimented
                    else if (Keyboard.GetState().IsKeyDown(Keys.D1))     
                    {
                        gameState = GameState.Title;
                    }
                    break;
            }

            _prevMState = _mState;

            base.Update(gameTime);
        }

        public void UpdateBullets()
        {
            /* if bullet is certain distance from player
            foreach (NormalBullet bullet in bullets)
            {
                bullet.position += bullet.velocity;
                if (Vector2.Distance(bullet.position, spritePosition) > 500)
                {
                    bullet.isVisible = false;
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                // remove it from screen and list
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }*/
        }

        // Not sure how to display bullets at the front of sprite
        /*
        public void Shoot()
        {
            NormalBullet newBullet = new NormalBullet(Content.Load<Texture2D>("Bullet"), rectangle);

            // retrieve angle of the player and shoots bullet at current angle
            // also prevent bullet from colliding into own player
            newBullet.velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 5f + player.Speed;

            // bullets position is equal to front of player and shoots out
            newBullet.position = spritePosition + newBullet.velocity * 5;
            newBullet.isVisible = true;

            if (bullets.Count < 20)
            {
                bullets.Add(newBullet);
            }
        }
        */

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumPurple);

            _spriteBatch.Begin();

            // switch to control what is being drawn to the screen at each part of our fsm
            switch (gameState)      
            {
                // ---Title state---
                case GameState.Title:       
                    _spriteBatch.Draw(
                        coverArt, 
                        new Rectangle(
                            0, 
                            0, 
                            800, 
                            480),
                        Color.White);

                    strtButt.Draw(_spriteBatch);
                    quitButt.Draw(_spriteBatch);

                    #region Text
                    _spriteBatch.DrawString(
                        verdana,
                        "ENTER THE ONEGEON",
                        new Vector2(10, 10),
                        Color.White);

                    _spriteBatch.DrawString(
                        verdana,
                        "Press 2 for game",
                        new Vector2(10, 30),
                        Color.White);

                    _spriteBatch.DrawString(
                        verdana,
                        "Press 3 for scores",
                        new Vector2(10, 80),
                        Color.White);
                    #endregion
                    break;

                // ---Main game state---
                case GameState.Game:        
                    _spriteBatch.Draw(
                        dungeon, 
                        new Rectangle(
                            0,
                            0,
                            800,
                            480), 
                        Color.White);

                    player.Draw(_spriteBatch);

                    foreach(Enemy en in enemyList)
                    {
                        en.Draw(_spriteBatch);
                    }

                    //Showing some of the player stuff temporarily
                    // Score UI
                    _spriteBatch.DrawString(
                        verdana,
                        String.Format("Score: {0}", score),
                        new Vector2(350, 10),
                        Color.White);

                    // Timer UI
                    _spriteBatch.DrawString(
                        verdana,
                        String.Format("Time Elapsed: {0:F3}", totalGameTime),
                        new Vector2(580, 10),
                        Color.White);

                    // Bullet UI
                    _spriteBatch.Draw(
                        bulletAsset, 
                        new Rectangle(
                            350, 
                            30, 
                            30, 
                            30), 
                        Color.White);

                    _spriteBatch.DrawString(
                        verdana,
                        String.Format("x{0}", player.BulletCount),
                        new Vector2(380, 30),
                        Color.White);

                    // Show  1st enemy position
                    /*if(enemyList.Count > 0)
                    {
                        _spriteBatch.DrawString(
                            verdana,
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

                    //foreach (NormalBullet bullet in bullets)
                    //{
                    //    bullet.Draw(_spriteBatch);
                    //}

                    #region Text
                    _spriteBatch.DrawString(
                        verdana,
                        "Press 1 for menu",
                        new Vector2(
                            10, 
                            50),
                        Color.White);
                    _spriteBatch.DrawString(
                        verdana,
                        "Press 3 for scores",
                        new Vector2(
                            10, 
                            80),
                        Color.White);
                    #endregion

                    //Drawing the line from player to cursor
                    for (int i = 0; i < 20; i++)
                    {
                        _spriteBatch.Draw(
                            dungeon, 
                            new Rectangle(
                                player.CenterX + ((_mState.X - player.CenterX) * i / 20), 
                                player.CenterY + ((_mState.Y - player.CenterY) * i / 20), 
                                4, 
                                4), 
                            Color.Black);
                    }
                    break;

                // ---Scoreboard state---
                case GameState.Score:       
                    _spriteBatch.Draw(
                        scoreBoard, 
                        new Rectangle(
                            0, 
                            0, 
                            800, 
                            480), 
                        Color.White);

                    _spriteBatch.DrawString(
                        verdana,
                        score.ToString(),
                        new Vector2(
                            400, 
                            200), 
                        Color.White);

                    menuButt.Draw(_spriteBatch);

                    quitButt.Draw(_spriteBatch);

                    #region Text
                    _spriteBatch.DrawString(verdana,
                        "Press 1 for menu",
                        new Vector2(10, 50),
                        Color.White);

                    _spriteBatch.DrawString(verdana,
                        "Press 2 for game",
                        new Vector2(10, 30),
                        Color.White);
                    #endregion
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //Temp helping method to spawn a certain number of enemies
        public void SpawnEnemy(int num)
        {
            for(int i = 0; i < num; i++)
            {
                enemyList.Add(
                    new TestEnemy(
                        enemyAsset, 
                        new Rectangle(
                            RandPoint(), 
                            new Point(
                                50, 
                                50)),
                        1));
            }

        }
        //gets a random point with a certain distance to the player
        // Above commented out that and now does off screen
        public Point RandPoint()
        {
            /*
            int randx = random.Next(0, GraphicsDevice.Viewport.Width - 200);
            int randy = random.Next(0, GraphicsDevice.Viewport.Height - 100);
            if(randx > player.X - 30)
            {
                randx += 200;
            }
            if (randy > player.Y - 30)
            {
                randy += 100;
            }

            return new Point(randx, randy);
            */

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
