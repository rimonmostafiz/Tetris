using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer;
        Board myBoard;

        public MainWindow()
        {

            InitializeComponent();
        }
        void MainWindow_Initilized(object sender, EventArgs e)
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(GameTick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 700);
            GameStart();
        }

        //Button Click Starts
        private void Button_Start(object sender, RoutedEventArgs e)
        {
            GameStart();
        }

        private void Button_Pause(object sender, RoutedEventArgs e)
        {
            GamePause();
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            GameEnd(); 
        }
        // Button Click Ends

        private void GameStart()
        {
            MainGrid.Children.Clear();
            myBoard = new Board(MainGrid);
            Timer.Start();
        }

        private void GamePause()
        {
            if (Timer.IsEnabled) Timer.Stop();
            else Timer.Start();
        }

        private void GameEnd()
        {
            Close();
        }

        void GameTick(object sender, EventArgs e)
        {

            Score.Text = myBoard.getScore().ToString("000000");
            Line.Text = myBoard.getLines().ToString("000000");
            myBoard.CurrTetraminoMoveDown();
        }
        
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (Timer.IsEnabled) myBoard.CurrTetraminoMoveLeft();
                    break;
                case Key.Right:
                    if (Timer.IsEnabled) myBoard.CurrTetraminoMoveRight();
                    break;
                case Key.Down:
                    if (Timer.IsEnabled) myBoard.CurrTetraminoMoveDown();
                    break;
                case Key.Up:
                    if (Timer.IsEnabled) myBoard.CurrTetraminoMoveRotate();
                    break;
                default:
                    break;
            }
        }
    }
}
