﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    ///Everything that is part of the "GameplayState"
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
        protected Rectangle hitbox;

        /// <summary>
        /// "sprite" is the sprite drawn on screen representing GameObject, and "hitbox" is the 
        /// rectangle that contains GameObject's coordinates and dimensions
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="hitbox"></param>
        public GameObject(Texture2D sprite, Rectangle hitbox)
        {
            this.sprite = sprite;
            this.hitbox = hitbox;
        }

        /// <summary>
        /// Returns the x of the upper left corner of GameObject
        /// </summary>
        public int X
        {
            get { return hitbox.X; }
        }

        /// <summary>
        /// Returns the x of the center of the GameObject
        /// </summary>
        public int CenterX
        {
            get { return hitbox.X + hitbox.Width/2; }
        }

        /// <summary>
        /// Returns the y of the upper left corner of GameObject
        /// </summary>
        public int Y
        {
            get { return hitbox.Y; }
        }

        /// <summary>
        /// Returns the y of the center of GameObject
        /// </summary>
        public int CenterY
        {
            get { return hitbox.Y + hitbox.Height / 2; }
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
        public abstract void Draw(SpriteBatch sb);

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
