
namespace CageGame
{
    public struct Vector2
    {
        private double _x;
        private double _y;

        public double X => _x;
        public double Y => _y;

        public Vector2(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public static Vector2 Zero => new Vector2();

        public static Vector2 operator +(Vector2 vectorA, Vector2 vectorB) =>
            new Vector2(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y);

        public static Vector2 operator -(Vector2 vectorA, Vector2 vectorB) =>
            new Vector2(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y);

        public static Vector2 operator *(Vector2 vectorA, Vector2 vectorB) =>
            new Vector2(vectorA.X * vectorB.X, vectorA.Y * vectorB.Y);

        public static Vector2 operator /(Vector2 vector, double A) =>
            new Vector2(vector.X / A, vector.Y / A);

        public static Vector2 operator *(Vector2 vector, double A) =>
            new Vector2(vector.X * A, vector.Y * A);

        public static Vector2 CreateVector(Vector2 point1, Vector2 point2) => 
            point2 - point1;

        private static double VectorMultiplication(Vector2 vector1, Vector2 vector2) =>
            vector1.X * vector2.Y - vector1.Y * vector2.X;

        public static bool VectorIntersection(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            double v1 = VectorMultiplication(CreateVector(point3, point4), CreateVector(point3, point1));
            double v2 = VectorMultiplication(CreateVector(point3, point4), CreateVector(point3, point2));
            double v3 = VectorMultiplication(CreateVector(point1, point2), CreateVector(point1, point3));
            double v4 = VectorMultiplication(CreateVector(point1, point2), CreateVector(point1, point4));

            return (v1 * v2 < 0) && (v3 * v4 < 0);
        }
    }

    public abstract class Transform
    {
        public Vector2 Position { get; private set; }
        public Vector2 Scale { get; private set; }

        public Transform()
        {
            Position = Vector2.Zero;
        }
        public Transform(Vector2 position, Vector2 scale)
        {
            Position = position;
            Scale = scale;
        }

        public void Move(Vector2 MoveVector) =>
            Position += MoveVector;
    }
}
