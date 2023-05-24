using System;
using System.Windows;
using System.Windows.Threading;

namespace CageGame
{
    /// <summary>
    /// Логика взаимодействия для StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Window
    {
        private MenuModel _model;
        private MenuPresenter _presenter;

        private DispatcherTimer _timer;

        public StartMenu()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TimerUpdate);
            _timer.Interval = TimeSpan.FromMilliseconds(GameModel.FrameLength);

            _model = new MenuModel(Fone);
            _presenter = new MenuPresenter(_model, CountSlider, SpeedSlider, MapSizeSlider, CountText, SpeedText, MapSizeText);
            _presenter.Init();

            _timer.Start();
        }

        private void TimerUpdate(object sender, EventArgs e) 
            => _presenter.DrawEllipses(Fone);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            _model.StartGame();
        }

        private void CountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) 
            => _presenter.ValuesChange();

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) 
            => _presenter.ValuesChange();

        private void MapSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            => _presenter.ValuesChange();

        private void Window_Closed(object sender, EventArgs e)
            => Environment.Exit(0);

        private void Button_Click_1(object sender, RoutedEventArgs e)
            => Environment.Exit(0);

    }
}
