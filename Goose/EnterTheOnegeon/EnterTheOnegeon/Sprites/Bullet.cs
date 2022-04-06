using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    class Bullet : GameObject 
    {
        /// <summary> coordinates of a point 1000 pixels in the direction of the mouse cursor </summary>
        private Vector2 trajectory;

        /// <summary> seconds taken to travel from player to trajectory</summary>
        private double speed;

        /// <summary> second the bullet was created </summary>
        //private double timeCreated;

        /// <summary> time the bullet has been alive </summary>
        private double timer;

        /// <summary> how many more enemies bullet can pass through </summary>
        private int passes;

        /// <summary> X coordinates of bullet's spawn location </summary>
        private int spawnX;

        /// <summary> the Y coordinates of bullet's spawn location </summary>
        private int spawnY;

        private int damage;

        //Creates inactive bullets
        public Bullet(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            // Bullets are created with *zero* stats
            // When a bullet is reset via Reset() being called in BulletManager, it gets
            // stats
        }

        /// <summary>
        /// Property that returns true when the bullet can pass through more enemies
        /// Bullets despawn when they collide with any wall
        /// </summary>
        public bool Active
        {
            get 
            {
                  return passes > 0 &&
                    // North wall
                    rectangle.Y > 0 + 96*2 &&
                    // West wall
                    rectangle.X > 0 + 96*2 &&
                    // East wall
                    rectangle.Y < 2176 - 32*2 &&
                    // South wall
                    rectangle.X < 3840 - 96*2;
            }
        }

        /// <summary>
        /// Whenever this  bullet hits an enemy decrement number of passes and make 
        /// the enemy take damage.
        /// </summary>
        /// <param name="enem"></param>
        /// <returns>true</returns>
        public void HitEnemy(Enemy enem)
        {
            passes--;
            enem.TakeDamage(damage);
        }

        public void HitPlayer(Player player)
        {
            passes--;
            player.TakeDamage(damage);
        }

        public void Update(GameTime gameTime)
        {
            if (this.Active)
            {
                // Sets timer to amount of time bullet has been alive
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                /* Original
                rectangle.X = (int)(spawnX + (trajectory.X * 1000 * timer / speed));
                rectangle.Y = (int)(spawnY + (trajectory.Y * 1000 * timer / speed));
                */
                int arbConst = 100;
                rectangle.X = (int)(spawnX + (trajectory.X * arbConst * timer * speed));
                rectangle.Y = (int)(spawnY + (trajectory.Y * arbConst * timer * speed));
            }
        }

        /// <summary>
        /// Draws this bullet if it is active
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if(this.Active)
            {
                base.Draw(sb);
            }
        }

        //Overriding so that when bullets are inactive they don't collide with anything
        public override bool CollideWith(GameObject other)
        {
            if (!Active)
                return false;
            return base.CollideWith(other);
        }

        /// <summary>
        /// Resets(Spawns) an inactive bullet
        /// Use the top right for where you spawn it
        /// </summary>
        /// <param name="spaX">Spawning x pos</param>
        /// <param name="spaY">Spawning y pos</param>
        /// <param name="posToMove">Where to go</param>
        /// <param name="bStats">The stats</param>
        public void Reset(int spaX, int spaY, Vector2 posToMove, BulletStats bStats)
        {
            rectangle = new Rectangle(spaX, spaY, bStats.Size, bStats.Size);
            spawnX = spaX;
            spawnY = spaY;
            trajectory = VectorToPosition(posToMove);
            trajectory.Normalize();
            timer = 0;
            speed = bStats.Speed;
            passes = bStats.Passes;
            damage = bStats.Damage;
        }
        
    }
}
