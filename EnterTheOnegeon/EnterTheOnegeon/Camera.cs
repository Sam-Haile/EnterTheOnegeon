using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class Camera
    {
        // can only be set in camera class
        public Matrix Transform { get; private set; }

        /// <summary>
        /// camera follows assigned sprite 
        /// </summary>
        /// <param name="target"></param>
        public void Follow(Player target)
        {
            //center point of sprite
            var position = Matrix.CreateTranslation(
              -target.Position.X - (target.Rectangle.Width),
              -target.Position.Y - (target.Rectangle.Height),
              0);
            // put sprite at center of the window
            var offset = Matrix.CreateTranslation(
                Game1.screenWidth / 2,
                Game1.screenHeight / 2,
                0);
            // assign transformation
            Transform = position * offset;
        }
    }
}
