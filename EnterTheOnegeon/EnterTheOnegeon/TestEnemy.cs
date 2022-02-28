using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class TestEnemy : Enemy
    {
        public TestEnemy(Texture2D sprite, Rectangle rectangle, int health) : base(sprite, rectangle, health)
        {
        }
        public override bool CollideWith(GameObject other)
        {
            throw new NotImplementedException();
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
