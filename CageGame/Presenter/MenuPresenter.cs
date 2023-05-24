using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CageGame
{
    public sealed class MenuPresenter
    {
        private MenuModel _model;

        private Label _countText;
        private Label _speedText;
        private Label _mapSizeText;

        private Slider _countSlider;
        private Slider _speedSlider;
        private Slider _mapSizeSlider;

        private static Dictionary<MapSize, (Vector2, string)> MapSize = MenuModel.s_mapSize;

        public MenuPresenter(MenuModel model, Slider countSlider, Slider speedSlider, Slider mapSizeSlider, Label countLabel, Label speedLabel, Label mapSizeLabel)
        {
            _model = model;

            _speedSlider = speedSlider;
            _countSlider = countSlider;
            _mapSizeSlider = mapSizeSlider;

            _countSlider.SmallChange = 1;
            _speedSlider.SmallChange = 1;
            _mapSizeSlider.SmallChange = 0;

            _countText = countLabel;
            _speedText = speedLabel;
            _mapSizeText = mapSizeLabel;
        }

        public void Init()
        {
            _countSlider.Maximum = MenuModel.MaxCount;
            _countSlider.Minimum = 1;

            _speedSlider.Maximum = MenuModel.MaxSpeed;
            _speedSlider.Minimum = 1;

            _countSlider.Value = _model.InputCount;
            _speedSlider.Value = _model.SpeedValue;

            _mapSizeSlider.Minimum = 0;
            _mapSizeSlider.Maximum = MapSize.Count - 1;

            ValuesShow();
        }

        private void ValuesShow()
        {
            _countText.Content = $"Количество объектов: {(int)_countSlider.Value}";
            _speedText.Content = $"Скорость объектов: {(int)_speedSlider.Value}";
            _mapSizeText.Content = $"Размер поля: {MapSize[(MapSize)_mapSizeSlider.Value].Item2}";
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

        public void DrawEllipses(Canvas menuMap)
        {
            menuMap.Children.Clear();

            foreach (Entity entity in _model.MenuEntities)
            {
                entity.Update();
                DrawEllipse(menuMap, entity);
            }
        }

        public void ValuesChange()
        {
            _model.InputCount = (int)_countSlider.Value;
            _model.SpeedValue = (int)_speedSlider.Value;
            _model.CurrentMapSize = (MapSize)_mapSizeSlider.Value;
            ValuesShow();
        }
    }
}
