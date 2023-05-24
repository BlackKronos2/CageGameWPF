using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CageGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private DispatcherTimer _timer;
        private DispatcherTimer _secTimer;

        private GameModel _gameModel;
        private GameWPFPresenter _gamePresenter;

        public GameWindow(int entitiesCount, float entitiesSpeed, Vector2 mapSize)
        {
            InitializeComponent();
            Resizing(mapSize.X, mapSize.Y);

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerUpdate);
            _timer.Interval = TimeSpan.FromMilliseconds(GameModel.FrameLength);

            _secTimer = new DispatcherTimer();
            _secTimer.Tick += new EventHandler(SecTimerUpdate);
            _secTimer.Interval = TimeSpan.FromSeconds(1);

            Map.MouseLeftButtonDown += new MouseButtonEventHandler(MouseButDown);
            Map.MouseRightButtonDown += new MouseButtonEventHandler(MouseButUp);
            Map.MouseMove += new MouseEventHandler(MouseMove);

            _gameModel = new GameModel(entitiesCount, entitiesSpeed, mapSize);
            _gamePresenter = new GameWPFPresenter(_gameModel, Map, ObjectsCount, TimerText);

            GameEvents.OnGameEnd += () => Pause();
            GameEvents.OnGameEnd += () => this.Hide();

            _timer.Start();
            _secTimer.Start();
        }

        private void Resizing(double width, double height)
        {
            Map.Width = width;
            Map.Height = height;

            MapUI.Width = width;
            MapUI.Height = height + (TimerText.Height * 2f);
        }

        private void TimerUpdate(object sender, EventArgs e)
        {
            Map.Children.Clear();
            _gamePresenter.Update();
        }

        private void SecTimerUpdate(object sender, EventArgs e) => _gameModel.TimerPlus();

        private void Pause()
        {
            _secTimer.Stop();
            _timer.Stop();
        }

        private void MouseButDown(object sender, MouseEventArgs e)
            => _gamePresenter.StartDrawLine();

        private void MouseButUp(object sender, MouseEventArgs e)
            => _gamePresenter.StopDrawLine();

        private void MouseMove(object sender, MouseEventArgs e)
            => _gamePresenter.OnDrawLine();

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!_gamePresenter.isGameEnd)
                _gamePresenter.OpenMenu();
        }

    }
}
