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

        public Player(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            speed = 8;
            bulletCount = 15;
            //temp hp
            hp = 4;
            invTime = 3;
            invTimer = 0;
        }

        public int BulletCount
        {
            get { return bulletCount; }
            set { bulletCount = value; }
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

        public void TakeDamage(int damage)
        {
            if(invTimer <= 0)
            {
                hp -= damage;
                invTimer = invTime;
            }
            
        }

        public void Update(GameTime gameTime)
        {
            invTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            Move();
        }

        /// <summary>
        /// ToDo: Keyboard input
        /// </summary>
        public void Move()
        {
            KeyboardState keypress = Keyboard.GetState();

            // speed at which player moves is subject to change
            if (keypress.IsKeyDown(Keys.W))
            {
                rectangle.Y -= speed;
            }
            if (keypress.IsKeyDown(Keys.A))
            {
                rectangle.X -= speed;
            }
            if (keypress.IsKeyDown(Keys.S))
            {
                rectangle.Y += speed;
            }
            if (keypress.IsKeyDown(Keys.D))
            {
                rectangle.X += speed;
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
            //foreach ()
            {
                if (CollideWith(other))
                {
                    bulletCount += 1;
                }
            }
        }
    }
}
