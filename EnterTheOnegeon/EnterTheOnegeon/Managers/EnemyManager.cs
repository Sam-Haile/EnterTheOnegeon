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
        WaveToShop//,
        //Boss
    }
    /// <summary> All the different enemies </summary>
    public enum EnemyTypes
    {
        Gargoyle,
        BigGargoyle,
        FastGuy,
        Shooter,
        Other
    }
    //USE THIS FOR SpawnEnemy(EnemyTypes enemyType)

    /// <summary>
    /// This class handles all the enemies
    /// Also has the score and timer
    /// </summary>
    class EnemyManager
    {
        //EnemyStats constructor takes width, height, health, speed(can be a double), (contact)damage
        #region Enemy stat constants
        private EnemyStats GargoyleStats = new EnemyStats(50, 50, 1, 3, 1);
        private EnemyStats BigGargoyleStats = new EnemyStats(150, 150, 5, 2, 2);
        private EnemyStats FastGuyStats = new EnemyStats(40, 60, 1, 7, 1);
        private EnemyStats ShooterStats = new EnemyStats(40, 60, 1, 3, 1);
        public BulletStats ShooterBullets = new BulletStats(20, 5, 1, 1);
        #endregion

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
        //private List<TestEnemy> testEnemyList;
        private List<WalkEnemy> walkEnemyList;
        private List<UpgradeEnemy> upgradeEnemyList;
        private List<ShootingEnemy> shootEnemyList;

        private Texture2D GargoyleSpriteSheet;
        private Texture2D GargoyleAsset;
        private Texture2D eyeballAsset;
        private Texture2D UpgradeSheet;

        private EManagerState currState;

        /// <summary>
        /// Constructor will initialize the lists for every enemy type(only test enemies for now) 
        /// </summary>
        public EnemyManager(GraphicsDeviceManager graphics, Player player, BulletManager bManager, 
            Texture2D testSprite, Texture2D GargoyleSprite, Texture2D GargoyleSpriteSheet, Texture2D upgradeSheet)
        {
            exitBox = new Rectangle(3840- 400, 2176/2+50, 200, 200);

            currState = EManagerState.WaveToShop;
            this.graphics = graphics;
            rng = new Random();
            timer = 0;
            score = 0;

            GargoyleAsset = GargoyleSprite;
            eyeballAsset = testSprite;
            UpgradeSheet = upgradeSheet;
            this.GargoyleSpriteSheet = GargoyleSpriteSheet;

            ShopTime = 20;
            timeToShop = ShopTime;
            timeToWave = 5;
            wavePoints = 5;


            //Fill every list with inactive ones
            /*testEnemyList = new List<TestEnemy>();
            for(int i = 0; i < 0; i++)
            {
                testEnemyList.Add(new TestEnemy(testSprite, new Rectangle(), 0));
            }*/
            //Making list of inactive walkers
            walkEnemyList = new List<WalkEnemy>();
            for (int i = 0; i < 50; i++) // Cap at 50
            {
                walkEnemyList.Add(new WalkEnemy(GargoyleSprite));
                walkEnemyList[i].OnDeathScore += IncreaseScore;
            }
            upgradeEnemyList = new List<UpgradeEnemy>();
            //fill with inactive ones
            //eight in total
            for(int i = 0; i < 8; i++)
            {
                upgradeEnemyList.Add(new UpgradeEnemy(UpgradeSheet, new Rectangle(0, 0, 150, 150),rng));
                upgradeEnemyList[i].OnDeathUpgrade += player.ApplyUpgrade;
            }

            shootEnemyList = new List<ShootingEnemy>();
            //fill with inactive ones
            //eight in total
            for (int i = 0; i < 20; i++)
            {
                shootEnemyList.Add(new ShootingEnemy(eyeballAsset));
            }
            
        }

        public List<ShootingEnemy> GetShooters()
        {
            return shootEnemyList;
        }
        private void IncreaseScore(int score)
        {
            this.score += score;
        }

        /// <summary>
        /// Returns the total number of active enemies
        /// </summary>
        public int TotalEnemyCount
        {
            get 
            {
                int total = 0;
                foreach(WalkEnemy walke in walkEnemyList)
                {
                    if (walke.Active)
                        total++;
                }
                foreach(ShootingEnemy sho in shootEnemyList)
                {
                    if (sho.Active)
                        total++;
                }    
                return total; 
            }
        }
        #region Some Properties
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
        /*public List<TestEnemy> GetTestEnemies
        {
            get { return testEnemyList; }
        }*/
        public List<WalkEnemy> GetWalkEnemies
        {
            get { return walkEnemyList; }
        }
        #endregion

        /// <summary>
        /// TODO Change difficulty scaling and stuff
        /// Used for updating the waves
        /// </summary>
        public void UpdateWaveStuff()
        {
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
                        int randNum = rng.Next(10);
                        //Change LATER for different enemy types
                        if (randNum > 7)
                        {
                            SpawnEnemy(EnemyTypes.BigGargoyle);
                            curWavePoints -= 3;
                        }
                        else if(randNum >= 5)
                        {
                            SpawnEnemy(EnemyTypes.Shooter);
                            curWavePoints -= 2;
                        }
                        else if(randNum == 3)
                        {
                            SpawnEnemy(EnemyTypes.FastGuy);
                            curWavePoints -= 2;
                        }
                        else
                        {
                            SpawnEnemy(EnemyTypes.Gargoyle);
                            curWavePoints -= 1;
                        }
                    }
                    else
                    {
                        SpawnEnemy(EnemyTypes.Gargoyle);
                        curWavePoints -= 1;
                    }
                }
                //Every wave gets harder
                wavePoints += 1;
            }
            
        }

        /// <summary>
        /// Handles updating all the enemies and the spawning of them
        /// </summary>
        public void Update(GameTime gameTime, Player player)
        {
            switch (currState)
            {
                #region Wave state
                // Enemies waves spawn until timeToShop < 0 
                case EManagerState.Waves:
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    timeToWave -= gameTime.ElapsedGameTime.TotalSeconds;
                    timeToShop -= gameTime.ElapsedGameTime.TotalSeconds;

                    // Spawn and update wave stuff 
                    UpdateWaveStuff();

                    // Update the enemies
                    // UpdateTestEnemy(player, gameTime);
                    UpdateWalkEnemies(player, gameTime);

                    UpdateShooters(player, gameTime);

                    // Transition the state and reset some variables
                    if (timeToShop < 0)
                    {
                        // Will be used as time to consectively stand on a box to left the shop
                        timeToShop = 0;
                        timeToWave = 5;
                        currState = EManagerState.WaveToShop;
                    }
                    break;
                #endregion
                #region Wave to shop state
                // When all enemies die, state ends and shop opens 
                case EManagerState.WaveToShop:
                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    // UpdateTestEnemy(player, gameTime);
                    UpdateWalkEnemies(player, gameTime);
                    UpdateShooters(player, gameTime);

                    if (TotalEnemyCount == 0)
                    {
                        timeToShop = 0;
                        timeToWave = 5;
                        currState = EManagerState.Shop;
                    }
                    break;
                #endregion
                #region Shop state
                // Ends when player stands on exit box for 3 seconds
                case EManagerState.Shop:
                    foreach(UpgradeEnemy uEnemy in upgradeEnemyList)
                    {
                        uEnemy.UpdateAnimation(gameTime);
                    }
                    // Populating shop
                    // Hardcoded
                    for(int i = 0; i < upgradeEnemyList.Count; i++)
                    {
                        if(!upgradeEnemyList[i].Active)
                        {
                            switch (i)
                            {
                                //Planning to have one for each stat
                                //Hp
                                case 0:
                                    upgradeEnemyList[i].Reset(3840 / 2 - 300, 2176 / 2 - 300, 5, 1, 0, new BulletStats(0, 0, 0, 0));
                                    break;
                                //Speed
                                case 1:
                                    upgradeEnemyList[i].Reset(3840 / 2 - 100, 2176 / 2 - 300, 5, 0, 1, new BulletStats(0, 0, 0, 0));
                                    break;
                                //Bullet size
                                case 2:
                                    upgradeEnemyList[i].Reset(3840 / 2 - 800, 2176 / 2 + 300, 5, 0, 0, new BulletStats(10, 0, 0, 0));
                                    break;
                                //Bullet speed
                                case 3:
                                    upgradeEnemyList[i].Reset(3840 / 2 - 600, 2176 / 2 + 300, 5, 0, 0, new BulletStats(0, 1, 0, 0));
                                    break;
                                //Bullet pierce
                                case 4:
                                    if(player.BStats.Passes < 4)
                                    {
                                        upgradeEnemyList[i].Reset(3840 / 2 - 400, 2176 / 2 + 300,
                                        10 * player.BStats.Passes //COST INCREASES BY THE STAT
                                        , 0, 0, new BulletStats(0, 0, 1, 0));
                                    }
                                    
                                    break;
                                //Bullet damage
                                case 5:
                                    upgradeEnemyList[i].Reset(3840 / 2 - 200, 2176 / 2 + 300,
                                        10 * player.BStats.Damage
                                        , 0, 0, new BulletStats(0, 0, 0, 1));
                                    break;
                                case 100:
                                    upgradeEnemyList[i].Reset(3840 / 2, 2176 / 2 + 300, 7, 0, 0, new BulletStats(20, 7, 2, 0));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // Reuses timeToShop variable as exit shop timer variable
                    // Controlling the exit box logic here
                    // When the player is in the box, counts to 3
                    if (player.Position.Intersects(exitBox))
                    {
                        timeToShop += gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    // Player is off exit box and was previously on it for <3s
                    // (Sets timer to 0 again)
                    else if (timeToShop > 0)
                    {
                        timeToShop -= gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    // Safety to keep timer at or above 0
                    else if(timeToShop < 0)
                    {
                        timeToShop = 0;
                    }

                    // When the player has stood in exit box for 3s
                    // Transition out of shop
                    if (timeToShop > 3)
                    {
                        //Adding some bullets and hp back
                        player.BulletCount += 5;
                        player.Health += 1;
                        //actually reseting now
                        ShopTime += 10;
                        timeToShop = ShopTime;
                        timeToWave = 5;
                        //clear the shop enemies
                        foreach (UpgradeEnemy upEn in upgradeEnemyList)
                            upEn.Health = 0;
                        player.BulletCount += 5;
                        currState = EManagerState.Waves;
                    }
                    break;
                    #endregion
            }
            camera.Follow(player);
        }
        //public void DebugUpdate()

        /// <summary>
        /// Updates all the TestEnemies
        /// </summary>
        /*private void UpdateTestEnemy(Player player, GameTime gameTime)
        {
            //Updating each enemy and checking the collision
            //As well as making sure they don't overlap
            for (int i = 0; i < testEnemyList.Count; i++)
            {
                testEnemyList[i].Update(player, gameTime);
                if (testEnemyList[i].CollideWith(player))
                {
                    testEnemyList[i].HitPlayer(player);
                }
                //Checking each enemy with each other
                for (int j = i + 1; j < testEnemyList.Count; j++)
                {
                    if (testEnemyList[i].CollideWith(testEnemyList[j]))
                        testEnemyList[i].MoveAwayFrom(testEnemyList[j]);
                }
            }
        }*/

        private void UpdateWalkEnemies(Player player, GameTime gameTime)
        {
            for (int i = 0; i < walkEnemyList.Count; i++)
            {
                walkEnemyList[i].Update(player, gameTime);
                if (walkEnemyList[i].CollideWith(player))
                    walkEnemyList[i].HitPlayer(player);
                //Checking each enemy with each other
                for (int j = i + 1; j < walkEnemyList.Count; j++)
                {
                    if (walkEnemyList[i].CollideWith(walkEnemyList[j]))
                        walkEnemyList[i].MoveAwayFrom(walkEnemyList[j]);
                }
            }
        }

        private void UpdateShooters(Player player, GameTime gameTime)
        {
            foreach(ShootingEnemy sho in shootEnemyList)
            {
                sho.Update(player, gameTime);
                if (sho.CollideWith(player))
                    sho.HitPlayer(player);
            }
        }
        public void Draw(SpriteBatch sb, SpriteFont font)
        {
            switch(currState)
            {
                case EManagerState.Waves:
                    /*foreach (TestEnemy en in testEnemyList)
                    {
                        en.Draw(sb);
                    }*/
                    foreach (WalkEnemy walke in walkEnemyList)
                    {
                        if(walke.Speed == GargoyleStats.Speed)
                        {
                            walke.DrawWalkEnemy(sb,Color.White);
                        }
                        
                        else
                        {
                            walke.Draw(sb);
                        }

                    }
                    foreach (ShootingEnemy sho in shootEnemyList)
                        sho.Draw(sb);

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
                    /*foreach (TestEnemy en in testEnemyList)
                    {
                        en.Draw(sb);
                    }*/
                    foreach (WalkEnemy walke in walkEnemyList)
                    {
                        if (walke.Speed == GargoyleStats.Speed)
                        {
                            walke.DrawWalkEnemy(sb, Color.White);
                        }
                        else
                        {
                            walke.Draw(sb);
                        }

                    }
                    foreach (ShootingEnemy sho in shootEnemyList)
                        sho.Draw(sb);
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
                            sb.DrawString(font, "HP UP", new Vector2(upgradeEnemyList[i].X , upgradeEnemyList[i].Y - 50), Color.Goldenrod);
                        if (i == 1)
                            sb.DrawString(font, "SPD UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y - 50), Color.Goldenrod);
                        if (i == 2)
                            sb.DrawString(font, "BUL SIZE UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y -50), Color.Goldenrod);
                        if (i == 3)
                            sb.DrawString(font, "BUL SPD UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y - 50), Color.Goldenrod);
                        if (i == 4)
                            sb.DrawString(font, "PIERCE UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y- 50), Color.Goldenrod);
                        if (i == 5)
                            sb.DrawString(font, "DMG UP", new Vector2(upgradeEnemyList[i].X, upgradeEnemyList[i].Y - 50), Color.Goldenrod);
                    }
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
                        Color.Goldenrod);
                    //Drawing the exit box
                    Texture2D tempTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
                    tempTexture.SetData(new Color[] { Color.White });
                    sb.Draw(tempTexture, exitBox, Color.Gray);
                    sb.Draw(tempTexture, new Rectangle(exitBox.X, exitBox.Y, (int)(exitBox.Width * (timeToShop / 3)), exitBox.Height), Color.DarkBlue);
                    sb.DrawString(
                        font,
                        String.Format("EXIT"),
                        new Vector2(exitBox.X- 30, exitBox.Y - 60),
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
        //Spawns the type of enemy you want
        private void SpawnEnemy(EnemyTypes enemyType)
        {
            switch(enemyType)
            {
                case EnemyTypes.Gargoyle:
                    SpawnGargoyle();
                    break;
                case EnemyTypes.BigGargoyle:
                    SpawnBigGargoyle();
                    break;
                case EnemyTypes.FastGuy:
                    SpawnFastGuy();
                    break;
                case EnemyTypes.Shooter:
                    SpawnShooter();
                    break;
                default:
                    break;
            }
        }
        private void SpawnGargoyle()
        {
            WalkEnemy spawn = GetWalkEnemy();
            if (spawn != null)
            {
                spawn.Reset(GargoyleSpriteSheet, RandPoint(GargoyleStats.Width, GargoyleStats.Height), GargoyleStats);
            }
        }
        private void SpawnBigGargoyle()
        {
            WalkEnemy spawn = GetWalkEnemy();
            if (spawn != null)
            {
                spawn.Reset(GargoyleAsset, RandPoint(BigGargoyleStats.Width, BigGargoyleStats.Height), BigGargoyleStats);
            }
        }

        private void SpawnFastGuy()
        {
            WalkEnemy spawn = GetWalkEnemy();
            if (spawn != null)
            {
                spawn.Reset(eyeballAsset, RandPoint(FastGuyStats.Width, FastGuyStats.Height), FastGuyStats);
            }
        }

        private void SpawnShooter()
        {
            ShootingEnemy spawn = GetShootingEnemy();
            if (spawn != null)
            {
                spawn.Reset(RandPoint(ShooterStats.Width, ShooterStats.Height), ShooterStats, 1, ShooterBullets);
            }
        }
        /// <summary>
        /// Helping method: gets the first inactive walk enemy
        /// </summary>
        private WalkEnemy GetWalkEnemy()
        {
            for (int i = 0; i < walkEnemyList.Count; i++)
            {
                if (walkEnemyList[i].Active == false)
                    return walkEnemyList[i];
            }
            return null;
        }

        private ShootingEnemy GetShootingEnemy()
        {
            for (int i = 0; i < walkEnemyList.Count; i++)
            {
                if (shootEnemyList[i].Active == false)
                    return shootEnemyList[i];
            }
            return null;
        }
        /// <summary>
        /// Gets a random point from offscreen
        /// </summary>
        /// <param name="width">Width of the enemy</param>
        /// <param name="height">Height of the enemy</param>
        /// <returns></returns>
        public Point RandPoint(int width, int height)
        {
            int randX;
            int randY;
            // 50/50 to decide to change x or y offscreen
            //enemy comes in from left or right
            if (rng.Next(2) > 0)
            {
                randY = rng.Next(0 - GargoyleAsset.Height, graphics.GraphicsDevice.Viewport.Height);
                randX = rng.Next(2);
                // enemy spawns on the east wall
                if (randX == 1)
                {
                        // dungeon.png width (scaled x2) - east wall thickness (scaled 2x) - enemy width
                        randX = 1920 * 2 - 96 * 2 - width;
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
                randX = rng.Next(0 - GargoyleAsset.Width, graphics.GraphicsDevice.Viewport.Width);
                randY = rng.Next(2);
                // enemy spawns south of viewport 
                if (randY == 1)
                {
                        // dungeon.png height (scaled x2) - south wall thickness (scaled 2x) - enemy height
                        randY = 1088 * 2 - 32 * 2 - height;
                }
                // enemy spawns north of viewport
                else
                {
                    // north wall height (scaled x2)
                    randY = 0 + 96 * 2;
                }
            }
            return new Point(randX + width/2, randY + height/2);
        }


        #region Prob delete this, was replaced code
        /* Replaced
        /// <summary>
        /// Spawns a test enemy at the given point
        /// </summary>
        /// <param name="pos">position to spawn the enemy</param>
        public void SpawnTestEnemy(Point pos)
        {
            testEnemyList.Add(
                    new TestEnemy(
                        GargoyleAsset,
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
                        GargoyleAsset,
                        new Rectangle(
                            pos,
                            new Point(150, 100)),
                        3)); //Health
        }*/

        /* Replaced
        // Gets a random point off screen
        public Point RandPoint(bool isGargoyle)
        {
            int randX;
            int randY;
            // 50/50 to decide to change x or y offscreen
            //enemy comes in from left or right
            if (rng.Next(2) > 0)
            {
                randY = rng.Next(0 - GargoyleAsset.Height, graphics.GraphicsDevice.Viewport.Height);
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
                randX = rng.Next(0 - GargoyleAsset.Width, graphics.GraphicsDevice.Viewport.Width);
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
        */
        #endregion
    }
}
