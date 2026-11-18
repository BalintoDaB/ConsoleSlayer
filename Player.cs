using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer
{
    public class Player
    {

        public Position Position { get; set; }
        public ConsoleSprite Sprite { get; set; }
        public double FillingRatio { get; set; }
        private int _health;
        public int Health
        {
            get => _health;
            set => _health = Math.Clamp(value, 0, MaxHealth);
        }
        private int _ammo;
        public int Ammo
        {
            get => _ammo;
            set => _ammo = Math.Clamp(value, 0, MaxAmmo);
        }
        public bool Alive { get; set; }
        public int CombatPoints { get; set; }
        public int MaxHealth 
        { 
            get
            {
                return 100 + (CombatPoints / 10);
            } 
        }
        public int MaxAmmo
        {
            get
            {
                return 10 + (CombatPoints / 50);
            }
        }
        public int SightRange { get; set; }

        public Player(int x, int y)
        {
            this.Position = new Position(x, y);
            this.Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Green, 'O');
            FillingRatio = 0.4;
            Health = 100;
            SightRange = 8;
            Alive = true;
            Ammo = 5;
        }

        public void Shoot()
        {
            Ammo--;
        }

        public void AddCombatPoints(int CPToAdd)
        {
            CombatPoints += CPToAdd;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if(Health == 0) Alive = false;
        }

        public void PickUpAmmo(int count)
        {
            Ammo += count;
        }

        public void PickUpHealth(int count)
        {
            Health += count;
        }
    }
}
