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
        /// The pixels this bullet will move forward each time Update is run in Game1.cs
        /// </summary>
        private int speed;

        /// <summary>
        /// time which bullet is active
        /// Seconds for now
        /// </summary>
        private double timer;

        /// <summary>
        /// how many more enemies it can pass through
        /// </summary>
        private int passes;



        // Constructor
        // Parameterized
        // SHOULD DELETE LATER AFTER TESTING
        public Bullet(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            
            speed = 1;
            timer = 3;
            trajectory = new Vector2();
        }

        /// <summary>
        /// Creates a bullet using the 5 params
        /// </summary>
        /// <param name="sprite">The asset</param>
        /// <param name="rectangle">Currently the hitbox, asset dimensions, and intial postion</param>
        /// <param name="posToMoveTo">Point where you want the bullet to move to</param>
        /// <param name="spd">speed of the bullet</param>
        /// <param name="time">Time on the screen</param>
        public Bullet(Texture2D sprite, Rectangle rectangle, Vector2 posToMoveTo, int spd, double time) : base(sprite, rectangle)
        {
            speed = spd;
            timer = time;
            trajectory = base.VectorToPosition(posToMoveTo);
            trajectory.Normalize();
            passes = 1;
        }

        /// <summary>
        /// Same thing as other constructor just with the extra variable
        /// </summary>
        public Bullet(Texture2D sprite, Rectangle rectangle, Vector2 posToMoveTo, int spd, double time, int pass) : base(sprite, rectangle)
        {
            speed = spd;
            timer = time;
            trajectory = base.VectorToPosition(posToMoveTo);
            trajectory.Normalize();
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
                return timer > 0 && passes > 0;
            }
        }

        //For decrementing after it hits an enemy
        public void DecPasses()
        {
            passes--;
        }

        public void Update(GameTime gameTime)
        {
            if (this.Active)
            {
                timer -= gameTime.ElapsedGameTime.TotalSeconds;

                rectangle.X += (int)(trajectory.X * speed);
                rectangle.Y += (int)(trajectory.Y * speed);
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
