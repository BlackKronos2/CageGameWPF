using System;
using System.Windows;
using System.Windows.Threading;

namespace CageGame
{
    public partial class StartMenu : Window
    {
        private MenuModel _model;
        private DispatcherTimer _timer;

        public StartMenu()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerUpdate);
            _timer.Interval = TimeSpan.FromMilliseconds(GameModel.FrameLength);

            GameEvents.OnGameStart += (gameModel) => this.Hide();

            _model = new MenuModel(Fone);
            _timer.Start();
        }

        private MenuPresenter? Presenter => DataContext as MenuPresenter;

        private void TimerUpdate(object sender, EventArgs e)
        {
            if (Presenter != null)
                Presenter.DrawEllipses(Fone, _model);
        }

        private void Window_Closed(object sender, EventArgs e) => Environment.Exit(0);

        private void Button_Click_1(object sender, RoutedEventArgs e) => Environment.Exit(0);

    }
}
