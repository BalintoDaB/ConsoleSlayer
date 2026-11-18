using System.ComponentModel;

namespace ConsoleSlayer
{
    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }

        public Position(int x, int y)
        {
            this.x = x; this.y = y;
        }

        public static Position Add(Position p_1, Position p_2)
        {
            Position tmp = new Position(p_1.x + p_2.x, p_1.y + p_2.y);
            return tmp;
        }

        public static float Distance(Position p_1, Position p_2)
        {
            float dx = p_2.x - p_1.x;
            float dy = p_2.y - p_1.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
