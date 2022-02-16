using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    //Everything that is part of the "GameplayState"
    abstract class GameObject
    {
        protected Texture2D sprite;
        //protected Rectangle[] hitboxes; Add for later
        protected Rectangle hitbox;
        /// <summary>
        /// Add this
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="hitbox"></param>
        public GameObject(Texture2D sprite, Rectangle hitbox)
        {
            this.sprite = sprite;
            this.hitbox = hitbox;
        }
        /// <summary>
        /// Returns the x of the upperleft corner
        /// </summary>
        public int X
        {
            get { return hitbox.X; }
        }
        /// <summary>
        /// Returns the x of the center of the object
        /// </summary>
        public int CenterX
        {
            get { return hitbox.X + hitbox.Width/2; }
        }
        /// <summary>
        /// Returns the y of the upperleft corner
        /// </summary>
        public int Y
        {
            get { return hitbox.Y; }
        }
        /// <summary>
        /// Returns the y of the center of the object
        /// </summary>
        public int CenterY
        {
            get { return hitbox.Y + hitbox.Height / 2; }
        }
        public abstract void Move();
        public abstract bool IsDead();
        public abstract void Update();
        public abstract void Draw(SpriteBatch sb);
        public bool CollideWith(GameObject other)
        {
            return true;
        }
    }
}
