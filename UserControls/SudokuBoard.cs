using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuSolver.UserControls
{
    public class SudokuBoard : Grid
    {
        public static readonly DependencyProperty BoardProperty =
        DependencyProperty.Register(
            nameof(Board),
            typeof(int[][]),
            typeof(SudokuBoard),
            new PropertyMetadata(null, OnBoardChanged));

        public int[][] Board
        {
            get => (int[][])GetValue(BoardProperty);
            set => SetValue(BoardProperty, value);
        }

        public SudokuBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                RowDefinitions.Add(new RowDefinition());
                ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private static void OnBoardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SudokuBoard)d;
            control.BuildCells();
        }

        private void BuildCells()
        {
            Children.Clear();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox tb = new TextBox
                    {
                        Text = Board[row][col] == 0 ? "" : Board[row][col].ToString(),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        FontSize = 20,
                        BorderThickness = new Thickness(
                            col % 3 == 0 ? 2 : 0.5,
                            row % 3 == 0 ? 2 : 0.5,
                            (col == 8) ? 2 : 0.5,
                            (row == 8) ? 2 : 0.5
                        ),
                        BorderBrush = Brushes.Black
                    };

                    SetRow(tb, row);
                    SetColumn(tb, col);

                    tb.TextChanged += (s, e) =>
                    {
                        int modifiedRow = GetRow(tb);
                        int modifiedCol = GetColumn(tb);

                        if (int.TryParse(tb.Text, out int value))
                            Board[modifiedRow][modifiedCol] = value;
                        else
                            Board[modifiedRow][modifiedCol] = 0;
                    };

                    Children.Add(tb);
                }
            }
        }

        public void Refresh(int[][] NewBoard)
        {
            Board = NewBoard;

            BuildCells();
        }
    }
}
