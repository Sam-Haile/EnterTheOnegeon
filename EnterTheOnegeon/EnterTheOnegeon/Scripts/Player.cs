using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    enum WalkState
    {
        FaceLeft,
        WalkLeft,
        FaceRight,
        WalkRight,
        FaceDown,
        WalkDown,
        FaceUp,
        WalkUp,        
    }
    /// <summary>
    /// The player controlled character. Dies when health < 1
    /// </summary>
    class Player : GameObject
    {
        private int speed;
        private int bulletCount;

        //temp hp
        private int hp;
        private int maxH;

        //Invincibility frames
        private double invTime;
        private double invTimer;

        //The stats of the bullet the player will shoot
        private BulletStats bStats;

        private int numSpritesInSheet;
        private int widthOfSingleSprite;
        private int heightOfSingleSprite;


        // Animation reqs
        int currentFrame;
        double fps;
        double secondsPerFrame;
        double timeCounter;

        // enum to hold the current state
        private WalkState walkState;

        public WalkState WalkState
        {
            get { return this.walkState; }
        }


        public Player(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            speed = 6;
            bulletCount = 15;

            maxH = 4;
            hp = maxH;
            invTime = 0.5;
            invTimer = 0;

            //Size, Speed, Num of Passes, Damage
            bStats = new BulletStats(10, 7, 1, 1);

            numSpritesInSheet = 6;
            widthOfSingleSprite = sprite.Width / numSpritesInSheet;
            heightOfSingleSprite = sprite.Height / 4;


            // Set up animation stuff
            currentFrame = 1;
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;


        }

        public int BulletCount
        {
            get { return bulletCount; }
            set { bulletCount = value; }
        }

        public int Health
        {
            get { return hp; }
            set { hp = value; }
        }

        public int MaxHealth
        {
            get { return maxH; }
        }

        public override bool Active
        {
            get { return hp > 0; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public double IFrameTimeLeft
        {
            get { return invTimer; }
        }

        public BulletStats BStats
        {
            get { return bStats; }
            set { bStats = value; }
        }

        public void TakeDamage(int damage)
        {
            if (invTimer <= 0)
            {
                hp -= damage;
                invTimer = invTime;
            }

        }

        public void Update(GameTime gameTime, MouseState mState, MouseState prevMState, KeyboardState kState, KeyboardState prevKState)
        {
            invTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            Move(kState);
            UpdateAnimation(gameTime);
        }

        /// <summary>
        /// ToDo: Keyboard input
        /// </summary>
        public void Move(KeyboardState kState)
        {
            if (kState.IsKeyDown(Keys.A))
            {
                rectangle.X -= speed;
                actualX -= speed;
            }
            if (kState.IsKeyDown(Keys.D))
            {
                rectangle.X += speed;
                actualX += speed;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                rectangle.Y += speed;
                actualY += speed;
            }
            if (kState.IsKeyDown(Keys.W))
            {
                rectangle.Y -= speed;
                actualY -= speed;
            }

            switch (walkState)
            {

                case WalkState.FaceLeft:

                    if (kState.IsKeyDown(Keys.D))
                    {
                        walkState = WalkState.FaceRight;
                    }
                    else if (kState.IsKeyDown(Keys.A))
                    {
                        walkState = WalkState.WalkLeft;
                    }
                    else if (kState.IsKeyDown(Keys.S))
                    {
                        walkState = WalkState.FaceDown;
                    }
                    else if (kState.IsKeyDown(Keys.W))
                    {
                        walkState = WalkState.FaceUp;
                    }
                    break;
                case WalkState.WalkLeft:

                    if(kState.IsKeyDown(Keys.A))
                    {
                    }
                    else if (kState.IsKeyDown(Keys.D))
                    {
                        walkState = WalkState.WalkRight;
                    }
                    else if (kState.IsKeyUp(Keys.A))
                    {
                        walkState = WalkState.FaceLeft;
                    }
                    else if (kState.IsKeyDown(Keys.S))
                    {
                        walkState = WalkState.WalkDown;
                    }
                    else if (kState.IsKeyDown(Keys.W))
                    {
                        walkState = WalkState.WalkUp;
                    }
                    break;


                case WalkState.FaceRight:

                    if (kState.IsKeyDown(Keys.A))
                    {
                        walkState = WalkState.FaceLeft;
                    }
                    else if (kState.IsKeyDown(Keys.D))
                    {
                        walkState = WalkState.WalkRight;
                    }

                    else if (kState.IsKeyDown(Keys.S))
                    {
                        walkState = WalkState.FaceDown;
                    }
                    else if (kState.IsKeyDown(Keys.W))
                    {
                        walkState = WalkState.FaceUp;
                    }
                    break;

                case WalkState.WalkRight:

                    if (kState.IsKeyDown(Keys.D))
                    {
                    }
                    else if (kState.IsKeyDown(Keys.A))
                    {
                        walkState = WalkState.FaceLeft;
                    }
                    else if (kState.IsKeyUp(Keys.D))
                    {
                        walkState = WalkState.FaceRight;
                    }
                    else if (kState.IsKeyDown(Keys.S))
                    {
                        walkState = WalkState.FaceDown;
                    }
                    else if (kState.IsKeyDown(Keys.W))
                    {
                        walkState = WalkState.FaceUp;
                    }
                    break;

                case WalkState.FaceDown:

                    
                    if (kState.IsKeyDown(Keys.S))
                    {
                        walkState = WalkState.WalkDown;
                    }
                    else if (kState.IsKeyDown(Keys.A))
                    {
                        walkState = WalkState.FaceLeft;
                    }
                    else if (kState.IsKeyDown(Keys.D))
                    {
                        walkState = WalkState.FaceRight;
                    }
                    else if (kState.IsKeyDown(Keys.W))
                    {
                        walkState = WalkState.FaceUp;
                    }
                    break;


                case WalkState.WalkDown:
                    if (kState.IsKeyUp(Keys.S))
                    {
                        walkState = WalkState.FaceDown;
                    }
                    else if (kState.IsKeyDown(Keys.A))
                    {
                        walkState = WalkState.FaceLeft;
                    }
                    else if (kState.IsKeyDown(Keys.D))
                    {
                        walkState = WalkState.FaceRight;
                    }
                    else if (kState.IsKeyDown(Keys.W))
                    {
                        walkState = WalkState.FaceUp;
                    }
                    break;

                case WalkState.FaceUp:


                    if (kState.IsKeyDown(Keys.W))
                    {
                        walkState = WalkState.WalkUp;
                    }
                    else if (kState.IsKeyDown(Keys.A))
                    {
                        walkState = WalkState.FaceLeft;
                    }
                    else if (kState.IsKeyDown(Keys.D))
                    {
                        walkState = WalkState.FaceRight;
                    }
                    else if (kState.IsKeyDown(Keys.S))
                    {
                        walkState = WalkState.FaceDown;
                    }
                    break;

                case WalkState.WalkUp:

                    
                    if (kState.IsKeyDown(Keys.S))
                    {
                        walkState = WalkState.FaceDown;
                    }
                    else if (kState.IsKeyUp(Keys.W))
                    {
                        walkState = WalkState.FaceUp;
                    }
                    else if (kState.IsKeyDown(Keys.A))
                    {
                        walkState = WalkState.FaceLeft;
                    }
                    else if (kState.IsKeyDown(Keys.D))
                    {
                        walkState = WalkState.FaceRight;
                    }
                    break;
                default:
                    break;
            }



            // Keeps player inside of the map
            if (rectangle.Y < 0 + 96 * 2) // North
            {
                rectangle.Y = 96 * 2;
                actualY = 96 * 2;
            }
            else if (rectangle.Y > 2176 - 32 * 2 - 64) // South
            {
                rectangle.Y = 2176 - 32 * 2 - 64;
                actualY = 2176 - 32 * 2 - 64;
            }
            if (rectangle.X < 0 + 96 * 2) // West
            {
                rectangle.X = 96 * 2;
                actualX = 96 * 2;
            }
            else if (rectangle.X > 3840 - 96 * 2 - 32) // East
            {
                rectangle.X = 3840 - 96 * 2 - 32;
                actualX = 3840 - 96 * 2 - 32;
            }
        }



        /// <summary>
        /// Updates the animation time
        /// </summary>
        /// <param name="gameTime">Game time information</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= 4) currentFrame = 1;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }

        }


        //Overriding draw to draw a hp bar as well
        public override void Draw(SpriteBatch sb)
        {
            //Hp bar here
            Texture2D tempTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
            tempTexture.SetData(new Color[] { Color.White });
            sb.Draw(tempTexture, new Rectangle(rectangle.X + 6, rectangle.Y + rectangle.Height + 20, rectangle.Width + 10, 5), Color.Red);
            sb.Draw(tempTexture, new Rectangle(rectangle.X + 6, rectangle.Y + rectangle.Height + 20, (int)(rectangle.Width * ((double)hp / maxH)) + 10 , 5), Color.LimeGreen);

            // different states for walking
            // player will turn red when hit
            switch (walkState)
            {
                case WalkState.FaceLeft:
                    if (walkState == WalkState.FaceLeft)
                    {
                        DrawPlayerStanding(82, sb, Color.White);

                        if (invTimer > 0)
                        {
                            DrawPlayerStanding(82, sb, Color.Red);
                        }
                    }
                    break;
                case WalkState.WalkLeft:
                    if (walkState == WalkState.WalkLeft)
                    {
                        DrawPlayerWalking(82, sb, Color.White);

                        if (invTimer > 0)
                        {
                            DrawPlayerWalking(82, sb, Color.Red);
                        }
                    }
                    break;
               
                case WalkState.FaceRight:
                    if (walkState == WalkState.FaceRight)
                    {
                        DrawPlayerStanding(152, sb, Color.White);

                        if (invTimer > 0)
                        {
                            DrawPlayerStanding(152, sb, Color.Red);
                        }
                    }
                    break;
                
                case WalkState.WalkRight:
                    if (walkState == WalkState.WalkRight)
                    {
                        DrawPlayerWalking(152, sb, Color.White);

                        if (invTimer > 0)
                        {
                            DrawPlayerWalking(152, sb, Color.Red);
                        }
                    }
                    break;

                case WalkState.FaceUp:
                    if (walkState == WalkState.FaceUp)
                    {
                        DrawPlayerStanding(218,sb, Color.White);

                        if (invTimer > 0)
                        {
                            DrawPlayerStanding(218, sb, Color.Red);
                        }
                    }
                    break;
                case WalkState.WalkUp:
                    if (walkState == WalkState.WalkUp)
                    {
                        DrawPlayerWalking(218, sb, Color.White);

                        if (invTimer > 0)
                        {
                            DrawPlayerWalking(218, sb, Color.Red);
                        }
                    }
                    break;

                case WalkState.FaceDown:
                    if (walkState == WalkState.FaceDown)
                    {
                        DrawPlayerStanding(10, sb, Color.White);

                        if (invTimer > 0)
                        {
                            DrawPlayerStanding(10, sb, Color.Red);

                        }
                    }
                    break;
                case WalkState.WalkDown:
                    if (walkState == WalkState.WalkDown)
                    {
                        DrawPlayerWalking(10, sb, Color.White);

                        if (invTimer > 0)
                        {
                            DrawPlayerWalking(10, sb, Color.Red);
                        }
                    }
                    break;

                default:
                    break;
            }
           
        }

        /// <summary>
        /// Draws player with a walking animation
        /// </summary>
        /// <param name="flip">Should he be flipped horizontally?</param>
        private void DrawPlayerWalking(int yOffset, SpriteBatch sb, Color color)
        {
            
            sb.Draw(
                sprite,
                new Vector2(rectangle.X - 10 , rectangle.Y + 10),
                new Rectangle(widthOfSingleSprite * currentFrame, yOffset, widthOfSingleSprite, heightOfSingleSprite - 5),
                color,
                0.0f,
                Vector2.Zero,
                1.2f,
                SpriteEffects.None,
                0.0f);
        }


        /// <summary>
        /// Draws player standing still
        /// </summary>
        /// <param name="flip">Should he be flipped horizontally?</param>
        private void DrawPlayerStanding(int yOffset, SpriteBatch sb, Color color)
        {
            sb.Draw(
                sprite,
                new Vector2(rectangle.X - 10, rectangle.Y + 10),
                new Rectangle(0, yOffset, widthOfSingleSprite, heightOfSingleSprite - 5),
                color,
                0.0f,
                Vector2.Zero,
                1.2f,
                SpriteEffects.None,
                0.0f);
        }



        /// <summary>
        /// Adds the stats in the param to the player's stats
        /// </summary>
        /// <param name="spd">Speed</param>
        /// <param name="hp">Max hp up</param>
        /// <param name="bs">Bullet's stats</param>
        public void ApplyUpgrade(int spd, int hp, BulletStats bs)
        {
            this.hp += hp;
            maxH += hp;
            speed += spd;
            bStats += bs;
        }

        public void Parry(GameObject other)
        {
            // if player is colliding with any enemy
            // foreach ()
            {
                //if (CollideWith(b))
                {
                    bulletCount += 1;
                }
            }
        }
    }
}
