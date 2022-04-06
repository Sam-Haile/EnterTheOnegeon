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
        /// checks if player is dead or bullet should be removed
        /// </summary>
        public bool isRemoved = false;


        

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
        /// Returns the rectangle coordinates of a sprite
        /// </summary>
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, sprite.Width, sprite.Height); }
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

        //
        public Vector2 PositionV
        {
            get
            {
                return new Vector2(this.CenterX, this.CenterY);
            }
        }

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
        public virtual bool CollideWith(GameObject other)
        {
            if (this.Position.Intersects(other.Position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Gets a vector for the direction to a position defined by 2 doubles
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns></returns>
        public Vector2 VectorToPosition(double x, double y)
        {
            return new Vector2((float)(x - this.CenterX), (float)(y - this.CenterY));
        }
        /// <summary>
        /// Gets a vector for the direction to a position defined by a vector2
        /// </summary>
        /// <param name="pos">Vector2 position</param>
        /// <returns></returns>
        public Vector2 VectorToPosition(Vector2 pos)
        {
            return new Vector2((float)(pos.X - this.CenterX), (float)(pos.Y - this.CenterY));
        }

        /// <summary>
        /// Moves both game objects away from each other
        /// </summary>
        /// <param name="other">The other gameobject</param>
        public void MoveAwayFrom(GameObject other)
        {
            int moveBack = 5;
            //On the right side
            if(this.CenterX > other.CenterX)
            {
                rectangle.X += moveBack;
                other.rectangle.X += -moveBack;
            }
            //On the left side
            if (this.CenterX < other.CenterX)
            {
                rectangle.X += -moveBack;
                other.rectangle.X += moveBack;
            }
            //Under it
            if (this.CenterY > other.CenterY)
            {
                rectangle.Y += moveBack;
                other.rectangle.Y += -moveBack;
            }
            //Above it
            if (this.CenterY > other.CenterY)
            {
                rectangle.Y += -moveBack;
                other.rectangle.Y += moveBack;
            }
        }
    }
}
