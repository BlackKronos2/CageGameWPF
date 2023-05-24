using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CageGame
{
    public sealed class GameModel
    {
        /// <summary> В миллисекундах </summary>
        public const int FrameLength = 15;
        /// <summary> Отступ от границы для появления объектов </summary>
        private const double SpawnBorders = 15;

        private int _timer;
        private bool _endGameFlag;

        public DrawMaster DrawMaster { get; private set; }

        public List<Entity> Entities { get; private set; }

        public List<Border> DrawLines { get; private set; }
        public List<Border> CageLines { get; private set; }

        public float Time => _timer;
        public void TimerPlus() => _timer++;

        public GameModel(int entitiesCount, float entitiesSpeed, Vector2 mapSize)
        {
            _timer = 0;
            _endGameFlag = false;

            DrawLines = new List<Border>(0);
            CageLines = new List<Border>(0);

            CollisionMaster.GetInstance().ActiveBorders = DrawLines;
            CollisionMaster.GetInstance().Borders = CageLines;

            DrawMaster = new DrawMaster(DrawLines, CageLines);
            Entities = new List<Entity>(0);

            SpawnEntities(entitiesCount, entitiesSpeed, new Vector2(mapSize.X, mapSize.Y));

            GameEvents.OnCageSucces += (Vector2[] points) => CageEntities(points);
            GameEvents.OnCageSucces += (Vector2[] points) => GameEndTest();
        }

        private void SpawnEntities(int count, double speed, Vector2 mapSize)
        {
            int counter = count;
            while (counter-- > 0)
            {
                Random random = new Random();

                double scaleMult = random.NextDouble() + 1;
                int spawnBorder = (int)(scaleMult + SpawnBorders);
                Vector2 position = new Vector2(random.Next(spawnBorder, (int)mapSize.X) - scaleMult, random.Next(spawnBorder, (int)mapSize.Y) - scaleMult);
                Entities.Add(new Entity(position, new Vector2(15, 15) * scaleMult, speed, mapSize));
            }
        }
        private void CageEntities(Vector2[] points)
        {
            foreach (Entity entity in Entities.ToList())
            {
                if (entity.Position.X == Math.Clamp(entity.Position.X, Math.Min(points[0].X, points[2].X), Math.Max(points[0].X, points[2].X)) &&
                    (entity.Position.Y == Math.Clamp(entity.Position.Y, Math.Min(points[0].Y, points[2].Y), Math.Max(points[0].Y, points[2].Y))))
                {
                    Entities.Remove(entity);
                }
            }
        }
        private void GameEndTest()
        {
            if (Entities.Count == 0)
            {
                if (_endGameFlag)
                    return;

                GameEvents.SendGameEnd();
                _endGameFlag = true;
            }
        }

        public void UpdateTick()
        {
            foreach (Entity entity in Entities)
            {
                entity.Update();
            }
            DrawMaster.Draw();
        }
        public void DrawLineStart(Vector2 position) => DrawMaster.OnDrawLineStart(position);
        public void DrawLineStop() => DrawMaster.OnDrawLineStop(); 
        public void OnDrawLine(Vector2 position) => DrawMaster.OnDrawLine(position);

    }
}
