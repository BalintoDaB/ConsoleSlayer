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
                pos.x >= Console.WindowWidth || pos.y >= Console.WindowHeight)
                return;
            Console.SetCursorPosition(pos.x, pos.y);
            Console.ForegroundColor = sprite.Foreground;
            Console.BackgroundColor = sprite.Background;
            Console.Write(sprite.Glyph);
        }

        public void RenderGame()
        {
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.Clear();
            foreach (var item in Game.Items)
            {
                RenderSingleSprite(item.Position, item.Sprite);
            }
            foreach (var demon in Game.Demons)
            {
                RenderSingleSprite(demon.Position, demon.Sprite);
            }
            RenderSingleSprite(Game.Player.Position, Game.Player.Sprite);
        }

        public ConsoleRenderer(Game game)
        {
            Game = game;
        }
    }
}
