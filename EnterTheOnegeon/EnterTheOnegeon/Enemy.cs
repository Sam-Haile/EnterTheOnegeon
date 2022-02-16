using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    abstract class Enemy : GameObject
    {
        public Enemy(Texture2D sprite, Rectangle hitbox) : base(sprite, hitbox)
        {

        }
    }
}
