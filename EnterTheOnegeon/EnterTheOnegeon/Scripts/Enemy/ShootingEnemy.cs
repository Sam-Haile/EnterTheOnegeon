using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    public enum ShootEnState
    {
        Walking,
        Shooting
    }
    class ShootingEnemy : Enemy
    {
        //fields
        private Vector2 playerPos;
        
        public ShootingEnemy(Texture2D sprite) : base(sprite, new Rectangle(), 0)
        {
            speed = 1;
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
                base.UpdateRectanglePos();

                //Also update the hitTime
                base.Update(gameTime);
            }

        }

        /// <summary>
        /// Spawns the enemy at the given center point
        /// </summary>
        /// <param name="spawnPos"></param>
        /// <param name="enemyStats"></param>
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
    }
}
