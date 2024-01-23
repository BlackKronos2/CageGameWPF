using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CageGame
{
    public partial class GameWindow : Window
    {
        private DispatcherTimer _timer;
        private DispatcherTimer _secTimer;

        private GameModel _gameModel;

        public GameWindow(GameModel gameModel)
        {
            InitializeComponent();

            var mapSize = gameModel.MapSize;
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

            _gameModel = gameModel;
			DataContext = new GameWPFPresenter(_gameModel, Map);

            GameEvents.OnGameEnd += () => Pause();
            GameEvents.OnGameEnd += () => this.Hide();

            _timer.Start();
            _secTimer.Start();
        }

        private GameWPFPresenter? Presenter => DataContext as GameWPFPresenter;

		private void Resizing(double width, double height)
        {
            Map.Width = width;
            Map.Height = height;

            MapUI.Width = width;
            MapUI.Height = height + (TimerText.Height * 2f);

            this.Width = width + 70;
            this.Height = height + 80;

		}

        private void TimerUpdate(object sender, EventArgs e)
        {
            Map.Children.Clear();
            Presenter.Update();
        }

        private void SecTimerUpdate(object sender, EventArgs e) => _gameModel.TimerPlus();

        private void Pause()
        {
            _secTimer.Stop();
            _timer.Stop();
        }

        private void MouseButDown(object sender, MouseEventArgs e) => Presenter.StartDrawLine();
        private void MouseButUp(object sender, MouseEventArgs e) => Presenter.StopDrawLine();
        private void MouseMove(object sender, MouseEventArgs e) => Presenter.OnDrawLine();

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!Presenter.IsGameEnd)
                Presenter.OpenMenu();
        }

    }
}
