using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer
{
    public class ConsoleSprite
    {
        public ConsoleColor Background {  get; set; }
        public ConsoleColor Foreground { get; set; }
        public char Glyph { get; set; }

        public ConsoleSprite(ConsoleColor BackgroundColor, ConsoleColor ForegroundColor, char Glyph)
        {
            this.Background = BackgroundColor;
            this.Foreground = ForegroundColor;
            this.Glyph = Glyph;
        }

    }
}
