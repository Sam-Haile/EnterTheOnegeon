﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class TestEnemy : Enemy
    {
        int speed;
        Vector2 playerPos;
        Random rand;
        public TestEnemy(Texture2D sprite, Rectangle rectangle, int health) : base(sprite, rectangle, health)
        {
            speed = 5;
            rand = new Random();
            playerPos = new Vector2(rand.Next(0, 600), rand.Next(0, 450));
        }
        public override bool CollideWith(GameObject other)
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
        /// Movement testing for now
        /// </summary>
        public override void Move()
        {
            Vector2 direction = this.VectorToPosition(playerPos);
            
            direction.Normalize();
            rectangle.X += (int) (direction.X * speed);
            rectangle.Y += (int) (direction.Y * speed);
        }

        public override void Update()
        {
            this.Move();
        }
        //
        public void Relocate(Player p)
        {
            playerPos = p.PositionV;
        }

    }
}
