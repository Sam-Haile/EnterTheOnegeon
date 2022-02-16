using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    class Player : GameObject
    {
        private int speed;
        private bool isDead;
        public Player(Texture2D sprite, Rectangle hitbox) : base(sprite, hitbox)
        {
            speed = 1;
            isDead = false;
        }


    }
}
