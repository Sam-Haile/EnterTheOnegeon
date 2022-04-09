using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    public delegate void OnDeathUpgrade(int s, int h, BulletStats bs);
    class UpgradeEnemy : Enemy
    {
        //Stats of upgrades
        private new int speed;
        private int hp;
        private BulletStats bStats;

        public event OnDeathUpgrade OnDeathUpgrade;

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

        /* Added Active to gameobject so this does nothing
        public override bool CollideWith(GameObject other)
        {
            if (!this.Active)
                return false;
            return base.CollideWith(other);
        }*/

        /// <summary>
        /// Overriding the Take damage to apply the OnDeathUpgrade event
        /// and also not applying OnDeathScore event
        /// </summary>
        public override void TakeDamage(int damage)
        {
            if(this.Active)
            {
                health -= damage;
                if (health <= 0)
                {
                    if (OnDeathUpgrade != null)
                        OnDeathUpgrade(speed, hp, bStats);
                }
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            if (this.Active)
            {
                Texture2D tempTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
                tempTexture.SetData(new Color[] { Color.White });
                sb.Draw(tempTexture, this.rectangle, Color.Aqua);
                sb.DrawString(font, string.Format("x{0}", health), new Vector2(CenterX-20, CenterY-10), Color.Black);
                sb.DrawString(font, bStats.ToString(), new Vector2(X, CenterY+ 20), Color.Black);
                sb.DrawString(font, string.Format("{0}, {1}", hp, speed), new Vector2(X, CenterY - 40), Color.Black);
            }
        }

        /// <summary>
        /// Resets(spawns) an upgrade enemy
        /// </summary>
        /// <param name="health">Actual health of it</param>
        /// <param name="hp">Max hp up points</param>
        /// <param name="spd">Spd up point</param>
        /// <param name="bStats">Bullet stats points</param>
        public void Reset(int x, int y, int health, int hp, int spd, BulletStats bStats)
        {
            this.rectangle.X = x;
            this.rectangle.Y = y;
            this.Health = health;
            this.hp = hp;
            speed = spd;
            this.bStats = bStats;
        }
    }
}
