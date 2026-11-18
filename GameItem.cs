using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer
{
    public enum ItemType { Ammo, BFGCell, Door, LevelExit, Medikit, ToxicWaste, Wall }

    public class GameItem
    {
        public Position Position {  get; set; }
        public ConsoleSprite Sprite { get; set; }
        public ItemType Type { get; set; }
        public double FillingRatio { get; set; }
        public bool Available { get; set; }

        public GameItem(int x, int y, ItemType itemType)
        {
            Position = new Position(x, y);
            Type = itemType;
            SetInitialProperties();
        }

        private void SetInitialProperties()
        {
            Available = true;

            switch (Type)
            {
                case ItemType.Ammo:
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Cyan, ConsoleColor.Blue, 'A');
                    break;

                case ItemType.BFGCell:
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.DarkMagenta, ConsoleColor.Blue, 'B');
                    break;

                case ItemType.Door:
                    FillingRatio = 1.0;   // alapból zárva
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Blue, '/');
                    break;

                case ItemType.LevelExit:
                    FillingRatio = 1.0;
                    Sprite = new ConsoleSprite(ConsoleColor.DarkYellow, ConsoleColor.White, 'E');
                    break;

                case ItemType.Medikit:
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.DarkGray, ConsoleColor.Cyan, '+');
                    break;

                case ItemType.ToxicWaste:
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.DarkGray, ConsoleColor.Magenta, ':');
                    break;

                case ItemType.Wall:
                    FillingRatio = 1.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Gray, ' ');
                    break;
            }
        }

        public void Interact()
        {
            if(Type == ItemType.BFGCell || Type == ItemType.Ammo || Type == ItemType.Medikit) Available = false;
            if(Type == ItemType.Door)
            {
                if (FillingRatio == 0.0)
                {
                    FillingRatio = 1.0;
                    Sprite.Foreground = ConsoleColor.Blue;

                }
                else 
                {
                    FillingRatio = 0.0;
                    Sprite.Foreground = ConsoleColor.Cyan;
                }

            }
        }


    }
}
