using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon.Sprites
{
    /// <summary>
    /// Holds the stats of the bullet
    /// </summary>
    public struct BulletStats
    {
        private int speed;
        private int passes;
        private int damage;
        public BulletStats(int spd, int numPasses, int dmg)// int lifetime
        {
            speed = spd;
            passes = numPasses;
            damage = dmg;
        }
        public int Speed 
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Passes
        {
            get { return passes; }
            set { passes = value; }
        }
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

    }
}
