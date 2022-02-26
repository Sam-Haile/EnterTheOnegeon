﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        // player fields
        Texture2D playerAsset;
        Player player;
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

            // initialize player and its asset
            playerAsset = Content.Load<Texture2D>("player");
            // for now, i put the location of the sprite near the bottom of the screen
            player = new Player(playerAsset, new Rectangle(400, 350, 40, 40));
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            _prvsKbState = _currentKbState;
            _currentKbState = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.Title:
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            switch (gameState)      // switch to control what is being drawn to the screen at each part of our fsm
            {
                case GameState.Title:       // what is being drawn while in the title screen

                    break;
                case GameState.Game:        // what is happening while in the game state
                    player.Draw(_spriteBatch);

                    player.Draw(_spriteBatch);

                    break;
                case GameState.Score:       // what is happening while on the scoreboard/death screen

                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
