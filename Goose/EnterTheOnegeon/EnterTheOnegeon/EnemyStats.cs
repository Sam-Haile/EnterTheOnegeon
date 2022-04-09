using System;
using System.Collections.Generic;
using System.Text;

namespace EnterTheOnegeon
{
    /// <summary>
    /// Holds the stats of the enemy
    /// 
    /// </summary>
    public struct EnemyStats
    {
        //fields
        private int width;
        private int height;
        private int health;
        private double speed;
        private int damage;

        
        public EnemyStats(int width, int height, int hp, double spd, int dmg)
        {
            this.width = width;
            this.height = height;
            health = hp;
            speed = spd;
            damage = dmg;
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
    }
}
