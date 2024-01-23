using System.Collections.Generic;
using System.Windows.Controls;

namespace CageGame
{
    public sealed class MenuModel
    {
        public const int MaxCount = 15;
        public const int MaxSpeed = 10;

        public List<Entity> MenuEntities { get; private set; }

        public int InputCount { get; set; } = 4;
        public float SpeedValue { get; set; } = 3f;

        public MenuModel(Canvas menuMap)
        {
            MenuEntities = new List<Entity>(0);

            Vector2 foneSize = new Vector2(menuMap.Width, menuMap.Height);

            MenuEntities.Add(new Entity(new Vector2(20, 20), new Vector2(40, 40), SpeedValue, foneSize));
            MenuEntities.Add(new Entity(new Vector2(foneSize.X / 2, foneSize.Y / 2), new Vector2(30, 30), SpeedValue, foneSize));
            MenuEntities.Add(new Entity(new Vector2(foneSize.X / 2 + 30, 20), new Vector2(25, 25), SpeedValue, foneSize));

            CollisionMaster.GetInstance().ActiveBorders = new List<Border>(0);
            CollisionMaster.GetInstance().Borders = new List<Border>(0);
        }
    }
}
