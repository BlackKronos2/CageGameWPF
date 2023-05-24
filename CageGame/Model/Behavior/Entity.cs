using System;
using System.Linq;

namespace CageGame
{
    public class Entity: Transform
    {
        private Vector2 _mapMaxBorder;

        private Vector2 _moveVector;
        private double _speed = 2f;

        private static Vector2[] Directions = new Vector2[]
        {
            new Vector2(1,1),
            new Vector2(-1,-1),
            new Vector2(-1,1),
            new Vector2(1,-1),
        };

        public Entity(Vector2 position, Vector2 scale, double speed,Vector2 mapSize):base(position, scale)
        {
            _mapMaxBorder = mapSize;
            _speed = speed;

            Random random = new Random();
            _moveVector = Directions[random.Next(Directions.Count())];
        }

        public void Update()
        {
            Vector2 FuturePosition = Position + _moveVector * _speed;

            if (CollisionMaster.GetInstance().IntersectionDrawingLines(Position, FuturePosition, Scale))
                GameEvents.SendCageFail();

            if (FuturePosition.Y + (Scale.Y / 2) > _mapMaxBorder.Y || FuturePosition.Y - (Scale.Y / 2) < 0)
                _moveVector *= new Vector2(1, -1);

            if (FuturePosition.X + (Scale.X / 2) > _mapMaxBorder.X || FuturePosition.X - (Scale.X / 2) < 0)
                _moveVector *= new Vector2(-1, 1);

            if(CollisionMaster.GetInstance().IntersectionVerticalBorder(Position, FuturePosition, Scale))
                _moveVector *= new Vector2(-1, 1);

            if (CollisionMaster.GetInstance().IntersectionHorizontalBorder(Position, FuturePosition, Scale))
                _moveVector *= new Vector2(1, -1);

            Move(_moveVector * _speed);
        }
    }
}
