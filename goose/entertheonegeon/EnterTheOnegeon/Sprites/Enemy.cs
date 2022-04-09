using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    public delegate void IncreaseScore(int score);
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

        /// <summary>
        /// Speed of the enemy
        /// </summary>
        protected double speed;

        /// <summary>
        /// Contact damage of enemy
        /// </summary>
        protected int damage;

        /// <summary>
        /// Used for showing hit feedback, 
        /// </summary>
        protected double hitTimer;
        
        /// <summary>
        /// The velocity of the enemy
        /// </summary>
        protected Vector2 velocity;

        public event IncreaseScore OnDeathScore;

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
            speed = 0;
            damage = 0;
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
        
        /// <summary>
        /// Taking damage, but also the ondeathscore event activates when it dies
        /// </summary>
        public virtual void TakeDamage(int damage)
        {
            hitTimer = 0.3;
            health -= damage;
            if (health <= 0)
            {
                if (OnDeathScore != null)
                    OnDeathScore(100);
            }
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
            if(this.Active)
            {
                if (hitTimer > 0)
                {
                    sb.Draw(sprite, rectangle, Color.Black);
                }
                else
                {
                    base.Draw(sb);
                }
            }
        }

        public virtual void HitPlayer(Player player)
        {
            TakeDamage(1);
            player.TakeDamage(damage);
        }
    }
}
