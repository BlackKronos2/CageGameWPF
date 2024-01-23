using System;

namespace CageGame
{
    public sealed class GameEvents
    {
        public static Action? OnCageFail { get; set; }
        public static Action<Vector2[]>? OnCageSucces { get; set; }
        public static Action? OnGameEnd { get; set; }
		public static Action<GameModel>? OnGameStart { get; set; }

		public static void SendCageFail() => OnCageFail?.Invoke();
        public static void SendCageSucces(Vector2[] points) => OnCageSucces?.Invoke(points);
        public static void SendGameEnd() => OnGameEnd?.Invoke();
        public static void SendGameStart(GameModel gameModel) => OnGameStart?.Invoke(gameModel);
    }
}
