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
        private int widthOfSingleSprite;
        private int heightOfSingleSprite;
        public event OnDeathUpgrade OnDeathUpgrade;
        private Texture2D enemySS;
        // Animation reqs
        int currentFrame;
        double fps;
        double secondsPerFrame;
        double timeCounter;

        /// <summary>
        /// Creates an inactive upgrade enemy
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="rectangle"></param>
        public UpgradeEnemy(Texture2D sprite, Rectangle rectangle,Random rng) : base(sprite, rectangle, 0)
        {
            enemySS = sprite;
            speed = 0;
            hp = 0;
            bStats = new BulletStats(0,0,0,0);
            widthOfSingleSprite = 111;
            heightOfSingleSprite = 120;
            currentFrame = rng.Next(4);
            fps = 3;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
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
                DrawUEnemy(sb, Color.White);
                sb.DrawString(font, string.Format("x{0}", health), new Vector2(CenterX-20, CenterY-10), Color.Orange);
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

        /// <summary>
        /// Updates the animation time
        /// </summary>
        /// <param name="gameTime">Game time information</param>
        public void UpdateAnimation(GameTime gameTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= 3) currentFrame = 0;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }
        }

        private void DrawUEnemy( SpriteBatch sb, Color color)
        {

            sb.Draw(
                enemySS,
                new Vector2(this.rectangle.X,this.rectangle.Y),
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, heightOfSingleSprite),
                color,
                0.0f,
                Vector2.Zero,
                1.2f,
                SpriteEffects.None,
                0.0f);
        }
    }
}
