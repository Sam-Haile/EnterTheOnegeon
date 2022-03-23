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

        //Have a separate list for each type of enemy
        private List<TestEnemy> testEnemyList;
        //private List<Enemy2> enemy2List;

        /// <summary>
        /// Constructor will initialize the lists for every enemy type(only test enemies for now) 
        /// </summary>
        public EnemyManager(GraphicsDeviceManager graphics, Texture2D testSprite)
        {
            this.graphics = graphics;
            testEnemyList = new List<TestEnemy>();
            for(int i = 0; i < 30; i++)
            {
                testEnemyList.Add(new TestEnemy(testSprite, new Rectangle(), 0));
            }
            rng = new Random();
        }

        // Gets a rng point off screen
        public Point RandPoint()
        {
            int randX;
            int randY;
            // 50/50 to decide to change x or y offscreen
            //enemy comes in from left or right
            if (rng.Next(2) > 0)
            {
                randY = rng.Next(-200, graphics.GraphicsDevice.Viewport.Height);
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
                randX = rng.Next(-200, graphics.GraphicsDevice.Viewport.Width);
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
