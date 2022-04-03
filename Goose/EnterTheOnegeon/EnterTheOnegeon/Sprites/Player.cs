using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    /// <summary>
    /// The player controlled character. Dies when health < 1
    /// </summary>
    class Player : GameObject
    {
        private int speed;
        private int bulletCount;
        //temp hp
        private int hp;

        //Invincibility frames
        private double invTime;
        private double invTimer;

        //The stats of the bullet the player will shoot
        private BulletStats bStats;

        public Player(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            speed = 6;
            bulletCount = 15;
            //temp hp
            hp = 4;
            invTime = 3;
            invTimer = 0;

            //Size, Speed, Num of Passes, Damage(not used yet)
            bStats = new BulletStats(10, 5, 1, 1);
        }

        public int BulletCount
        {
            get { return bulletCount; }
            set { bulletCount = value; }
        }

        public int Health
        {
            get { return hp; }
            set { hp = value; }
        }

        public bool Active
        {
            get { return hp > 0; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public double IFrameTimeLeft
        {
            get { return invTimer; }
        }

        public BulletStats BStats
        {
            get { return bStats; }
            set { bStats = value; }
        }

        public void TakeDamage(int damage)
        {
            if(invTimer <= 0)
            {
                hp -= damage;
                invTimer = invTime;
            }
            
        }

        public void Update(GameTime gameTime, MouseState mState, MouseState prevMState, KeyboardState kState, KeyboardState prevKState)
        {
            invTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            Move(kState);
            if (mState.RightButton == ButtonState.Pressed)
            {
                //Parry();
            }
        }

        /// <summary>
        /// ToDo: Keyboard input
        /// </summary>
        public void Move(KeyboardState kState)
        {
            // speed at which player moves is subject to change
            if (kState.IsKeyDown(Keys.W))
            {
                rectangle.Y -= speed;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                rectangle.X -= speed;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                rectangle.Y += speed;
            }
            if (kState.IsKeyDown(Keys.D))
            {
                rectangle.X += speed;
            }

            // Keeps player inside of the map
            if (rectangle.Y < 0 + 96 * 2) // North
            {
                rectangle.Y = 96 * 2;
            }
            else if (rectangle.Y > 2176 - 32 * 2 - 64) // South
            {
                rectangle.Y = 2176 - 32 * 2 - 64;
            }
            if (rectangle.X < 0 + 96 * 2) // West
            {
                rectangle.X = 96 * 2;
            }
            else if(rectangle.X > 3840 - 96 * 2 - 32) // East
            {
                rectangle.X = 3840 - 96 * 2 - 32;
            }
        }

        //Overriding draw to draw a hp bar as well
        public override void Draw(SpriteBatch sb)
        {
            //When invincible he is red
            if(invTimer > 0)
            {
                sb.Draw(sprite, rectangle, Color.Red);
            }
            //not invincible
            else
            {
                sb.Draw(sprite, rectangle, Color.White);
            }

            //Hp bar here
            Texture2D tempTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
            tempTexture.SetData(new Color[] { Color.White });
            sb.Draw(tempTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 5), Color.Red);
            sb.Draw(tempTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, (int)(rectangle.Width * (double)hp/4), 5), Color.LimeGreen);
        }

        public void Parry(GameObject other)
        {
            // if player is colliding with any TestEnemy
            // foreach ()
            {
                //if (CollideWith(b))
                {
                    bulletCount += 1;
                }
            }
        }
    }
}
