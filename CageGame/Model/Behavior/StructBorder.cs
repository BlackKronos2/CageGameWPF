using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CageGame
{
    public struct Border
    {
        private Vector2 _point1 = Vector2.Zero;
        private Vector2 _point2 = Vector2.Zero;

        public const double StrokeThickness = 4f;

        public Vector2 Point1 => _point1;
        public Vector2 Point2 => _point2;

        public bool Init { get; private set; } = false;

        public Border(Vector2 point1, Vector2 point2)
        {
            _point1 = point1;
            _point2 = point2;

            Init = true;
        }

        public bool isVerticalBorder() 
            => Point1.X == Point2.X;
    }
}
