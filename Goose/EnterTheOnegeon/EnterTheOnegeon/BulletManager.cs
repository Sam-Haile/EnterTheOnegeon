using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class BulletManager
    {
        //fields
        /// <summary>Bullets from the player</summary>
        private List<Bullet> pBullets;
        /// <summary>Bullets from the enemies</summary>
        private List<Bullet> eBullets;

        //private Texture2D bulletAsset;
        protected Camera camera = new Camera();

        public BulletManager(Texture2D bulletAsset)
        {
            //this.bulletAsset = bulletAsset;
            //Adding inactive bullets, this is the cap on the amount of bullets on screen
            for (int i = 0; i < 20; i++)
            {
                pBullets.Add(new Bullet(bulletAsset, new Rectangle(0, 0, 1, 1)));
            }
            /*
            for (int i = 0; i < 10; i++)
            {
                eBullets.Add(new Bullet(bulletAsset, new Rectangle(0, 0, 1, 1)));
            }*/
        }

        public void Update(GameTime gameTime, Player player)
        {

        }
        public void Draw(SpriteBatch sb, SpriteFont font)
        {
        }

        //Helping method to get the first inactive bullet
    }
}
