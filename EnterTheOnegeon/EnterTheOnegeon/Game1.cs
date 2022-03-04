using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace EnterTheOnegeon
{
    /// <summary>
    /// Contains all code to run Enter the Onegeon
    
    // base fsm, other enums to be added include Help + Pause
    enum GameState
    {
        Title,
        Game,
        Score
    }
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState gameState = GameState.Title;

        // handles keyboard input
        KeyboardState _currentKbState;
        KeyboardState _prvsKbState;
        //handles mouse input
        MouseState _mState;
        MouseState _prevMState;
        
        // player fields
        Texture2D playerAsset;
        Player player;

        // bullet fields
        Vector2 distance;
        float rotation;
        Vector2 spriteVelocty;

        Vector2 spritePosition;

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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentKbState = Keyboard.GetState();
            _prvsKbState = _currentKbState;

            // initialize background texture
            coverArt = Content.Load<Texture2D>("coverArt");
            dungeon = Content.Load<Texture2D>("dungeon");
            scoreBoard = Content.Load<Texture2D>("scoreBoard");

            // initialize player and its asset
            playerAsset = Content.Load<Texture2D>("player");
            // for now, i put the location of the sprite near the bottom of the screen
            player = new Player(playerAsset, new Rectangle(400, 350, 16, 32));

            // loading enemy and initializing a list
            enemyAsset = Content.Load<Texture2D>("badguy");
            enemyList = new List<Enemy>() { new TestEnemy(enemyAsset, new Rectangle(50, 50, 40, 40), 1),
                                            new TestEnemy(enemyAsset, new Rectangle(500, 700, 40, 40), 1),
                                            new TestEnemy(enemyAsset, new Rectangle(0, 200, 40, 40), 1)};
            // load font
            verdana = Content.Load<SpriteFont>("Verdana15");

            T_Button = Content.Load<Texture2D>("T_Button");
            strtButt = new Button(verdana, T_Button, "Start", new Rectangle(30, GraphicsDevice.Viewport.Height - 90, 150, 75), Color.Gold);
            quitButt = new Button(verdana, T_Button, "Start", new Rectangle(30, GraphicsDevice.Viewport.Height - 90, 150, 75), Color.Gold);
            menuButt = new Button(verdana, T_Button, "Start", new Rectangle(30, GraphicsDevice.Viewport.Height - 90, 150, 75), Color.Gold);

        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            _prvsKbState = _currentKbState;
            _currentKbState = Keyboard.GetState();
            _mState = Mouse.GetState();

            switch (gameState)
            {
                case GameState.Title:
                    if(_mState.X < strtButt.ButtRect.X + strtButt.ButtRect.Width && _mState.X > strtButt.ButtRect.X && _mState.Y < strtButt.ButtRect.Y + strtButt.ButtRect.Height && _mState.Y > strtButt.ButtRect.Y && _mState.LeftButton == ButtonState.Released && _prevMState.LeftButton == ButtonState.Pressed)
                    {
                        gameState = GameState.Game;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.D2))     //temp dev shortcut until buttons are implimented
                    {
                        gameState = GameState.Game;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D3))     //temp dev shortcut until buttons are implimented
                    {
                        gameState = GameState.Score;
                    }
                    break;
                case GameState.Game:
                    
                    // players movement
                    player.Move();

                    MouseState mouse = Mouse.GetState();
                    IsMouseVisible = true;

                    distance.X = mouse.X - spritePosition.X;
                    distance.Y = mouse.Y - spritePosition.Y;

                    //rotation = (float)Math.Atan2(distance.Y, distance.X);

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && _prvsKbState.IsKeyUp(Keys.Space))
                    {
                        //Shoot();
                    }
                    _prvsKbState = Keyboard.GetState();

                    //enemy updating
                    foreach (Enemy en in enemyList)
                    {
                        
                        ((TestEnemy)en).Update(player);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D1))     //temp dev shortcut until buttons are implimented
                    {
                        gameState = GameState.Title;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D3))     //temp dev shortcut until buttons are implimented
                    {
                        gameState = GameState.Score;
                    }

                    break;
                case GameState.Score:


                    if (Keyboard.GetState().IsKeyDown(Keys.D2))     //temp dev shortcut until buttons are implimented
                    {
                        gameState = GameState.Game;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D1))     //temp dev shortcut until buttons are implimented
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
            // if bullet is certain distance from player
            //foreach (NormalBullet bullet in bullets)
            //{
            //    bullet.position += bullet.velocity;
            //    if (Vector2.Distance(bullet.position, spritePosition) > 500)
            //    {
            //        bullet.isVisible = false;
            //    }
            //}
            //for (int i = 0; i < bullets.Count; i++)
            //{
            //    // remove it from screen and list
            //    if (!bullets[i].isVisible)
            //    {
            //        bullets.RemoveAt(i);
            //        i--;
            //    }
            //}
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

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            switch (gameState)      // switch to control what is being drawn to the screen at each part of our fsm
            {
                case GameState.Title:       // what is being drawn while in the title screen
                    _spriteBatch.Draw(coverArt, new Rectangle(0, 0, 800, 480), Color.White);
                    strtButt.Draw(_spriteBatch);

                    #region Text
                    _spriteBatch.DrawString(verdana,
                        "ENTER THE ONEGEON",
                        new Vector2(10, 10),
                        Color.White);

                    _spriteBatch.DrawString(verdana,
                        "Press 2 for game",
                        new Vector2(10, 30),
                        Color.White);

                    _spriteBatch.DrawString(verdana,
                        "Press 3 for scores",
                        new Vector2(10, 80),
                        Color.White);
                    #endregion

                    break;
                case GameState.Game:        // what is happening while in the game state

                    _spriteBatch.Draw(dungeon, new Rectangle(0,0,800,480), Color.White);
                    player.Draw(_spriteBatch);
                    foreach(Enemy en in enemyList)
                    {
                        en.Draw(_spriteBatch);
                    }

                    //foreach (NormalBullet bullet in bullets)
                    //{
                    //    bullet.Draw(_spriteBatch);
                    //}
                    #region Text
                    _spriteBatch.DrawString(verdana,
                        "Press 1 for menu",
                        new Vector2(10, 50),
                        Color.White);
                    _spriteBatch.DrawString(verdana,
                        "Press 3 for scores",
                        new Vector2(10, 80),
                        Color.White);
                    #endregion

                    break;
                case GameState.Score:       // what is happening while on the scoreboard/death screen

                    _spriteBatch.Draw(scoreBoard, new Rectangle(0, 0, 800, 480), Color.White);
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
    }
}
