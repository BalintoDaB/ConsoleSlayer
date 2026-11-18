using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer
{
    public class ConsoleRenderer
    {
        public Game Game {  get; set; }

        private void RenderSingleSprite(Position pos, ConsoleSprite sprite)
        {
            if (pos.x < 0 || pos.y < 0 ||
                pos.x >= Console.WindowWidth || pos.y >= Console.WindowHeight - 1)
                return;
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ForegroundColor = sprite.Foreground;
            Console.BackgroundColor = sprite.Background;
            Console.Write(sprite.Glyph);
        }

        private bool IsWithinSight(Position pos)
        {
            return Position.Distance(Game.Player.Position, pos) <= Game.Player.SightRange;
        }

        public void RenderGame()
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            foreach (var item in Game.Items)
            {
                if (IsWithinSight(item.Position))
                    RenderSingleSprite(item.Position, item.Sprite);
            }
            foreach (var demon in Game.Demons)
            {
                if (IsWithinSight(demon.Position))
                    RenderSingleSprite(demon.Position, demon.Sprite);
            }
            RenderSingleSprite(Game.Player.Position, Game.Player.Sprite);
            RenderHUD();
        }

        private void RenderHUD()
        {
            int width = Console.WindowWidth;
            int row = Console.WindowHeight - 1;
            if (width < 2 || row < 0) return;

            Player p = Game.Player;
            string text = $" Ammo: {p.Ammo}/{p.MaxAmmo} | HP: {p.Health}/{p.MaxHealth} | BFG: {p.BFGCells}/{p.MaxBFGCells} | XP: {p.CombatPoints}";
            if (text.Length > width - 1) text = text.Substring(0, width - 1);
            text = text.PadRight(width - 1);

            Console.SetCursorPosition(0, row);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(text);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public ConsoleRenderer(Game game)
        {
            Game = game;
        }
    }
}
