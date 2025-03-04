
using System.Linq.Expressions;

namespace SudokuSolver
{
    internal class Solver
    {
        private SudokuBoard SudokuControl;
        private int[] SudokuArr = new int[81];

        public Solver(SudokuBoard sudoku)
        {
            this.SudokuControl = sudoku;
        }

        public void Solve()
        {
            this.SudokuControl.ReadOnly = true;

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

        public void SolveAnimate()
        {
            this.SudokuControl.ReadOnly = true;

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
