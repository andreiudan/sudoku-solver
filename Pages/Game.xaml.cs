using System.Windows;
using System.Windows.Controls;

namespace SudokuSolver.Pages
{
    public partial class Game : Page
    {
        public int[][] Board { get; set; }

        Random rand = new Random();

        public Game()
        {
            InitializeComponent();
        }

        private async void Game_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => GenerateBoard());
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void GenerateBoard()
        {
            Task.Delay(3000).Wait();

            Board = [ new int[9],
                    new int[9],
                    new int[9],
                    new int[9],
                    new int[9],
                    new int[9],
                    new int[9],
                    new int[9],
                    new int[9]];

            FillCell(0, 0);
        }

        private bool FillCell(int row, int col)
        {
            if (row > 8)
            {
                return true;
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

                if (CheckRow(row, col, nextNum) && CheckCol(row, col, nextNum) && CheckSquare(row, col, nextNum))
                {
                    Board[row][col] = nextNum;

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
                        Board[row][col] = 0;
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

        private bool CheckRow(int row, int colInsertedOn, int insertedNum)
        {
            for (int col = 0; col < 9; col++)
            {
                if (Board[row][col] == insertedNum && col != colInsertedOn)
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
                if (Board[row][col] == insertedNum && row != rowInsertedOn)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSquare(int rowInsertedOn, int colInsertedOn, int insertedNum)
        {
            int startRow = rowInsertedOn - (rowInsertedOn % 3) + 1;
            int startCol = colInsertedOn - (colInsertedOn % 3) + 1;

            for (int row = startRow; row < 3; row++)
            {
                for (int col = startCol; col < 3; col++)
                {
                    if (Board[row][col] == insertedNum && row != rowInsertedOn && col != colInsertedOn)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
