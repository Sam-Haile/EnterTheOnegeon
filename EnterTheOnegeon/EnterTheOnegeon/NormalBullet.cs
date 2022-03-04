using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    class NormalBullet : Bullet
    {

        public NormalBullet(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle )
        {
            this.sprite = sprite;
            isVisible = false;
        }

        public override bool CollideWith(GameObject other)
        {
            throw new NotImplementedException();
        }

        public override bool IsDead()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }


    }
}
