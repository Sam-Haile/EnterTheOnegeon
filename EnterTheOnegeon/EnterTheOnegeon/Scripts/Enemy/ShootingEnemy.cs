using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class ShootingEnemy : Enemy
    {
        private Vector2 playerPos;
        public ShootingEnemy(Texture2D sprite) : base(sprite, new Rectangle(), 0)
        {
            speed = 1;
            playerPos = new Vector2();
        }
    }
}
