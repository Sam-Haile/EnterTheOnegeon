using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EnterTheOnegeon
{
    /// <summary>
    /// Holds the stats of the bullet
    /// </summary>
    public struct BulletStats
    {
        //fields
        private int size;
        private double speed;
        private int passes;
        private int damage;

        public BulletStats(int size, double spd, int numPasses, int dmg)// int lifetime
        {
            speed = spd;
            passes = numPasses;
            damage = dmg;
            this.size = size;
        }
        public double Speed 
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
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        //Overloaded the + operator
        //Adding bullet stats together results in combining the stats
        public static BulletStats operator +(BulletStats a, BulletStats b)
            => new BulletStats(
                a.Size + b.Size,
                a.Speed + b.Speed,
                a.Passes + b.Passes,
                a.Damage + b.Damage);
    }
}
