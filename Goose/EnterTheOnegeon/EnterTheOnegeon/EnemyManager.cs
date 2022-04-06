using System;
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
        Shop,
        WaveToShop
    }
    /// <summary>
    /// This class handles all the enemies
    /// Also has the score and timer
    /// </summary>
    class EnemyManager
    {
        /// <summary>
        /// The exit box for shop
        /// </summary>
        private Rectangle exitBox;
        //fields
        private Random rng;
        private GraphicsDeviceManager graphics;
        private double timer;

        private double timeToShop;
        private double ShopTime; //Prob need to rename the variable
        //To rename hightlight it and select "rename"

        private double timeToWave;
        private int wavePoints;

        private int score;
        protected Camera camera = new Camera();

        //Have a separate list for each type of enemy
        private List<TestEnemy> testEnemyList;
        private Texture2D testEnemyAsset;
        //private List<Enemy2> enemy2List;
        private List<UpgradeEnemy> upgradeEnemyList;

        private EManagerState currState;

        /// <summary>
        /// Constructor will initialize the lists for every enemy type(only test enemies for now) 
        /// </summary>
        public EnemyManager(GraphicsDeviceManager graphics, Texture2D testSprite, Player player)
        {
            exitBox = new Rectangle(3840- 400, 2176/2+50, 200, 200);
            currState = EManagerState.Waves;
            this.graphics = graphics;
            rng = new Random();
            timer = 0;
            ShopTime = 20;
            timeToShop = ShopTime;
            timeToWave = 5;
            wavePoints = 5;
            testEnemyList = new List<TestEnemy>();
            testEnemyAsset = testSprite;
            /*for(int i = 0; i < 30; i++)
            {
                testEnemyList.Add(new TestEnemy(testSprite, new Rectangle(), 0));
            }*/

            upgradeEnemyList = new List<UpgradeEnemy>();
            //fill with inactive ones
            //two for now
            for(int i = 0; i < 8; i++)
            {
                upgradeEnemyList.Add(new UpgradeEnemy(testSprite, new Rectangle(0, 0, 150, 150)));
                upgradeEnemyList[i].OnDeath += player.ApplyUpgrade;
            }
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
        public List<UpgradeEnemy> UpgradeEnemies
        {
            get { return upgradeEnemyList; }
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
            switch (currState)
            {
                case EManagerState.Waves:
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    timeToWave -= gameTime.ElapsedGameTime.TotalSeconds;
                    timeToShop -= gameTime.ElapsedGameTime.TotalSeconds;

                    //Every time a wave is spawned
                    //Resets the countdown to next wave
                    //Spawns an amount of enemies according to the amount of wave points availible
                    //Might move this to a method of some sort
                    //UpdateWave();
                    if (timeToWave <= 0)
                    {
                        if (timer < 20)
                            timeToWave = 5;
                        else if (timer < 40)
                            timeToWave = 4;
                        else
                            timeToWave = 3;
                        //Spawn the amount of enemies using the amount of wave points availible
                        int curWavePoints = wavePoints;
                        while (curWavePoints > 0)
                        {
                            //when there is enough points it will do checks for spawning which type of enemy
                            if (curWavePoints >= 3)
                            {
                                //10% chance to spawn wide dude
                                //Change LATER for different enemy types
                                if (rng.Next(10) == 0)
                                {
                                    SpawnGargoyle(RandPoint(true));
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
                    UpdateTestEnemy(player, gameTime);
                    //Transition the state and reset some variables
                    if (timeToShop < 0)
                    {
                        //Will be used as time to consectively stand on a box to left the shop
                        timeToShop = 0;
                        timeToWave = 5;
                        currState = EManagerState.WaveToShop;
                    }
                    break;
                //Transition from wave to shop
                //Wait for all enemies to die
                //NOT ADDED, but can add later to delete enemies if needed
                case EManagerState.WaveToShop:
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    UpdateTestEnemy(player, gameTime);
                    
                    if(TotalEnemyCount == 0)
                    {
                        timeToShop = 0;
                        timeToWave = 5;
                        currState = EManagerState.Shop;
                    }
                    break;
                case EManagerState.Shop:
                    //populating shop
                    //only two right now
                    for(int i = 0; i < upgradeEnemyList.Count; i++)
                    {
                        if(!upgradeEnemyList[i].Active)
                        {
                            //Planning to have one for each stat
                            //Upgrades hp
                            if(i == 0)
                            {
                                //position, cost, hp up, spd up, bullet
                                upgradeEnemyList[i].Reset(3840 / 2- 300, 2176 / 2- 300, 5, 1, 0, new BulletStats(0, 0, 0, 0));
                            }
                            //Upgrades spd
                            else if (i == 1)
                            {
                                //position, cost, hp up,spd up, bullet
                                upgradeEnemyList[i].Reset(3840 / 2 - 100, 2176 / 2 - 300, 5, 0, 1, new BulletStats(0, 0, 0, 0));
                            }
                            //Upgrades bullet size
                            else if (i == 2)
                            {
                                //position, cost, hp up,spd up, bullet
                                upgradeEnemyList[i].Reset(3840 / 2 - 800, 2176 / 2 + 300, 5, 0, 0, new BulletStats(10, 0, 0, 0));
                            }
                            //Upgrades bullet spd
                            else if (i == 3)
                            {
                                //position, cost, hp up,spd up, bullet
                                upgradeEnemyList[i].Reset(3840 / 2 - 600, 2176 / 2 + 300, 5, 0, 0, new BulletStats(0, 1, 0, 0));
                            }
                            //Upgrades bullet pierce
                            else if (i == 4)
                            {
                                //position, cost, hp up,spd up, bullet
                                upgradeEnemyList[i].Reset(3840 / 2 - 400, 2176 / 2 + 300, 
                                    10*player.BStats.Passes //COST INCREASES BY THE STAT
                                    , 0, 0, new BulletStats(0, 0, 1, 0));
                            }
                            //Upgrades bullet damage
                            else if (i == 5)
                            {
                                //position, cost, hp up,spd up, bullet
                                upgradeEnemyList[i].Reset(3840 / 2 - 200, 2176 / 2 + 300,
                                    10 * player.BStats.Damage
                                    , 0, 0, new BulletStats(0, 0, 0, 1));
                            }

                            else if(i == 100)
                            {
                                //position, cost, hp up,spd up, bullet
                                upgradeEnemyList[i].Reset(3840 / 2, 2176 / 2 +300, 7, 0, 0, new BulletStats(20, 7, 2, 0));
                            }
                        }
                    }
                    //going to reuse variable as timer variable
                    //Controlling the exit box logic here
                    //When the player was and is in the box
                    if (player.Position.Intersects(exitBox))
                    {
                        timeToShop += gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else if (timeToShop > 0)
                    {
                        timeToShop -= gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else if(timeToShop < 0)
                        timeToShop = 0;
                    //When the player has stood long enough on it
                    if (timeToShop > 3)
                    {
                        //actually reseting now
                        ShopTime += 10;
                        timeToShop = ShopTime;
                        timeToWave = 5;
                        //clear the shop enemies
                        foreach (UpgradeEnemy upEn in upgradeEnemyList)
                            upEn.Health = 0;
                        currState = EManagerState.Waves;
                    }
                    break;
            }
            camera.Follow(player);
        }
        //public void DebugUpdate()

        /// <summary>
        /// Updates all the TestEnemies
        /// </summary>
        private void UpdateTestEnemy(Player player, GameTime gameTime)
        {
            //Updating each enemy and checking the collision
            //As well as making sure they don't overlap
            for (int i = 0; i < testEnemyList.Count; i++)
            {
                testEnemyList[i].Update(player, gameTime);
                if (testEnemyList[i].CollideWith(player))
                    testEnemyList[i].HitPlayer(player);
                //Checking each enemy with each other
                for (int j = i + 1; j < testEnemyList.Count; j++)
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
                }

            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            switch(currState)
            {
                case EManagerState.Waves:
                    foreach (TestEnemy en in testEnemyList)
                    {
                        en.Draw(sb);
                    }
                    //Time to next wave
                    sb.DrawString(
                        font,
                        String.Format("Next wave: {0:F0}", timeToWave),
                        new Vector2(
                            -(int)camera.Transform.Translation.X + 820,
                            -(int)camera.Transform.Translation.Y + 70),
                        Color.White);
                    //Time until the "shop" (prob rename it) appears
                    sb.DrawString(
                        font,
                        String.Format("Shop in {0:F0}s", timeToShop),
                        new Vector2(
                            -(int)camera.Transform.Translation.X + 820,
                            -(int)camera.Transform.Translation.Y + 120),
                        Color.White);
                    break;
                case EManagerState.WaveToShop:
                    foreach (TestEnemy en in testEnemyList)
                    {
                        en.Draw(sb);
                    }
                    //Some message to clear the enemies
                    sb.DrawString(
                        font,
                        String.Format("{0} enemies remaining", this.TotalEnemyCount),
                        new Vector2(
                            -(int)camera.Transform.Translation.X + 820,
                            -(int)camera.Transform.Translation.Y + 120),
                        Color.White);
                    break;
                
                case EManagerState.Shop:
                    //drawing the upgrade guys
                    foreach (UpgradeEnemy upEn in upgradeEnemyList)
                    {
                        upEn.Draw(sb, font);
                    }
                    for(int i = 0; i < upgradeEnemyList.Count; i++)
                    {
                        if (i == 0)
                            sb.DrawString(font, "HP UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y), Color.Black);
                        if (i == 1)
                            sb.DrawString(font, "SPD UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y), Color.Black);
                        if (i == 2)
                            sb.DrawString(font, "BUL SIZE UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y), Color.Black);
                        if (i == 3)
                            sb.DrawString(font, "BUL SPD UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y), Color.Black);
                        if (i == 4)
                            sb.DrawString(font, "PIERCE UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y), Color.Black);
                        if (i == 5)
                            sb.DrawString(font, "DMG UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y), Color.Black);
                    }
                    //TODO: position these
                    /*
                    This one above
                    sb.DrawString(
                        font,
                        String.Format("Player Upgrades"),
                        new Vector2(0, 0),
                        Color.White);
                    This one below
                    sb.DrawString(
                        font,
                        String.Format("Bullet Upgrades"),
                        new Vector2(0, 0),
                        Color.White);
                    */
                    sb.DrawString(
                        font,
                        String.Format("Gain power by breaking"),
                        new Vector2(-(int)camera.Transform.Translation.X + 820,
                            -(int)camera.Transform.Translation.Y + 120),
                        Color.White);

                    //Some temporary exit text
                    sb.DrawString(
                        font,
                        String.Format("Exit --------------------------->"),
                        new Vector2(3840 / 2, 2176 / 2 + 100),
                        Color.White);
                    //Drawing the exit box
                    Texture2D tempTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
                    tempTexture.SetData(new Color[] { Color.White });
                    sb.Draw(tempTexture, exitBox, Color.Gray);
                    sb.Draw(tempTexture, new Rectangle(exitBox.X, exitBox.Y, (int)(exitBox.Width * (timeToShop / 3)), exitBox.Height), Color.DarkBlue);
                    sb.DrawString(
                        font,
                        String.Format("Stand here to exit"),
                        new Vector2(exitBox.X-50, exitBox.Y),
                        Color.White);

                    break;
            }
            //Score
            sb.DrawString(
                font,
                String.Format("Score: {0}", score),
                new Vector2(
                    -(int)camera.Transform.Translation.X + 1680,
                    -(int)camera.Transform.Translation.Y + 70),
                Color.White);
            //Total time in top right
            sb.DrawString(
                font,
                String.Format("Total Time: {0:F0}s", timer),
                new Vector2(
                    -(int)camera.Transform.Translation.X + 1603,
                    -(int)camera.Transform.Translation.Y + 120),
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
                            new Point(50, 50)),
                        1)); //Health
        }

        //Delete this later
        public void SpawnGargoyle(Point pos)
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
        public Point RandPoint(bool isGargoyle)
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
                    if (isGargoyle)
                    {
                        // dungeon.png width (scaled x2) - east wall thickness (scaled 2x) - Gargoyle width
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
                    randX = 0 + 96 * 2;
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
                    if (isGargoyle)
                    {
                        // dungeon.png height (scaled x2) - south wall thickness (scaled 2x) - Gargoyle height
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
                    randY = 0 + 96 * 2;
                }
            }
            return new Point(randX, randY);
        }
    }
}
