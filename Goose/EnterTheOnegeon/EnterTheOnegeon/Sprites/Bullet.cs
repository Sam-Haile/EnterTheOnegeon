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

            speed = 1;
            timer = 0;
            passes = 0;
            trajectory = new Vector2();
        }

        /// <summary>
        /// Creates a bullet using the 5 params
        /// </summary>
        /// <param name="sprite">The asset</param>
        /// <param name="rectangle">Currently the hitbox, asset dimensions, and intial postion</param>
        /// <param name="posToMoveTo">Point where you want the bullet to move to</param>
        /// <param name="spd">seconds it will take the bullet to travel 1000 pixels</param>
        /// <param name="time">Time on the screen</param>
        public Bullet(Texture2D sprite, Rectangle rectangle, GameTime gameTime, Vector2 posToMoveTo, int spd) 
            : base(sprite, rectangle)
        {
            speed = spd;

            //timeCreated = gameTime.TotalGameTime.TotalSeconds;
            timer = 0;

            spawnX = rectangle.X;
            spawnY = rectangle.Y;

            passes = 1; // no piercing enemies

            trajectory = VectorToPosition(posToMoveTo);
            trajectory.Normalize();
        }

        /// <summary>
        /// Same thing as other constructor just with the extra variable
        /// </summary>
        public Bullet(Texture2D sprite, Rectangle rectangle, GameTime gameTime, Vector2 posToMoveTo, int spd, 
           int pass) : base(sprite, rectangle)
        {
            speed = spd;

            //timeCreated = gameTime.TotalGameTime.TotalSeconds;
            timer = 0;

            spawnX = rectangle.X;
            spawnY = rectangle.Y;

            passes = pass;

            trajectory = VectorToPosition(posToMoveTo);
            trajectory.Normalize();
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

        //Whenever this  bullet hits an enemy decrement number of passes and make the enemy take damage
        public void HitEnemy(Enemy enem)
        {
            passes--;
            enem.TakeDamage(1);
        }

        public void HitPlayer(Player player)
        {
            passes--;
            player.TakeDamage(1);
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
        /// Use the center for where you spawn it
        /// </summary>
        /// <param name="spaX">Spawning x pos</param>
        /// <param name="spaY">Spawning y pos</param>
        /// <param name="posToMove">Where to go</param>
        /// <param name="bStats">The stats</param>
        public void Reset(int spaX, int spaY, Vector2 posToMove, BulletStats bStats)
        {
            rectangle = new Rectangle(spaX-bStats.Size / 2, spaY - bStats.Size / 2, bStats.Size, bStats.Size);
            spawnX = spaX;
            spawnY = spaY;
            trajectory = VectorToPosition(posToMove);
            trajectory.Normalize();
            timer = 0;
            speed = bStats.Speed;
            passes = bStats.Passes;
        }
        
    }
}
