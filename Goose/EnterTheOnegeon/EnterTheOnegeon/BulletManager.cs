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

        public void Update(GameTime gameTime, MouseState mState, MouseState prevMState, Player player, EnemyManager eManager)
        {
            #region "Creating" bullets on each click
            //Creating player bullets when clicking
            if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released && player.BulletCount > 0)
            {
                GetPlayerBullet().Reset(
                            player.CenterX - player.BStats.Size / 2,
                            player.CenterY - player.BStats.Size / 2,
                        new Vector2(
                            mState.X - camera.Transform.Translation.X,
                            mState.Y - camera.Transform.Translation.Y),
                        player.BStats);
                player.BulletCount--;
            }
            #endregion
            #region Updating the player bullets
            foreach (Bullet b in pBullets)
            {
                b.Update(gameTime);
                foreach (TestEnemy en in eManager.GetTestEnemies())
                {
                    if (b.CollideWith(en))
                    {
                        b.HitEnemy(en);
                    }
                }
            }
            #endregion
        }

        public void Draw(SpriteBatch sb/*, SpriteFont font*/)
        {
            foreach (Bullet b in pBullets)
            {
                b.Draw(sb);
            }
        }

        //Helping method to get the first inactive player bullet
        //Returns null if none are inactive
        public Bullet GetPlayerBullet()
        {
            for(int i = 0; i < pBullets.Count; i++)
            {
                if (pBullets[i].Active == false)
                    return pBullets[i];
            }
            return null;
        }
    }
}
