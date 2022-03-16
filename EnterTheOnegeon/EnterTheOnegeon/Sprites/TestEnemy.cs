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
        private int speed;
        private Vector2 playerPos;
        public TestEnemy(Texture2D sprite, Rectangle rectangle, int health) : base(sprite, rectangle, health)
        {
            speed = 2;
            playerPos = new Vector2();
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
        /// Movement testing for now
        /// </summary>
        public void Move()
        {
            Vector2 direction = this.VectorToPosition(playerPos);
            
            if(direction.Length() > 1)
                direction.Normalize();

            rectangle.X += (int) (Math.Round(direction.X * speed));
            rectangle.Y += (int) (Math.Round(direction.Y * speed));
        }

        public void Update(Player p)
        {
            playerPos = p.PositionV;
            this.Move();
        }

    }
}
