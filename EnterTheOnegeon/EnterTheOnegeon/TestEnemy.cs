using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class TestEnemy : Enemy
    {
        int speed;
        Vector2 place;
        Random rand;
        public TestEnemy(Texture2D sprite, Rectangle rectangle, int health) : base(sprite, rectangle, health)
        {
            speed = 5;
            rand = new Random();
            place = new Vector2(rand.Next(0, 600), rand.Next(0, 450));
        }
        public override bool CollideWith(GameObject other)
        {
            if (this.Position.Intersects(other.Position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Moves to random point
        /// </summary>
        public override void Move()
        {
            Vector2 direction = this.VectorToPosition(place);
            if(rectangle.X < 0 || rectangle.Y < 0 || rectangle.X > 600 || rectangle.Y > 450)
            {
                place.X = rand.Next(0, 600);
                place.Y = rand.Next(0, 450);
            }
            rectangle.X += (int) (direction.X * speed);
            rectangle.Y += (int) (direction.Y * speed);
        }

        public override void Update()
        {
            this.Move();
        }
    }
}
