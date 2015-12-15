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
    public class Board
    {
        private int Rows;
        private int Cols;
        private int Score;
        private int LinesFilled;
        private MyTetris currTetris;
        private Label[,] BlockControls;

        static private Brush NoBrush = Brushes.Transparent;
        static private Brush SilverBrush = Brushes.Gray;
        public Board(Grid TetrisGrid)
        {
            Rows = TetrisGrid.RowDefinitions.Count;
            Cols = TetrisGrid.ColumnDefinitions.Count;

            Score = 0;
            LinesFilled = 0;

            BlockControls = new Label[Cols, Rows];
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    BlockControls[i, j] = new Label();
                    BlockControls[i, j].Background = NoBrush;
                    BlockControls[i, j].BorderBrush = SilverBrush;
                    BlockControls[i, j].BorderThickness = new Thickness(1, 1, 1, 1);
                    Grid.SetRow(BlockControls[i, j], j);
                    Grid.SetColumn(BlockControls[i, j], i);
                    TetrisGrid.Children.Add(BlockControls[i, j]);
                }
            }
            currTetris = new MyTetris();
            currTetrisDraw();
        }
        public int getScore() { return Score; }
        public int getLines() { return LinesFilled; }
        private void currTetrisDraw()
        {
            // Where Print??
            Point Position = currTetris.getCurrPosition();
            // What Print??
            Point[] Shape = currTetris.getCurrShape();
            // What Color??
            Brush Color = currTetris.getCurrColor();
            foreach (Point S in Shape)
            {
                BlockControls[(int)(S.X + Position.X) + ((Cols / 2) - 1),
                              (int)(S.Y + Position.Y) + 2].Background = Color;
            }
        }
        private void currTetraminoErase()
        {
            // Where Print??
            Point Position = currTetris.getCurrPosition();
            // What Print??
            Point[] Shape = currTetris.getCurrShape();
            foreach (Point S in Shape)
            {
                BlockControls[(int)(S.X + Position.X) + ((Cols / 2) - 1),
                              (int)(S.Y + Position.Y) + 2].Background = NoBrush;
            }
        }
        private void CheckRows()
        {
            bool Full;
            for (int i = Rows - 1; i > 0; i--)
            {
                Full = true;
                for (int j = 0; j < Cols; j++)
                {
                    if (BlockControls[j, i].Background == NoBrush)
                    {
                        Full = false;
                    }
                }
                if (Full)
                {
                    RemoveRow(i);
                    Score += 10;
                    LinesFilled += 1;
                }
            }
        }
        private void RemoveRow(int row)
        {
            for (int i = row; i > 1; i--)
            {
                for (int j = 0; j < Cols; j++)
                {
                    BlockControls[j, i].Background = BlockControls[j, i - 1].Background;
                }
            }
        }
        public void CurrTetraminoMoveLeft()
        {
            Point Position = currTetris.getCurrPosition();
            Point[] Shape = currTetris.getCurrShape();
            bool move = true;
            currTetraminoErase();
            foreach (Point S in Shape)
            {
                if (((int)(S.X + Position.X) + ((Cols / 2) - 1) - 1) < 0)
                {
                    move = false;
                }
                else if (BlockControls[((int)(S.X + Position.X) + ((Cols / 2) - 1) - 1),
                                        (int)(S.Y + Position.Y) + 2].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetris.moveLeft();
                currTetrisDraw();
            }
            else
            {
                currTetrisDraw();
            }
        }
        public void CurrTetraminoMoveRight()
        {
            Point Position = currTetris.getCurrPosition();
            Point[] Shape = currTetris.getCurrShape();
            bool move = true;
            currTetraminoErase();
            foreach (Point S in Shape)
            {
                if (((int)(S.X + Position.X) + ((Cols / 2) - 1) + 1) >= Cols)
                {
                    move = false;
                }
                else if (BlockControls[((int)(S.X + Position.X) + ((Cols / 2) - 1) + 1),
                                     (int)(S.Y + Position.Y) + 2].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetris.moveRight();
                currTetrisDraw();
            }
            else
            {
                currTetrisDraw();
            }
        }
        public void CurrTetraminoMoveDown()
        {
            Point Position = currTetris.getCurrPosition();
            Point[] Shape = currTetris.getCurrShape();
            bool move = true;
            currTetraminoErase();
            foreach (Point S in Shape)
            {
                if (((int)(S.Y + Position.Y) + 2 + 1) >= Rows)
                {
                    move = false;
                }
                else if (BlockControls[((int)(S.X + Position.X) + ((Cols / 2) - 1)),
                                     (int)(S.Y + Position.Y) + 2 + 1].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetris.moveDown();
                currTetrisDraw();
            }
            else
            {
                currTetrisDraw();
                CheckRows();
                currTetris = new MyTetris();
            }
        }
        public void CurrTetraminoMoveRotate()
        {
            Point Position = currTetris.getCurrPosition();
            Point[] S = new Point[4];
            Point[] Shape = currTetris.getCurrShape();
            bool move = true;
            Shape.CopyTo(S, 0);
            currTetraminoErase();
            for (int i = 0; i < S.Length; i++)
            {
                double x = S[i].X;
                S[i].X = S[i].Y * -1;
                S[i].Y = x;
                if (((int)((S[i].Y + Position.Y) + 2)) > Rows)
                {
                    move = false;
                }
                else if (((int)(S[i].X + Position.X) + ((Cols / 2) - 1)) < 0)
                {
                    move = false;
                }
                else if (((int)(S[i].X + Position.X) + ((Cols / 2) - 1)) >= Rows)
                {
                    move = false;
                }
                else if (BlockControls[((int)(S[i].X + Position.X) + ((Cols / 2) - 1)),
                                        (int)(S[i].Y + Position.Y + 2)].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currTetris.moveRotate();
                currTetrisDraw();
            }
            else
            {
                currTetrisDraw();
            }
        }
    }
}
