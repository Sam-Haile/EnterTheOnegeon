using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        enum GameState { Title, Game, Score}        // base fsm, other enums to be added include Help + Pause
        private GameState gameState = GameState.Title;

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
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
            switch (gameState)      // switch to control what is being drawn to the screen at each part of our fsm
            {
                case GameState.Title:       // what is being drawn while in the title screen

                    break;
                case GameState.Game:        // what is happening while in the game state

                    break;
                case GameState.Score:       // what is happening while on the scoreboard/death screen

                    break;
            }

            base.Draw(gameTime);
        }
    }
}
