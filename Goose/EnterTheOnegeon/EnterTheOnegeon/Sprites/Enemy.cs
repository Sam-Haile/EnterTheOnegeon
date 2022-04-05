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
            set { health = value; }
        }

        public Enemy(Texture2D sprite, Rectangle rectangle, int health) : base(sprite, rectangle)
        {
            this.health = health;
        }


        public virtual bool Active
        {
            get
            {
                return health > 0;
            }
        }

        public virtual void TakeDamage(int damage)
        {
            health -= damage;
        }

        public virtual void HitPlayer(Player player)
        {
            TakeDamage(1);
            player.TakeDamage(1);
        }
    }
}
