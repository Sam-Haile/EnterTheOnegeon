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

        public Player(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            speed = 10;
            bulletCount = 5;
            //temp hp
            hp = 4;
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

        public void TakeDamage(int damage)
        {
            hp -= damage;
        }

        public void Update()
        {
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
            sb.Draw(sprite, rectangle, Color.White);
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
