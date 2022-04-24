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
        public BulletStats ShooterBullets = new BulletStats(10, 5, 1, 1);
        private double tempTimer;
        //private Texture2D bulletAsset;

        public bool shooting;

        public bool Shooting 
        { get { return this.shooting; }
          set { this.shooting = value; } }

        protected Camera camera = new Camera();

        public BulletManager(Texture2D bulletAsset)
        {
            tempTimer = 0;
            //this.bulletAsset = bulletAsset;
            pBullets = new List<Bullet>();
            //Adding inactive bullets, this is the cap on the amount of bullets on screen
            for (int i = 0; i < 15; i++)
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
        public void Update(GameTime gameTime, MouseState mState, MouseState prevMState, Player player, EnemyManager eManager, WalkState walkState)
        {
            //Creating player bullets when clicking
            if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released && player.BulletCount > 0)
            {
                if(GetPlayerBullet() != null)
                {
                    switch (walkState)
                    {
                        case WalkState.WalkLeft:
                        case WalkState.FaceLeft:
                            GetPlayerBullet().Reset(
                                player.CenterX - 15,
                                player.CenterY + 10,
                                new Vector2(
                                    mState.X - camera.Transform.Translation.X,
                                    mState.Y - camera.Transform.Translation.Y),
                                player.BStats);
                            player.BulletCount--;
                            break;
                        case WalkState.WalkRight:
                        case WalkState.FaceRight:
                            GetPlayerBullet().Reset(
                                player.CenterX + 40,
                                player.CenterY + 10,
                                new Vector2(
                                    mState.X - camera.Transform.Translation.X,
                                    mState.Y - camera.Transform.Translation.Y),
                                player.BStats);
                            player.BulletCount--;
                            break;
                        case WalkState.WalkUp:
                        case WalkState.FaceUp:
                            GetPlayerBullet().Reset(
                                player.CenterX + 12,
                                player.CenterY - 15,
                                new Vector2(
                                    mState.X - camera.Transform.Translation.X,
                                    mState.Y - camera.Transform.Translation.Y),
                                player.BStats);
                            player.BulletCount--;
                            break;
                        case WalkState.WalkDown:
                        case WalkState.FaceDown:
                            GetPlayerBullet().Reset(
                                player.CenterX + 12,
                                player.CenterY + 15,
                                new Vector2(
                                    mState.X - camera.Transform.Translation.X,
                                    mState.Y - camera.Transform.Translation.Y),
                                player.BStats);
                            player.BulletCount--;
                            break;
                    }
                    shooting = true;
                }

                camera.Follow(player);
            }
            else if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released && player.BulletCount == 0)
            {
                shooting = true;
            }
            
            foreach(ShootingEnemy sho in eManager.GetShooters())
            {
                if(sho.Active)
                {
                    if (sho.IsShooting)
                        SpawnEnemyBullet(sho.CenterX, sho.CenterY, player.PositionV, ShooterBullets);
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
                /*foreach (TestEnemy en in eManager.GetTestEnemies)
                {
                    if (b.CollideWith(en))
                    {
                        b.HitEnemy(en);
                        player.BulletCount++;
                    }
                }*/
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

        public void SpawnEnemyBullet(int x, int y, Vector2 posi, BulletStats bStats)
        {
            if(GetEnemyBullet() != null)
            {
                GetEnemyBullet().Reset(x, y, posi, bStats);
            }
        }
    }
}
