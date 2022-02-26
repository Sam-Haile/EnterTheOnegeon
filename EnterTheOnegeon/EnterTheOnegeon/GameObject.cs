using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    /// <summary>
    /// Everything that is part of the "GameplayState"
    /// </summary>

    abstract class GameObject
    {
        /// <summary>
        /// The sprite drawn on screen representing GameObject
        /// </summary>
        protected Texture2D sprite;

        //protected Rectangle[] hitboxes; Add for later

        /// <summary>
        /// The rectangle object containing GameObject's coordinates and dimensions
        /// </summary>
        protected Rectangle rectangle;

        /// <summary>
        /// "sprite" is the sprite drawn on screen representing GameObject, and "rectangle" is the 
        /// Rectangle that contains GameObject's coordinates and dimensions
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="rectangle"></param>
        public GameObject(Texture2D sprite, Rectangle rectangle)
        {
            this.sprite = sprite;
            this.rectangle = rectangle;
        }

        /// <summary>
        /// Returns the x of the upper left corner of GameObject
        /// </summary>
        public int X
        {
            get { return rectangle.X; }
        }

        /// <summary>
        /// Returns the x of the center of the GameObject
        /// </summary>
        public int CenterX
        {
            get { return rectangle.X + rectangle.Width/2; }
        }

        /// <summary>
        /// Returns the y of the upper left corner of GameObject
        /// </summary>
        public int Y
        {
            get { return rectangle.Y; }
        }

        /// <summary>
        /// Returns the y of the center of GameObject
        /// </summary>
        public int CenterY
        {
            get { return rectangle.Y + rectangle.Height / 2; }
        }

        /// <summary>
        /// return the rectangle of GameObject
        /// </summary>
        public Rectangle Position 
        {
            get { return rectangle; }
        }

        /// <summary>
        /// Will be used in child classes to move said class's X and Y coordinates
        /// This will be done via keyboard input for the player and AI for NPC
        /// </summary>
        public abstract void Move();

        /// <summary>
        /// Will return true if this GameObject is dead.
        /// </summary>
        public abstract bool IsDead();

        /// <summary>
        /// Not sure what this will be used for in each individual class
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Will be used to draw this object on screen
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, rectangle, Color.White);
        }

        /// <summary>
        /// Will check if this GameObject is colliding with another GameObject
        /// Some GameObjects like the player will have hitboxes smaller than the dimensions of their 
        /// rectangle
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true if this object is collding with object "other"</returns>
        public abstract bool CollideWith(GameObject other);
    }
}
