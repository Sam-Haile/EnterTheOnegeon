using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    // Bullet Class
    // Child of GameObject
    abstract class Bullet : GameObject 
    {
        // Field
        private int speed;

        // Property
        public int Speed
        { get { return speed; } }

        // Constructor
        // Parameterized
        public Bullet(int speed, Texture2D sprite, Rectangle hitbox) : base(sprite, hitbox)
        {
            this.speed = speed;
        }

        public override void Update()
        {

        }

        public override void Move()
        {

        }

        // Draw Bullet
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                sprite,
                hitbox,
                Color.White);
        }
    }
}
