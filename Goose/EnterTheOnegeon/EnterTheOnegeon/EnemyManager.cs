﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    
    public enum EManagerState
    {
        Waves,
        Shop
    }
    /// <summary>
    /// This class handles all the enemies
    /// Also has the score and timer
    /// </summary>
    class EnemyManager
    {
        //fields
        private Random rng;
        private GraphicsDeviceManager graphics;
        private double timer;
        private double waveTime;
        private int wavePoints;
        private int score;
        protected Camera camera = new Camera();

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

        public int Score
        {
            get { return score; }
        }
        public double Time
        {
            get { return timer; }
        }
        public List<TestEnemy> GetTestEnemies()
        {
            return testEnemyList;
        }
        /// <summary>
        /// Handles updating all the enemies and the spawning of them
        /// </summary>
        public void Update(GameTime gameTime, Player player)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            waveTime -= gameTime.ElapsedGameTime.TotalSeconds;
            
            //Every time a wave is spawned
            //Resets the countdown to next wave
            //Spawns an amount of enemies according to the amount of wave points availible
            //Might move this to a method of some sort
            //UpdateWave();
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
                        //10% chance to spawn wide dude
                        //Change LATER for different enemy types
                        if(rng.Next(10) == 0)
                        {
                            SpawnWideBoi(RandPoint(true));
                            curWavePoints -= 3;
                        }
                        else
                        {
                            SpawnTestEnemy(RandPoint(false));
                            curWavePoints -= 1;
                        }
                    }
                    else
                    {
                        SpawnTestEnemy(RandPoint(false));
                        curWavePoints -= 1;
                    }
                }
                //Every wave gets harder
                wavePoints += 1;
            }
            UpdateTestEnemy(player);
            camera.Follow(player);

        }
        //public void DebugUpdate()

        /// <summary>
        /// Updates all the TestEnemies
        /// </summary>
        private void UpdateTestEnemy(Player player)
        {
            //Updating each enemy and checking the collision
            //As well as making sure they don't overlap
            for (int i = 0; i < testEnemyList.Count; i++)
            {
                testEnemyList[i].Update(player);
                if (testEnemyList[i].CollideWith(player))
                    testEnemyList[i].HitPlayer(player);
                //Checking each enemy with each other
                for (int j = i+1; j < testEnemyList.Count; j++)
                {
                    if (testEnemyList[i].CollideWith(testEnemyList[j]))
                        testEnemyList[i].MoveAwayFrom(testEnemyList[j]);
                }
            }
            //Removing the inactive enemies
            for (int i = testEnemyList.Count - 1; i >= 0; i--)
            {
                if (!testEnemyList[i].Active)
                {
                    testEnemyList.RemoveAt(i);
                    //Stuff that happens when an enemy dies
                    score += 100;
                    player.BulletCount++;
                }

            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            foreach (TestEnemy en in testEnemyList)
            {
                en.Draw(sb);
            }
            //Score
            sb.DrawString(
                font,
                String.Format("Score: {0}", score),
                new Vector2(
                    -(int)camera.Transform.Translation.X + 1500,
                    -(int)camera.Transform.Translation.Y + 70),
                Color.White);
            //Time to next wave
            sb.DrawString(
                font,
                String.Format("Next wave: {0:F3}", waveTime),
                new Vector2(
                    -(int)camera.Transform.Translation.X + 800,
                    -(int)camera.Transform.Translation.Y + 100),
                Color.White);
            //Total time in top right
            sb.DrawString(
                font,
                String.Format("Total Time: {0:F3}", timer),
                new Vector2(
                    -(int)camera.Transform.Translation.X + 1500,
                    -(int)camera.Transform.Translation.Y + 100),
                Color.White);

        }

        public void DebugDraw(SpriteBatch sb, SpriteFont font)
        {
            Draw(sb, font);
        }
        
        /// <summary>
        /// Spawns a test enemy at the given point
        /// </summary>
        /// <param name="pos">position to spawn the enemy</param>
        public void SpawnTestEnemy(Point pos)
        {
            testEnemyList.Add(
                    new TestEnemy(
                        testEnemyAsset,
                        new Rectangle(
                            pos,
                            new Point(50,50)),
                        1)); //Health
        }

        //Delete this later
        public void SpawnWideBoi(Point pos)
        {
            testEnemyList.Add(
                    new TestEnemy(
                        testEnemyAsset,
                        new Rectangle(
                            pos,
                            new Point(150, 100)),
                        3)); //Health
        }

        // Gets a random point off screen
        public Point RandPoint(bool isWideBoi)
        {
            int randX;
            int randY;
            // 50/50 to decide to change x or y offscreen
            //enemy comes in from left or right
            if (rng.Next(2) > 0)
            {
                randY = rng.Next(0 - testEnemyAsset.Height, graphics.GraphicsDevice.Viewport.Height);
                randX = rng.Next(2);
                // enemy spawns on the east wall
                if (randX == 1)
                {
                    if (isWideBoi)
                    {
                        // dungeon.png width (scaled x2) - east wall thickness (scaled 2x) - wideboi width
                        randX = 1920 * 2 - 96 * 2 - 150;
                    }
                    else
                    {
                        // dungeon.png width (scaled x2) - east wall thickness (scaled 2x) - enemy width
                        randX = 1920 * 2 - 96 * 2 - 50;
                    }
                }
                // enemy spawns west of viewport
                else
                {
                    // west wall width (scaled x2)
                    randX = 0 + 96*2;
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
                    if (isWideBoi)
                    {
                        // dungeon.png height (scaled x2) - south wall thickness (scaled 2x) - wideboi height
                        randY = 1088 * 2 - 32 * 2 - 100;
                    }
                    else
                    {
                        // dungeon.png height (scaled x2) - south wall thickness (scaled 2x) - enemy height
                        randY = 1088 * 2 - 32 * 2 - 50;
                    }
                }
                // enemy spawns north of viewport
                else
                {
                    // north wall height (scaled x2)
                    randY = 0 + 96*2;
                }
            }
            return new Point(randX, randY);
        }
    }
}