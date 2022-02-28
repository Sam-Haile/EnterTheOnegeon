using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class TestEnemy : Enemy
    {
        int speed;
        public TestEnemy(Texture2D sprite, Rectangle rectangle, int health) : base(sprite, rectangle, health)
        {
            speed = 5;
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

        /// <summary>
        /// Moves arbitrarily to a point of the screen for now
        /// </summary>
        public override void Move()
        {
            Vector2 direction = this.VectorToPosition(300, 300);
            rectangle.X += (int) (direction.X * speed);
            rectangle.Y += (int) (direction.Y * speed);
        }

        public override void Update()
        {
            this.Move();
        }
    }
}
