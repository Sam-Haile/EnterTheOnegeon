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

        /*
        //Invincibility frames
        //Unused
        protected double invTime;*/
        /// <summary>
        /// Used for showing hit feedback, 
        /// </summary>
        protected double hitTimer;
        

        protected Vector2 velocity;

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
            velocity = new Vector2();
            //invTime = 0;
            hitTimer = 0;
        }


        public override bool Active
        {
            get
            {
                return health > 0;
            }
        }
        
        public virtual void TakeDamage(int damage)
        {
            hitTimer = 0.5;
            health -= damage;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (hitTimer > 0)
            {
                hitTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Overriding draw to make it have a different color when hit
        /// </summary>
        public override void Draw(SpriteBatch sb)
        {
            if(hitTimer > 0)
            {
                sb.Draw(sprite, rectangle, Color.LightYellow);
            }
            else
            {
                base.Draw(sb);
            }
        }

        public virtual void HitPlayer(Player player)
        {
            TakeDamage(1);
            player.TakeDamage(1);
        }
    }
}
