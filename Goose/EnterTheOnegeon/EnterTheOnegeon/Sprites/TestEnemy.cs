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

        /* Does the same as gameobject's collidewith
        /// <summary>
        /// returns true if collsision is detected
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool CollideWith(GameObject other)
        {
            if (!Active)
                return false;
            return base.CollideWith(other);
        }
        */

        /// <summary>
        /// Movement testing for now
        /// </summary>
        public void Move()
        {
            // Moves to center of player, not upper left
            Vector2 direction = this.VectorToPosition(playerPos);
            
            if(direction.Length() > 0)
            {
                direction.Normalize();
            }
            /* 
            posX += direction.X * speed;
            posY += direction.Y * speed;

            //If they teleport then they stack
            rectangle.X = (int)(Math.Round(posX));
            rectangle.Y = (int)(Math.Round(posY));
            */

            rectangle.X += (int) (Math.Round(direction.X * speed));
            rectangle.Y += (int) (Math.Round(direction.Y * speed));
        }

        public void Update(Player p, GameTime gameTime)
        {
            if(this.Active)
            {
                playerPos = p.PositionV;
                this.Move();
            }
        }

        public void Reset(int hp)
        {
            speed = 3;
            health = hp;
            maxHealth = hp;
        }
    }
}
