using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon.Sprites
{
    public delegate void OnDeathUpgrade(int s, int h, BulletStats bs);
    class UpgradeEnemy : Enemy
    {
        //Stats of upgrades
        private int speed;
        private int hp;
        private BulletStats bStats;

        public event OnDeathUpgrade OnDeath;

        /// <summary>
        /// Creates an inactive upgrade enemy
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="rectangle"></param>
        public UpgradeEnemy(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle, 0)
        {
            speed = 0;
            hp = 0;
            bStats = new BulletStats(0,0,0,0);
        }

        public override bool CollideWith(GameObject other)
        {
            if (!this.Active)
                return false;
            return base.CollideWith(other);
        }
        /// <summary>
        /// Overriding the Take damage to apply the OnDeath event
        /// </summary>
        public override void TakeDamage(int damage)
        {
            if(this.Active)
            {
                health -= damage;
                if (health < 0)
                {
                    if (OnDeath != null)
                        OnDeath(speed, hp, bStats);
                }
            }
        }

        /// <summary>
        /// For now it will draw the stats of upgrade
        /// And it does not currently use the texture2D
        /// </summary>
        public override void Draw(SpriteBatch sb)
        {
            if(this.Active)
            {
                Texture2D tempTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
                tempTexture.SetData(new Color[] { Color.White });
                sb.Draw(tempTexture, this.rectangle, Color.Blue);
            }
        }

        /// <summary>
        /// Resets(spawns) an upgrade enemy
        /// </summary>
        /// <param name="health">Actual health of it</param>
        /// <param name="hp">Hp up points</param>
        /// <param name="spd">Spd up point</param>
        /// <param name="bStats">Bullet stats points</param>
        public void Reset(int health, int hp, int spd, BulletStats bStats)
        {
            this.Health = health;
            this.hp = hp;
            speed = spd;
            this.bStats = bStats;
        }
    }
}
