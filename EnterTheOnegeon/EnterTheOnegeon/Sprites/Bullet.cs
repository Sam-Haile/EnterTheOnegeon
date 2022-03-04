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
        public Vector2 position;

        public bool isVisible;
        /// <summary>
        /// The pixels this bullet will move forward each time Update is run in Game1.cs
        /// </summary>
        private int speed;

        /// <summary>
        /// time which bullet is active
        /// Seconds for now
        /// </summary>
        private float timer;

        /// <summary>
        /// Speed can not be set at all
        /// </summary>
        public int Speed
        { get { return speed; } }


        // Constructor
        // Parameterized
        public Bullet(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            position = this.PositionV;
            speed = 1;
            timer = 5;
        }
        public Bullet(Texture2D sprite, Rectangle rectangle, Vector2 ) : base(sprite, rectangle)
        {
            position = this.PositionV;
            speed = 1;
            timer = 5;
        }

        /*
        public override void Update(GameTime gameTime, List<SpriteBatch>)
        {
            // METHOD HERE SHOULD REMOVE BULLET AFTER CERTAIN AMOUNT OF TIME
        }
        */

        public override void Move()
        {

        }

        public override bool IsDead()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override bool CollideWith(GameObject other)
        {
            throw new NotImplementedException();
        }
    }
}
