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
    abstract class Bullet : GameObject 
    {
        /// <summary>
        /// The pixels this bullet will move forward each time Update is run in Game1.cs
        /// </summary>
        private int speed;

        /// <summary>
        /// Speed can not be set at all
        /// </summary>
        public int Speed
        { get { return speed; } }

        // Constructor
        // Parameterized
        public Bullet(Texture2D sprite, Rectangle rectangle, int speed) : base(sprite, rectangle)
        {
            this.speed = speed;
        }

        public override void Update()
        {

        }

        public override void Move()
        {

        }

        /// <summary>
        /// Draws the bullet
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                sprite,
                rectangle,
                Color.White);
        }
    }
}
