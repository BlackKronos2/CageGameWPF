using System.Collections.Generic;
using System.Linq;

namespace CageGame
{
    public class CollisionMaster
    {
        private static CollisionMaster Instance;

        public Border DrawingLine { get; set; }

        public List<Border> ActiveBorders { get; set; }
        public List<Border> Borders { get; set; }

        public static CollisionMaster GetInstance()
        {
            if (Instance == null)
                Instance = new CollisionMaster();
            return Instance;
        }

        private CollisionMaster() 
        { }

        private bool IntersectionBorder(Border border, Vector2 position, Vector2 futurePos, Vector2 scale)
        {
            double left;

            if (border.isVerticalBorder())
                left = (position.X < futurePos.X) ? (1) : (-1);
            else
                left = (position.Y < futurePos.Y) ? (1) : (-1);

            Vector2 FPos = futurePos + (scale / 2) * left;

            return Vector2.VectorIntersection(position, FPos, border.Point1, border.Point2);
        }

        public bool IntersectionVerticalBorder(Vector2 position, Vector2 futurePos, Vector2 scale)
        {
            foreach (Border line in Borders.Where(border => border.isVerticalBorder()))
            {
                if (IntersectionBorder(line, position, futurePos, scale))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IntersectionHorizontalBorder(Vector2 position, Vector2 futurePos, Vector2 scale)
        {
            foreach (Border line in Borders.Where(border => !border.isVerticalBorder()))
            {
                if (IntersectionBorder(line, position, futurePos, scale))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IntersectionLine(Vector2 point1, Vector2 point2)
        {
            foreach (Border line in new[] { ActiveBorders, Borders }.SelectMany(list => list))
            {
                if (IntersectionBorder(line, point1, point2, Vector2.Zero))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IntersectionDrawingLines(Vector2 position, Vector2 futurePos, Vector2 scale)
            => (IntersectionBorder(DrawingLine, position, futurePos, scale));
    }
}
