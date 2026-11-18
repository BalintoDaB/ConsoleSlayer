using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer
{
    public enum DemonType { Zombieman, Imp, Mancubus }
    public enum DemonStateType { Idle, Move, Attack }

    public class Demon
    {
        public Position Position { get; set; }
        public ConsoleSprite Sprite { get; set; }
        public DemonType Type { get; set; }
        public DemonStateType State {  get; set; }
        public double FillingRatio { get; set; }
        public int Health { get; set; }
        public bool Alive { get; set; }
        public int SightRange { get; set; }
        public int AttackRange { get; set; }
        public int Speed {  get; set; }

        public Demon(int x, int y, DemonType type)
        {
            Position = new Position(x, y);
            Type = type;
            State = DemonStateType.Idle;
            SetInitialProperties();
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0) Alive = false;
        }

        public void UpdateState(Player player)
        {
            float d = Position.Distance(player.Position, Position);
            if (d < AttackRange)
            {
                State = DemonStateType.Attack;
            }
            else if (d < SightRange) 
            {
                State = DemonStateType.Move;
            }
            else
            {
                State = DemonStateType.Idle;
            }
        }

        private void SetInitialProperties()
        {
            Alive = true;

            switch (Type)
            {
                case DemonType.Zombieman:
                    FillingRatio = 0.4;
                    Health = 20;
                    SightRange = 3;
                    AttackRange = 1;
                    Speed = 70;
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Gray, 'o');
                    break;

                case DemonType.Imp:
                    FillingRatio = 0.4;
                    Health = 60;
                    SightRange = 6;
                    AttackRange = 3;
                    Speed = 93;
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Red, 'o');
                    break;

                case DemonType.Mancubus:
                    FillingRatio = 0.96;
                    Health = 600;
                    SightRange = 9;
                    AttackRange = 6;
                    Speed = 70;
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Magenta, 'O');
                    break;
            }
        }
    }
}
