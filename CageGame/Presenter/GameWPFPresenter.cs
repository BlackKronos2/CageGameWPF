using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CageGame
{
    public class GameWPFPresenter
    {
        private GameModel _gameModel;
        private Canvas _map;
        private Label _counter;
        private Label _timer;

        private bool _gameEnd;

        public bool isGameEnd => _gameEnd;

        public GameWPFPresenter(GameModel gameModel, Canvas drawMap, Label counter, Label timer)
        {
            GameDebug.GetInstance().DebugMap = drawMap;

            _gameModel = gameModel;
            _map = drawMap;
            _counter = counter;
            _timer = timer;

            _gameEnd = false;
            GameEvents.OnGameEnd += () => GameEnd();
            GameEvents.OnCageSucces += (Vector2[] points) => CounterUpdate();

            CounterUpdate();
        }

        #region DRAW

        private void DrawBorder(Border drawingBorder)
        {
            Line DrawLine = new Line();
            DrawLine.StrokeThickness = Border.StrokeThickness;
            DrawLine.Stroke = Brushes.Blue;
            DrawLine.X1 = drawingBorder.Point1.X;
            DrawLine.X2 = drawingBorder.Point2.X;
            DrawLine.Y1 = drawingBorder.Point1.Y;
            DrawLine.Y2 = drawingBorder.Point2.Y;

            _map.Children.Add(DrawLine);
        }

        private void DrawBorder(Border drawingBorder, SolidColorBrush color)
        {
            Line DrawLine = new Line();
            DrawLine.StrokeThickness = Border.StrokeThickness;
            DrawLine.Stroke = color;
            DrawLine.X1 = drawingBorder.Point1.X;
            DrawLine.X2 = drawingBorder.Point2.X;
            DrawLine.Y1 = drawingBorder.Point1.Y;
            DrawLine.Y2 = drawingBorder.Point2.Y;

            _map.Children.Add(DrawLine);
        }

        private void DrawLines()
        {
            foreach (Border line in _gameModel.CageLines)
            {
                DrawBorder(line);
            }

            foreach (Border line in _gameModel.DrawLines)
            {
                DrawBorder(line, Brushes.Red);
            }
        }

        private void DrawEntity()
        {
            foreach (Entity entity in _gameModel.Entities)
            {
                Random random = new Random();

                Ellipse ellipse = new Ellipse();
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                //mySolidColorBrush.Color = EntitiesColors[random.Next(EntitiesColors.Count())];
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 255);
                ellipse.Fill = mySolidColorBrush;
                ellipse.StrokeThickness = 2;
                ellipse.Stroke = Brushes.Black;

                Canvas.SetLeft(ellipse, entity.Position.X - (entity.Scale.X / 2));
                Canvas.SetTop(ellipse, entity.Position.Y - (entity.Scale.Y / 2));

                ellipse.Width = entity.Scale.X;
                ellipse.Height = entity.Scale.Y;

                _map.Children.Add(ellipse);
            }
        }

        private void DrawActiveLine()
        {
            Border drawingLine = _gameModel.DrawMaster.ActiveLine;

            if (!drawingLine.Init)
                return;

            Line DrawLine = new Line();
            DrawLine.StrokeThickness = 4;
            DrawLine.Stroke = Brushes.White;
            DrawLine.X1 = drawingLine.Point1.X;
            DrawLine.X2 = drawingLine.Point2.X;
            DrawLine.Y1 = drawingLine.Point1.Y;
            DrawLine.Y2 = drawingLine.Point2.Y;

            _map.Children.Add(DrawLine);
        }

        #endregion

        #region UI

        private void GameEnd()
        {
            if (_gameEnd)
                return;

            _gameEnd = true;
            _counter.Content = $"Количество объектов: 0";
            MessageBox.Show($"Игра окончена \nИтоговое время: {_gameModel.Time.ToString("##.##")}");
            OpenMenu();
        }

        private void CounterUpdate()
        {
            int count = _gameModel.Entities.Count;
            _counter.Content = $"Количество объектов: {count}";
        }

        private void TimerUpdate() => _timer.Content = "Время: " + _gameModel.Time.ToString();

        public void OpenMenu()
        {
            StartMenu menu = new StartMenu();
            menu.Show();
        }

        #endregion

        #region INPUT

        public void StartDrawLine() 
            => _gameModel.DrawLineStart(new Vector2(Mouse.GetPosition(_map).X, Mouse.GetPosition(_map).Y));

        public void StopDrawLine()
            => _gameModel.DrawLineStop();

        public void OnDrawLine()
            => _gameModel.OnDrawLine(new Vector2(Mouse.GetPosition(_map).X, Mouse.GetPosition(_map).Y));

        public void Update()
        {
            if (_gameEnd)
                return;

            _gameModel.UpdateTick();

            DrawEntity();
            DrawActiveLine();
            DrawLines();
            TimerUpdate();
        }

        #endregion
    }
}
