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
        private Vector2 position;

        private Vector2 trajectory;

        public bool isVisible;
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
        /// Speed can not be set at all
        /// </summary>
        public int Speed
        { get { return speed; } }


        // Constructor
        // Parameterized
        // SHOULD DELETE LATER AFTER TESTING
        public Bullet(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            position = this.PositionV;
            speed = 1;
            timer = 3;
            trajectory = new Vector2();
        }


        public Bullet(Texture2D sprite, Rectangle rectangle, Vector2 posToMoveTo, int spd, double time) : base(sprite, rectangle)
        {
            position = this.PositionV;
            speed = spd;
            timer = time;
            trajectory = base.VectorToPosition(posToMoveTo);
            trajectory.Normalize();
        }

        //public Bullet(Texture2D sprite, Rectangle rectangle, Vector2 spawnPos, int spd, double time) : base(sprite, rectangle)
        //{
        //    position = spawnPos;
        //    speed = spd;
        //    timer = time;
        //}

        public void Update(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.TotalSeconds;
            
            rectangle.X += (int)(trajectory.X * speed);
            rectangle.Y += (int)(trajectory.Y * speed);
        }

        public override void Draw(SpriteBatch sb)
        {
            if(!TimeUp())
            {
                base.Draw(sb);
            }
        }

        public bool TimeUp()
        {
            return timer <= 0;
        }
    }
}
