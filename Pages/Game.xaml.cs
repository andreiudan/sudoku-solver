using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuSolver.Pages
{
    public partial class Game : Page
    {
        public Cell[,] Board { get; set; } = new Cell[9, 9];
        private Random rand = new Random();

        public Game()
        {
            InitializeComponent();
            InitializeBoard();
            DataContext = this;
        }

        private async void Game_Loaded(object sender, RoutedEventArgs e)
        {
            btnSolve.IsEnabled = false;

            await Task.Run(GenerateBoard);

            btnSolve.IsEnabled = true;
        }

        private async void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(Solve);
        }
        
        private void InitializeBoard()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Board[row, col] = new Cell { Value = 0 };
                }
            }
        }

        private bool Solve()
        {
            bool isSolved = false;

            for (int row = 0; row < 9 && !isSolved; row++)
            {
                for (int col = 0; col < 9 && !isSolved; col++)
                {
                    if (Board[row, col].Value != 0)
                    {
                        continue;
                    }

                    isSolved = FillCell(row, col);
                }
            }

            return isSolved;
        }

        private void RemoveNumbersFromSolution()
        {
            int numbersRemoved = 0;
            int numbersToRemove = rand.Next(46, 55);
            int maxAttempts = 1000;

            while (numbersRemoved < numbersToRemove && maxAttempts != 0)
            {
                int row = rand.Next(0, 9);
                int col = rand.Next(0, 9);

                if (Board[row, col].Value != 0)
                {
                    int backup = Board[row, col].Value;

                    Board[row, col].Value = 0;
                    int solutions = CountSolutions(0, 0);

                    if (solutions != 1)
                    {
                        Board[row, col].Value = backup;
                        maxAttempts--;
                    }
                    else
                    {
                        numbersRemoved++;
                        maxAttempts = 1000;
                    }
                }
            }
        }

        private int CountSolutions(int row, int col, int limit = 2)
        {
            if (row > 8)
            {
                return 1;
            }

            if (Board[row, col].Value != 0)
            {
                if (col < 8)
                {
                    return CountSolutions(row, col + 1);
                }
                else
                {
                    return CountSolutions(row + 1, 0);
                }
            }

            int solutionCounter = 0;

            for (int i = 1; i <= 9; i++)
            {
                if (IsValidEntry(row, col, i))
                {
                    int nextRow = col < 8 ? row : row + 1;
                    int nextCol = col < 8 ? col : 0;

                    Board[row, col].Value = i;
                    solutionCounter += CountSolutions(nextRow, nextCol);

                    if (solutionCounter >= limit)
                    {
                        Board[row, col].Value = 0;
                        return solutionCounter;
                    }

                    Board[row, col].Value = 0;
                }
            }

            return solutionCounter;
        }

        private void GenerateBoard()
        {
            FillCell(0, 0);
            RemoveNumbersFromSolution();
        }

        private bool FillCell(int row, int col)
        {
            if (row > 8)
            {
                return true;
            }

            if (Board[row, col].Value != 0)
            {
                if (col < 8)
                {
                    return FillCell(row, col + 1);
                }
                else
                {
                    return FillCell(row + 1, 0);
                }
            }

            bool[] triedNums = new bool[10];
            int numsTried = 0;
            bool triedAllNums = false;

            while (!triedAllNums)
            {
                int nextNum = rand.Next(1, 10);

                if (triedNums[nextNum])
                {
                    continue;
                }

                if (IsValidEntry(row, col, nextNum))
                {
                    Board[row, col].Value = nextNum;

                    bool isGoodNum;
                    if (col < 8)
                    {
                        isGoodNum = FillCell(row, col + 1);
                    }
                    else
                    {
                        isGoodNum = FillCell(row + 1, 0);
                    }

                    if (isGoodNum)
                    {
                        return true;
                    }
                    else
                    {
                        Board[row, col].Value = 0;
                        triedNums[nextNum] = true;
                        numsTried++;
                    }
                }
                else
                {
                    triedNums[nextNum] = true;
                    numsTried++;
                }

                if (numsTried >= 9)
                {
                    triedAllNums = true;
                }
            }

            return false;
        }

        private bool IsValidEntry(int row, int col, int num)
        {
            return CheckRow(row, col, num) && CheckCol(row, col, num) && CheckSquare(row, col, num);
        }

        private bool CheckRow(int row, int colInsertedOn, int insertedNum)
        {
            for (int col = 0; col < 9; col++)
            {
                if (Board[row, col].Value == insertedNum && col != colInsertedOn)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckCol(int rowInsertedOn, int col, int insertedNum)
        {
            for (int row = 0; row < 9; row++)
            {
                if (Board[row, col].Value == insertedNum && row != rowInsertedOn)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSquare(int rowInsertedOn, int colInsertedOn, int insertedNum)
        {
            int startRow = rowInsertedOn - (rowInsertedOn % 3);
            int startCol = colInsertedOn - (colInsertedOn % 3);

            for (int row = startRow; row < startRow + 3; row++)
            {
                for (int col = startCol; col < startCol + 3; col++)
                {
                    if (Board[row, col].Value == insertedNum && row != rowInsertedOn && col != colInsertedOn)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
