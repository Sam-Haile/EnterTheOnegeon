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
        private int speed;

        /// <summary> second the bullet was created </summary>
        private double timeCreated;

        /// <summary> time the bullet has been alive </summary>
        private double timer;

        /// <summary> how many more enemies bullet can pass through </summary>
        private int passes;

        /// <summary> X coordinates of bullet's spawn location </summary>
        private int spawnX;

        /// <summary> the Y coordinates of bullet's spawn location </summary>
        private int spawnY;

        // Constructor
        // Parameterized
        // SHOULD DELETE LATER AFTER TESTING
        // MAYBE NOT DELETE AND IT CAN BE USED FOR CREATING AN INACTIVE BULLET
        //public Bullet(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        //{
        //    
        //    speed = 1;
        //    timer = 3;
        //    trajectory = new Vector2();
        //}

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

            timeCreated = gameTime.TotalGameTime.TotalSeconds;

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

            timeCreated = gameTime.TotalGameTime.TotalSeconds;

            spawnX = rectangle.X;
            spawnY = rectangle.Y;

            passes = pass;

            trajectory = VectorToPosition(posToMoveTo);
            trajectory.Normalize();
        }

        /*
         * Delete the other constructors after implementing this
         * Instead of having so many params use only a bulletstats struct to instantiate all the fields we would need
        public Bullet(Texture2D sprite, Rectangle rectangle, BulletStats bStats) : base(sprite, rectangle)
        {
            speed = bStats.Speed etc.
        }
        */


        /// <summary>
        /// Property that returns true when the bullet can pass through more enemies
        /// Bullets despawn when 100 pixels offscreen in any direction
        /// </summary>
        public bool Active
        {
            get 
            {
                return passes > 0 && 
                   rectangle.Y > 0 - 100 && 
                   rectangle.X > 0 - 100 &&
                   rectangle.Y < 1080 + 100 &&
                   rectangle.X < 1920 + 100;
            }
        }

        //Whenever this  bullet hits an enemy decrement number of passes and make the enemy take damage
        public void HitEnemy(Enemy enem)
        {
            passes--;
            enem.TakeDamage(1);
        }

        public void Update(GameTime gameTime)
        {
            if (this.Active)
            {
                // Sets timer to amount of time bullet has been alive
                timer = gameTime.TotalGameTime.TotalSeconds - timeCreated;
                /*
                rectangle.X = (int)(spawnX + (trajectory.X * 1000 * timer / speed));
                rectangle.Y = (int)(spawnY + (trajectory.Y * 1000 * timer / speed));
                 */
                rectangle.X = (int)(spawnX + (trajectory.X * 1000 * timer / speed));
                rectangle.Y = (int)(spawnY + (trajectory.Y * 1000 * timer / speed));
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if(this.Active)
            {
                base.Draw(sb);
            }
        }

        /*
         * Make it take a bulletStats struct to essentially do what a constructor does
         * For object pooling purposes
        public void ResetBullet(BulletStats bStats)
        {

        }
        */
    }
}
