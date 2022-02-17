using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace EnterTheOnegeon
{
    /// <summary>
    /// The player controlled character. Dies when health < 1
    /// </summary>
    class Player : GameObject
    {
        private int speed;
        private bool isDead;

        public Player(Texture2D sprite, Rectangle rectangle) : base(sprite, rectangle)
        {
            speed = 1;
            isDead = false;
        }

        public override void Update()
        {

        }

        /// <summary>
        /// ToDo: Keyboard input
        /// </summary>
        public override void Move()
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public override bool IsDead()
        {
            return true;
        }

    }
}
