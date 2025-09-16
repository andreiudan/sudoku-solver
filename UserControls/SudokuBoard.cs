using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SudokuSolver.UserControls
{
    public class SudokuBoard : Grid
    {
        public static readonly DependencyProperty BoardProperty =
        DependencyProperty.Register(
            nameof(Board),
            typeof(Cell[,]),
            typeof(SudokuBoard),
            new PropertyMetadata(null, OnBoardChanged));

        public Cell[,] Board
        {
            get => (Cell[,])GetValue(BoardProperty);
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
            control.Board = (Cell[,])e.NewValue;
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

                    tb.SetBinding(TextBox.TextProperty, new Binding("Value")
                    {
                        Source = Board[row, col],
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });

                    Children.Add(tb);
                }
            }
        }
    }
}
