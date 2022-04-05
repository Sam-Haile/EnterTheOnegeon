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

        protected int maxHealth;

        //Invincibility frames
        protected double invTime;
        protected double invTimer;

        /// <summary>
        /// Actual X value of the enemy
        /// </summary>
        protected float posX;

        /// <summary>
        /// Actual Y value of the enemy
        /// </summary>
        protected float posY;


        /// <summary>
        /// returns enemies health
        /// </summary>
        public int Health 
        {
            get { return health; }
            set { health = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public Enemy(Texture2D sprite, Rectangle rectangle, int health) : base(sprite, rectangle)
        {
            this.health = health;
            maxHealth = health;
            invTime = 0.5;
            invTimer = 0;
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
            if(invTimer <= 0)
            {
                health -= damage;
            }
        }
        public void Update(GameTime gameTime)
        {
            if(this.Active)
            {
                if (invTimer > 0)
                {
                    invTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public virtual void HitPlayer(Player player)
        {
            TakeDamage(1);
            player.TakeDamage(1);
        }
    }
}
