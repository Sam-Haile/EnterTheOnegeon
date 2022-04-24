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
        private ShootEnState currState;
        private double shootCooldown;
        private double shootTimer;
        private BulletStats bStats;
        
        public ShootingEnemy(Texture2D sprite) : base(sprite, new Rectangle(), 0)
        {
            speed = 1;
            playerPos = new Vector2();
            currState = ShootEnState.Walking;
            bStats = new BulletStats();
            shootCooldown = 1;
            shootTimer = 0.1;
        }

        public bool IsShooting
        {
            get { return shootTimer <= 0; }
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
                switch (currState)
                {
                    case ShootEnState.Walking:
                        UpdateVelocity();
                        actualX += velocity.X;
                        actualY += velocity.Y;
                        base.UpdateRectanglePos();
                        if(VectorToPosition(playerPos).Length() < 300)
                        {
                            currState = ShootEnState.Shooting;
                        }
                        break;
                    case ShootEnState.Shooting:
                        if (VectorToPosition(playerPos).Length() > 600)
                        {
                            currState = ShootEnState.Walking;
                        }
                        if(shootTimer > 0)
                        {
                            shootTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            shootTimer = shootCooldown;
                            //bullet.Reset((int)Math.Round(this.actualX), (int)Math.Round(this.actualY), playerPos, bStats);
                        }
                        break;
                }
                //Also update the hitTime
                base.Update(gameTime);
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            if(Active)
            {
                switch (currState)
                {
                    case ShootEnState.Walking:
                        base.Draw(sb);
                        break;
                    case ShootEnState.Shooting:
                        if (shootTimer > 0)
                        {
                            sb.Draw(sprite, rectangle, Color.Green);
                        }
                        else
                        {
                            base.Draw(sb);
                        }
                        break;
                }
            }
            
        }

        /// <summary>
        /// Spawns the enemy at the given center point
        /// </summary>
        /// <param name="spawnPos"></param>
        /// <param name="enemyStats"></param>
        public void Reset(Point spawnPos, EnemyStats enemyStats, int sCD, BulletStats bull)
        {
            currState = ShootEnState.Walking;
            hitTimer = 0;
            rectangle.Width = enemyStats.Width;
            rectangle.Height = enemyStats.Height;
            actualX = spawnPos.X;
            actualY = spawnPos.Y;
            speed = enemyStats.Speed;
            health = enemyStats.Health;
            maxHealth = enemyStats.Health;
            damage = enemyStats.Damage;

            bStats = bull;

            shootCooldown = sCD;
            shootTimer = 0.1;
        }
    }
}
