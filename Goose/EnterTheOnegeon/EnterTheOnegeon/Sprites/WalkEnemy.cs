using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    /// <summary>
    /// Simple enemy that only walks towards the player
    /// </summary>
    class WalkEnemy : Enemy
    {
        private Vector2 playerPos;

        /// <summary>
        /// Makes an inactive one
        /// </summary>
        /// <param name="sprite">Sprite</param>
        public WalkEnemy(Texture2D sprite) : base(sprite, new Rectangle(), 0)
        {
            speed = 1;
            playerPos = new Vector2();
        }

        //Test purposes
        public WalkEnemy(Texture2D sprite, Rectangle rectangle, int health, int spd) : base(sprite, rectangle, health)
        {
            speed = spd;
            playerPos = new Vector2();
        }

        /// <summary>
        /// Updates the velocity based on where the player is
        /// </summary>
        private void UpdateVelocity()
        {
            velocity = this.VectorToPosition(playerPos);
            if (velocity.Length() != 0)
            {
                velocity.Normalize();
            }
            velocity.X *= (float)speed;
            velocity.Y *= (float)speed;
        }

        /// <summary>
        /// Changes player position, calls update velocity, and applies the velocity
        /// </summary>
        public void Update(Player p, GameTime gameTime)
        {
            if (this.Active)
            {
                playerPos = p.PositionV;
                UpdateVelocity();
                actualX += velocity.X;
                actualY += velocity.Y;

                //Need to adjust rectangle 
                rectangle.X = (int)(Math.Round(actualX) - rectangle.Width/2);
                rectangle.Y = (int)(Math.Round(actualY) - rectangle.Height/2);
                //Also update the hitTime
                base.Update(gameTime);
            }
           
        }

        public void Reset(Point spawnPos, EnemyStats enemyStats)
        {
            hitTimer = 0;
            rectangle.Width = enemyStats.Width;
            rectangle.Height = enemyStats.Height;
            actualX = spawnPos.X;
            actualY = spawnPos.Y;
            speed = enemyStats.Speed;
            health = enemyStats.Health;
            maxHealth = enemyStats.Health;
            damage = enemyStats.Damage;
        }
        //Also changes the sprite
        public void Reset(Texture2D sprite, Point spawnPos, EnemyStats enemyStats)
        {
            this.Reset(spawnPos, enemyStats);
            this.sprite = sprite;
        }
    }
}
