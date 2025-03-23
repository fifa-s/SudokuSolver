using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SudokuSolver
{
    internal class Solver
    {
        private SudokuBoardControl SudokuControl;
        private int[] SudokuArr = new int[81];

        public Solver(SudokuBoardControl sudoku)
        {
            this.SudokuControl = sudoku;
        }

        public void Solve()
        {
            this.SudokuControl.ReadOnly = true;

            if (!CheckBoard())
                return;

            List<int> solutionIndexes = new List<int>();

            for (int j = 0; j < 81; j++)
            {
                int n = this.SudokuControl.GetAt(j);
                if (n != 0)
                {
                    solutionIndexes.Add(j);
                }
                this.SudokuArr[j] = n;
            }
            
            List<int>[] tried = new List<int>[81];
            for (int j = 0; j < 81; j++)
            {
                tried[j] = new List<int>();
            }
            
            int i = 0;
            while (i < 81)
            {
                if (solutionIndexes.Contains(i))
                {
                    i++;
                    continue;
                }

                List<int> nums = GetPossibleNumbers(i);
                nums.RemoveAll(x => tried[i].Contains(x));
                
                if (nums.Count != 0)
                {
                    this.SudokuArr[i] = nums[0];

                    i++;
                }
                else
                {
                    tried[i].Clear();
                    do
                    {
                        i--;
                    } while (solutionIndexes.Contains(i));

                    tried[i].Add(this.SudokuArr[i]);
                    this.SudokuArr[i] = 0;
                }
            }

            for (int j = 0; j < 81; j++)
            {
                if (!solutionIndexes.Contains(j))
                    this.SudokuControl.SetAt(j, this.SudokuArr[j], true);
            }
        }


        // Same as Solve(), but animated
        public void SolveAnimate()
        {
            this.SudokuControl.ReadOnly = true;


            if (!CheckBoard())
                return;

            List<int> solutionIndexes = new List<int>();

            for (int j = 0; j < 81; j++)
            {
                int n = this.SudokuControl.GetAt(j);
                if (n != 0)
                {
                    solutionIndexes.Add(j);
                }
                this.SudokuArr[j] = n;
            }



            List<int>[] tried = new List<int>[81];
            for (int j = 0; j < 81; j++)
            {
                tried[j] = new List<int>();
            }

            int i = 0;
            while (i < 81)
            {
                if (solutionIndexes.Contains(i))
                {
                    i++;
                    continue;
                }

                List<int> nums = GetPossibleNumbers(i);
                nums.RemoveAll(x => tried[i].Contains(x));

                if (nums.Count != 0)
                {
                    this.SudokuArr[i] = nums[0];
                    this.SudokuControl.SetAt(i, nums[0], true);
                    i++;
                }
                else
                {
                    tried[i].Clear();
                    do
                    {
                        i--;
                    } while (solutionIndexes.Contains(i));

                    tried[i].Add(this.SudokuArr[i]);
                    this.SudokuArr[i] = 0;

                    this.SudokuControl.SetAt(i, 0, true);
                }

                this.SudokuControl.Update();
                Thread.Sleep(1);
                Application.DoEvents();
            }
        }

        // Returns true if board is valid
        private bool CheckBoard()
        {
            int[] board = new int[81];

            for (int i = 0; i < 81; i++)
            {
                board[i] = this.SudokuControl.GetAt(i);
            }

            for (int i = 0; i < 9; i++)
            {
                bool rowDuplicates = board
                    .Skip(i * 9)
                    .Take(9)
                    .GroupBy(x => x)
                    .Where(g => g.Count() > 1 && g.Key != 0)
                    .Any();

                bool colDuplicates = Enumerable.Range(0, 9)
                    .Select(row => board[i + 9 * row])
                    .GroupBy(x => x)
                    .Where(g => g.Count() > 1 && g.Key != 0)
                    .Any();

                bool squareDuplicates = Enumerable.Range(0, 9)
                    .Select(j => {
                        int squareX = (i % 3) * 3;
                        int squareY = (i / 3) * 3;
                        int x = squareX + j % 3;
                        int y = squareY + j / 3;
                        return board[x + y * 9];
                    })
                    .GroupBy(x => x)
                    .Where(g => g.Count() > 1 && g.Key != 0)
                    .Any();

                if (rowDuplicates || colDuplicates || squareDuplicates)
                {
                    MessageBox.Show($"This board is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private List<int> GetPossibleNumbers(int index)
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int row = index / 9;
            int col = index % 9;
            int square_row = row / 3;
            int square_col = col / 3;

            for (int i = 0; i < 9; i++)
            {
                int n;
                // Row
                n = this.SudokuArr[row * 9 + i];
                if (n != 0)
                {
                    numbers.Remove(n);
                }

                // Collumn
                n = this.SudokuArr[col + i * 9];
                if (n != 0)
                {
                    numbers.Remove(n);
                }

                // Square
                int x = square_col * 3 + i % 3;
                int y = square_row * 3 + i / 3;
                n = this.SudokuArr[x + 9 * y];
                if (n != 0)
                {
                    numbers.Remove(n);
                }
            }

            return numbers;
        }


    }
}
