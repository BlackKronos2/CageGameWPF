using System.Collections.Generic;
using System.Windows.Controls;


namespace CageGame
{
    public enum MapSize
    {
        BIG,
        MEDIUM,
        SMALL,
        VERY_SMALL,
    }

    public sealed class MenuModel
    {
        public const int MaxCount = 15;
        public const int MaxSpeed = 10;

        public List<Entity> MenuEntities { get; private set; }

        public int InputCount { get; set; } = 4;
        public float SpeedValue { get; set; } = 1f;
        public MapSize CurrentMapSize { get; set; }

        public static Dictionary<MapSize, (Vector2, string)> s_mapSize = new Dictionary<MapSize, (Vector2, string)>()
        {
            { MapSize.BIG, (new Vector2(850, 470), "Большой")},
            { MapSize.MEDIUM, (new Vector2(740, 370), "Средний")},
            { MapSize.SMALL, (new Vector2(550, 330), "Малый")},
        };

        public MenuModel(Canvas menuMap)
        {
            MenuEntities = new List<Entity>(0);
            CurrentMapSize = MapSize.BIG;

            Vector2 foneSize = new Vector2(menuMap.Width, menuMap.Height);

            MenuEntities.Add(new Entity(new Vector2(20, 20), new Vector2(40, 40), 0.5f, foneSize));
            MenuEntities.Add(new Entity(new Vector2(foneSize.X / 2, foneSize.Y / 2), new Vector2(30, 30), 0.5f, foneSize));
            MenuEntities.Add(new Entity(new Vector2(foneSize.X / 2 + 30, 20), new Vector2(25, 25), 0.5f, foneSize));

            CollisionMaster.GetInstance().ActiveBorders = new List<Border>(0);
            CollisionMaster.GetInstance().Borders = new List<Border>(0);
        }

        public void StartGame()
        {
            GameWindow gameWindow = new GameWindow(InputCount, SpeedValue / 3, s_mapSize[CurrentMapSize].Item1);
            gameWindow.Show();
        }
    }
}
