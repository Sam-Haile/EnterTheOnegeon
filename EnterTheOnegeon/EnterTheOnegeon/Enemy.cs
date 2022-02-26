using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    abstract class Enemy : GameObject
    {

        protected int health;

        /// <summary>
        /// returns enemies health
        /// </summary>
        public int Health 
        {
            get { return health; }
        }

        public Enemy(int health, Texture2D sprite, Rectangle hitbox) : base(sprite, hitbox)
        {
            this.health = health;
        }


        /// <summary>
        /// determines whether enemy is dead
        /// </summary>
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
        /// displays enemy sprite
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, hitbox, Color.White);
        }
    }
}
