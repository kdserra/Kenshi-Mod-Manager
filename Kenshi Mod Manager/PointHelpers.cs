using System.Drawing;

namespace Kenshi_Mod_Manager
{
    public static class PointHelpers
    {
        public static Point WithX(this Point point, int x)
        {
            return new Point(x, point.Y);
        }

        public static Point WithY(this Point point, int y)
        {
            return new Point(point.X, y);
        }
    }
}
