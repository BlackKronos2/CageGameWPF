using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CageGame
{
    public sealed class MenuPresenter : ModelViewBase
    {
		private static readonly Dictionary<int, (Vector2, string)> GameMapSize = new Dictionary<int, (Vector2, string)>()
		{
			{ 2, (new Vector2(850, 470), "Большой")},
			{ 1, (new Vector2(740, 370), "Средний")},
			{ 0, (new Vector2(550, 330), "Малый")},
		};

		#region FIELDS

		private string _count;
        private string _speed;
        private string _mapSize;

        private int _countValue;
        private int _speedValue;
        private int _mapSizeValue;

        public string Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
        public string Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                OnPropertyChanged();
            }
        }
        public string MapSize
        {
            get { return _mapSize; }
            set
            {
                _mapSize = value;
                OnPropertyChanged();
            }
        }

        public int CountValue
        {
            get { return _countValue; }
            set
            {
                _countValue = value;
                OnPropertyChanged();
                Count = $"Количество объектов: {CountValue}";
			}
        }
        public int SpeedValue
        {
            get { return _speedValue; }
            set
            {
                _speedValue = value;
                OnPropertyChanged();
                Speed = $"Скорость объектов: {SpeedValue}";
			}
        }
        public int MapSizeValue
        {
            get { return _mapSizeValue; }
            set
            {
                _mapSizeValue = value;
                OnPropertyChanged();
				MapSize = $"Размер поля: {GameMapSize[MapSizeValue].Item2}";
			}
        }

		#endregion

		#region BORDERS

		public int MaxCount => MenuModel.MaxCount;
        public int MaxSpeed => MenuModel.MaxSpeed;
        public static int MapMaxSize => GameMapSize.Values.Count - 1;

		public const int MapSizeMax = 2;
        public const int MinValue = 1;

		#endregion

        public ICommand StartGameCommand { get; }

		public MenuPresenter()
        {
            CountValue = 3;
            SpeedValue = MinValue;
            MapSizeValue = MinValue;

            StartGameCommand = new ViewModelCommand(StartGame);
		}

        private void StartGame(object obj)
        {
            GameModel gameModel = new GameModel(CountValue, SpeedValue, GameMapSize[MapSizeValue].Item1);
            GameEvents.SendGameStart(gameModel);
        }

        private void DrawEllipse(Canvas menuMap, Entity entity)
        {
            Ellipse ellipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            mySolidColorBrush.Color = Color.FromArgb(145, 170, 145, 255);
            ellipse.Fill = mySolidColorBrush;
            ellipse.StrokeThickness = 2;
            ellipse.Stroke = Brushes.Black;

            Canvas.SetLeft(ellipse, entity.Position.X - (entity.Scale.X / 2));
            Canvas.SetTop(ellipse, entity.Position.Y - (entity.Scale.Y / 2));

            ellipse.Width = entity.Scale.X;
            ellipse.Height = entity.Scale.Y;

            menuMap.Children.Add(ellipse);
        }

        public void DrawEllipses(Canvas menuMap, MenuModel model)
        {
            menuMap.Children.Clear();

            foreach (Entity entity in model.MenuEntities)
            {
                entity.Update();
                DrawEllipse(menuMap, entity);
            }
        }
    }
}
