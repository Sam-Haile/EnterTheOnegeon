using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    /// <summary>
    /// The player controlled character. Dies when health < 1
    /// </summary>
    class Player : GameObject
    {
        private int speed;
        private bool isDead;

        public Player(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            speed = 1;
            isDead = false;
        }

        public override void Update()
        {

        }

        /// <summary>
        /// ToDo: Keyboard input
        /// </summary>
        public override void Move()
        {
            KeyboardState keypress = Keyboard.GetState();

            // speed at which player moves is subject to change
            if (keypress.IsKeyDown(Keys.W))
            {
                rectangle.Y -= 5;
            }
            if (keypress.IsKeyDown(Keys.A))
            {
                rectangle.X -= 5;
            }
            if (keypress.IsKeyDown(Keys.S))
            {
                rectangle.Y += 5;
            }
            if (keypress.IsKeyDown(Keys.D))
            {
                rectangle.X += 5;
            }
        }

        public override bool IsDead()
        {
            return true;
        }

        public override bool CollideWith(GameObject other)
        {
            if (this.Position.Intersects(other.Position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
