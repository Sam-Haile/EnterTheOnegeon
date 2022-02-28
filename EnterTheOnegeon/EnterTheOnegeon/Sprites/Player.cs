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
        private int bulletCount;
        private bool isDead;

        public Player(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            speed = 5;
            bulletCount = 1;
            isDead = false;
        }

        public override void Update()
        {
            Move();
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
                rectangle.Y -= speed;
            }
            if (keypress.IsKeyDown(Keys.A))
            {
                rectangle.X -= speed;
            }
            if (keypress.IsKeyDown(Keys.S))
            {
                rectangle.Y += speed;
            }
            if (keypress.IsKeyDown(Keys.D))
            {
                rectangle.X += speed;
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

        public void Shoot()
        {

        }

        public void Parry(GameObject other)
        {
            //foreach ()
            {
                if (CollideWith(other))
                {
                    bulletCount += 1;
                }
            }
        }
    }
}
