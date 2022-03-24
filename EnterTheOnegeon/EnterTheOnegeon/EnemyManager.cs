using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class EnemyManager
    {
        //fields
        private Random rng;
        private GraphicsDeviceManager graphics;
        private double timer;
        private double waveTime;
        private int wavePoints;

        //Have a separate list for each type of enemy
        private List<TestEnemy> testEnemyList;
        private Texture2D testEnemyAsset;
        //private List<Enemy2> enemy2List;

        /// <summary>
        /// Constructor will initialize the lists for every enemy type(only test enemies for now) 
        /// </summary>
        public EnemyManager(GraphicsDeviceManager graphics, Texture2D testSprite)
        {
            this.graphics = graphics;
            rng = new Random();
            timer = 0;
            waveTime = 5;
            wavePoints = 5;
            testEnemyList = new List<TestEnemy>();
            testEnemyAsset = testSprite;
            /*for(int i = 0; i < 30; i++)
            {
                testEnemyList.Add(new TestEnemy(testSprite, new Rectangle(), 0));
            }*/
        }

        public int TotalEnemyCount
        {
            get { return testEnemyList.Count /*+ enemy2List.Count etc*/; }
        }

        public double Time
        {
            get { return timer; }
        }
        public List<TestEnemy> GetTestEnemies()
        {
            return testEnemyList;
        }
        public void Update(GameTime gameTime, Player player)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            waveTime -= gameTime.ElapsedGameTime.TotalSeconds;
            
            //Every time a wave is spawned
            //Resets the countdown to next wave
            //Spawns an amount of enemies according to the amount of wave points availible
            if(waveTime <= 0)
            {
                if (timer < 20)
                    waveTime = 5;
                else if (timer < 40)
                    waveTime = 4;
                else
                    waveTime = 3;
                //Spawn the amount of enemies using the amount of wave points availible
                int curWavePoints = wavePoints;
                while(curWavePoints > 0)
                {
                    //when there is enough points it will do checks for spawning which type of enemy
                    if(curWavePoints >= 3)
                    {
                        //25% chance to spawn wide dude
                        if(rng.Next(4) == 0)
                        {
                            SpawnWideBoi(RandPoint());
                            curWavePoints -= 3;
                        }
                        else
                        {
                            SpawnTestEnemy(RandPoint());
                            curWavePoints -= 1;
                        }
                    }
                    else
                    {
                        SpawnTestEnemy(RandPoint());
                        curWavePoints -= 1;
                    }
                }
                //Every wave gets harder
                wavePoints += 1;
            }

            foreach (TestEnemy tEn in testEnemyList)
            {
                tEn.Update(player);
                if (tEn.CollideWith(player))
                    tEn.HitPlayer(player);
            }
            for (int i = testEnemyList.Count - 1; i >= 0; i--)
            {
                if (!testEnemyList[i].Active)
                {
                    testEnemyList.RemoveAt(i);
                }

            }
        }


        public void SpawnTestEnemy(Point pos)
        {
            testEnemyList.Add(
                    new TestEnemy(testEnemyAsset,
                        new Rectangle(
                            pos,
                            new Point(50,50)),
                        1)); //Health
        }
        //Delete this later
        public void SpawnWideBoi(Point pos)
        {
            testEnemyList.Add(
                    new TestEnemy(testEnemyAsset,
                        new Rectangle(
                            pos,
                            new Point(150, 100)),
                        3)); //Health
        }
        // Gets a random point off screen
        public Point RandPoint()
        {
            int randX;
            int randY;
            // 50/50 to decide to change x or y offscreen
            //enemy comes in from left or right
            if (rng.Next(2) > 0)
            {
                randY = rng.Next(0 - testEnemyAsset.Height, graphics.GraphicsDevice.Viewport.Height);
                randX = rng.Next(2);
                // enemy spawns east of viewport
                if (randX == 1)
                {
                    randX = graphics.GraphicsDevice.Viewport.Width;
                }
                // enemy spawns west of viewport
                else
                {
                    // 50 is the height of the enemy object, the height of the object !=
                    // the height of the sprite.
                    randX = 0 - 50;
                }
            }
            //enemy comes in from top or bottom
            else
            {
                randX = rng.Next(0 - testEnemyAsset.Width, graphics.GraphicsDevice.Viewport.Width);
                randY = rng.Next(2);
                // enemy spawns south of viewport 
                if (randY == 1)
                {
                    randY = graphics.GraphicsDevice.Viewport.Height;
                }
                // enemy spawns north of viewport
                else
                {
                    // 50 is the height of the enemy object, the height of the object !=
                    // the height of the sprite.
                    randY = 0 - 50;
                }
            }
            return new Point(randX, randY);
        }
    }
}
