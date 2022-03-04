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
     


    }
}
