using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon.Sprites
{
    enum WalkState
    {
        FaceLeft,
        WalkLeft,
        FaceRight,
        WalkRight
    }

    class Animation : Game1
    {
        protected Texture2D playerSprite;
        //protected Vector2 playerPosition;
        int numSpritesInSheet;
        int widthOfSingleSprite;

        // Animation reqs
        int currentFrame;
        double fps;
        double secondsPerFrame;
        double timeCounter;

        private WalkState walkState;

        public Animation(Texture2D texture, int numSpritesInSheet)
        {
            this.playerSprite = texture;
            this.numSpritesInSheet = numSpritesInSheet;
            widthOfSingleSprite = playerSprite.Width / numSpritesInSheet;

            // Set up animation stuff
            currentFrame = 1;
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
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
    }

}
