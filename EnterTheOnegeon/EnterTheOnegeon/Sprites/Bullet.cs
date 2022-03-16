using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    /// <summary>
    /// Bullet Class (Child of GameObject) - Creates an object that moves at speed "speed"
    /// </summary>
    class Bullet : GameObject 
    {
        private Vector2 trajectory;

        /// <summary>
        /// seconds the bullet takes to travel 1000 pixels in the direction of (spawnX, spawnY)
        /// </summary>
        private int speed;

        /// <summary>
        /// second the bullet was created
        /// </summary>
        private double timeCreated;

        /// <summary>
        /// total time the bullet has been alive
        /// Seconds for now
        /// </summary>
        private double timer;

        /// <summary>
        /// how many more enemies it can pass through
        /// </summary>
        private int passes;

        /// <summary>
        /// the X coordinates of the bullet's spawn location
        /// </summary>
        private int spawnX;

        /// <summary>
        /// the Y coordinates of the bullet's spawn location
        /// </summary>
        private int spawnY;

        // Constructor
        // Parameterized
        // SHOULD DELETE LATER AFTER TESTING
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
        public Bullet(Texture2D sprite, Rectangle rectangle, Vector2 posToMoveTo, int spd,
            GameTime gameTime) : base(sprite, rectangle)
        {
            speed = spd;
            timeCreated = gameTime.TotalGameTime.TotalSeconds;
            spawnX = rectangle.X;
            spawnY = rectangle.Y;
            trajectory = VectorToPosition(posToMoveTo);
            trajectory.Normalize();
            trajectory.X = trajectory.X * 1000;
            trajectory.Y = trajectory.Y * 1000;
            passes = 1;
        }

        /// <summary>
        /// Same thing as other constructor just with the extra variable
        /// </summary>
        public Bullet(Texture2D sprite, Rectangle rectangle, Vector2 posToMoveTo, int spd, 
            GameTime gameTime, int pass) : base(sprite, rectangle)
        {
            speed = spd;
            timeCreated = gameTime.TotalGameTime.TotalSeconds;
            spawnX = rectangle.X;
            spawnY = rectangle.Y;
            trajectory = VectorToPosition(posToMoveTo);
            trajectory.Normalize();
            trajectory.X = trajectory.X * 1000;
            trajectory.Y = trajectory.Y * 1000;
            passes = pass;
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
        /// Property that returns true when the bullet is active
        /// CheckList:
        /// -timer is more than 0
        /// -if it has passed through more enemies
        /// </summary>
        public bool Active
        {
            get 
            {
                return passes > 0;
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

                rectangle.X = (int)(spawnX + (trajectory.X * timer / speed));
                rectangle.Y = (int)(spawnY + (trajectory.Y * timer / speed));
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
