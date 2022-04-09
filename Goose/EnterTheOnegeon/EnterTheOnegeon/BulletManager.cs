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

        private double tempTimer;
        //private Texture2D bulletAsset;

        protected Camera camera = new Camera();

        public BulletManager(Texture2D bulletAsset)
        {
            tempTimer = 0;
            //this.bulletAsset = bulletAsset;
            pBullets = new List<Bullet>();
            //Adding inactive bullets, this is the cap on the amount of bullets on screen
            for (int i = 0; i < 5; i++)
            {
                pBullets.Add(new Bullet(bulletAsset, new Rectangle(0, 0, 1, 1)));
            }

            eBullets = new List<Bullet>();
            for (int i = 0; i < 10; i++)
            {
                eBullets.Add(new Bullet(bulletAsset, new Rectangle(0, 0, 1, 1)));
            }
        }

        /// <summary>
        /// Handles player bullet creation, and the updating of player and enemy bullets
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="mState"></param>
        /// <param name="prevMState"></param>
        /// <param name="player"></param>
        /// <param name="eManager"></param>
        public void Update(GameTime gameTime, MouseState mState, MouseState prevMState, Player player, EnemyManager eManager)
        {
            camera.Follow(player);

            //Creating player bullets when clicking
            if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released && player.BulletCount > 0)
            {
                if(GetPlayerBullet() != null)
                {
                    GetPlayerBullet().Reset(
                            player.CenterX,
                            player.CenterY,
                        new Vector2(
                            mState.X - camera.Transform.Translation.X,
                            mState.Y - camera.Transform.Translation.Y),
                        player.BStats);
                    player.BulletCount--;
                }
            }
            /*TEMPORARY SPAWNING OF ENEMY BULLETS AT THE TOP LEFT OF DUNGEON
             * 
            tempTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(tempTimer > 0.5)
            {
                tempTimer = 0;
                if (GetEnemyBullet() != null)
                {
                    GetEnemyBullet().Reset(
                            300,
                            300,
                        new Vector2(301,300),
                        new BulletStats(20, 4, 1, 1));
                }
            }
             */

            //Don't need to remove inactive bullets
            #region Updating the player bullets
            foreach (Bullet b in pBullets)
            {
                b.Update(gameTime);
                foreach (TestEnemy en in eManager.GetTestEnemies)
                {
                    if (b.CollideWith(en))
                    {
                        b.HitEnemy(en);
                        player.BulletCount++;
                    }
                }
                foreach(WalkEnemy walke in eManager.GetWalkEnemies)
                {
                    if(b.CollideWith(walke))
                    {
                        b.HitEnemy(walke);
                        player.BulletCount++;
                    }
                }
                //Added upgrade enemies
                foreach (UpgradeEnemy en in eManager.UpgradeEnemies)
                {
                    if (b.CollideWith(en))
                    {
                        b.HitEnemy(en);
                    }
                }
            }
            #endregion
            #region Updating the enemy bullets
            foreach (Bullet b in eBullets)
            {
                b.Update(gameTime);
                if (b.CollideWith(player))
                {
                    b.HitPlayer(player);
                }
            }
            #endregion
        }

        /// <summary>
        /// Draws each active bullet in pBullets and eBullets
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb/*, SpriteFont font*/)
        {
            foreach (Bullet b in pBullets)
                b.Draw(sb);
            foreach (Bullet b in eBullets)
                b.Draw(sb);
        }

        /// <summary>
        /// Helping method to get the first inactive player bullet
        /// </summary>
        /// <returns>First inactive bullet in pBullets, or null of there are no inactive 
        /// bullets</returns>
        public Bullet GetPlayerBullet()
        {
            for(int i = 0; i < pBullets.Count; i++)
            {
                if (pBullets[i].Active == false)
                    return pBullets[i];
            }
            return null;
        }

        /// <summary>
        /// Helping method to get the first inactive enemy bullet
        /// </summary>
        /// <returns>First inactive bullet in eBullets, or null of there are no inactive 
        /// bullets</returns>
        public Bullet GetEnemyBullet()
        {
            for (int i = 0; i < eBullets.Count; i++)
            {
                if (eBullets[i].Active == false)
                    return eBullets[i];
            }
            return null;
        }
    }
}
