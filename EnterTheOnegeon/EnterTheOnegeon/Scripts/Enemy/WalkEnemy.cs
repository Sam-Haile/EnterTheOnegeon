using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    /// <summary>
    /// Simple enemy that only walks towards the player
    /// </summary>
    class WalkEnemy : Enemy
    {
        private Vector2 playerPos;
        // Animation reqs
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;
        private int widthOfSingleSprite;
        private int heightOfSingleSprite;
        private int damage;
        
        
        /// <summary>
        /// 
        /// </summary>
        public int Damage
        {
            get { return damage; }
        }
        
        /// <summary>
        /// Makes an inactive one
        /// </summary>
        /// <param name="sprite">Sprite</param>
        public WalkEnemy(Texture2D sprite) : base(sprite, new Rectangle(), 0)
        {
            speed = 1;
            playerPos = new Vector2();
            widthOfSingleSprite = 69;
            heightOfSingleSprite = 69;
            currentFrame = 0;
            fps = 8;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
        }

        //Test purposes
        public WalkEnemy(Texture2D sprite, Rectangle rectangle, int health, int spd) : base(sprite, rectangle, health)
        {
            speed = spd;
            playerPos = new Vector2();
            widthOfSingleSprite = 69;
            heightOfSingleSprite = 69;
            currentFrame = 0;
            fps = 8;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
        }

        /// <summary>
        /// Updates the velocity based on where the player is
        /// </summary>
        private void UpdateVelocity()
        {
            velocity = this.VectorToPosition(playerPos);
            if (velocity.Length() != 0)
            {
                velocity.Normalize();
            }
            velocity.X *= (float)speed;
            velocity.Y *= (float)speed;
        }

        /// <summary>
        /// Changes player position, calls update velocity, and applies the velocity
        /// </summary>
        public void Update(Player p, GameTime gameTime)
        {
            if (this.Active)
            {
                playerPos = p.PositionV;
                UpdateVelocity();
                actualX += velocity.X;
                actualY += velocity.Y;

                //Need to adjust rectangle 
                base.UpdateRectanglePos();
                UpdateAnimation(gameTime);

                //Also update the hitTime
                base.Update(gameTime);

            }
           
        }

        /// <summary>
        /// Spawns the enemy at the given center point
        /// </summary>
        /// <param name="spawnPos"></param>
        /// <param name="enemyStats"></param>
        public void Reset(Point spawnPos, EnemyStats enemyStats)
        {
            hitTimer = 0;
            rectangle.Width = enemyStats.Width;
            rectangle.Height = enemyStats.Height;
            actualX = spawnPos.X;
            actualY = spawnPos.Y;
            speed = enemyStats.Speed;
            health = enemyStats.Health;
            maxHealth = enemyStats.Health;
            damage = enemyStats.Damage;
        }
        //Also changes the sprite
        public void Reset(Texture2D sprite, Point spawnPos, EnemyStats enemyStats)
        {
            this.Reset(spawnPos, enemyStats);
            this.sprite = sprite;
        }

        public void DrawWalkEnemy(SpriteBatch sb, Color color)
        {
            if(this.Health != 0)
            {
                sb.Draw(
               sprite,
               new Vector2(this.rectangle.X, this.rectangle.Y),
               new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, heightOfSingleSprite),
               color,
               0.0f,
               Vector2.Zero,
               1f,
               SpriteEffects.None,
               0.0f);
            }
            else
            {
                sb.Draw(
               sprite,
               new Vector2(this.rectangle.X, this.rectangle.Y),
               new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, heightOfSingleSprite),
               Color.Red,
               0.0f,
               Vector2.Zero,
               1f,
               SpriteEffects.None,
               0.0f);
            }
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= 7) currentFrame = 0;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }
        }
    }
}
