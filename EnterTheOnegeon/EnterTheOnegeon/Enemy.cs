using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    /// <summary>
    /// The base class that every enemy type will inherit from
    /// </summary>
    abstract class Enemy : GameObject
    {
        /// <summary>
        /// The health of the enemy, when it goes below one the enemy is dead
        /// </summary>
        protected int health;

        /// <summary>
        /// returns enemies health
        /// </summary>
        public int Health 
        {
            get { return health; }
        }

        public Enemy(Texture2D sprite, Rectangle rectangle, int health) : base(sprite, rectangle)
        {
            this.health = health;
        }


        /// <summary>
        /// determines if enemy is dead
        /// </summary>
        /// <returns>true if enemy is dead, false if enemy is alive</returns>
        public override bool IsDead()
        {
            if (health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Gets a normal vector for the direction to a position defined by 2 doubles
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns></returns>
        public Vector2 VectorToPosition(double x, double y)
        {
            Vector2 temp = new Vector2((float)(x - this.CenterX), (float)(y - this.CenterY));
            temp.Normalize();
            return temp;
        }
        /// <summary>
        /// Gets a normal vector for the direction to a position defined by a vector2
        /// </summary>
        /// <param name="pos">Vector2 position</param>
        /// <returns></returns>
        public Vector2 VectorToPosition(Vector2 pos)
        {
            Vector2 temp = new Vector2((float)(pos.X - this.CenterX), (float)(pos.Y - this.CenterY));
            temp.Normalize();
            return temp;
        }


    }
}
