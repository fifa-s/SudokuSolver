using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    class Solver
    {
        private SudokuBoard Board;

        public Solver(SudokuBoard board) {
            Board = board;
        }

        public void Solve()
        {
            SolveRecursive(0);
        }

        public bool SolveRecursive(int index)
        {
            if (index >= 81)
                return true;

            if (Board[index] != 0)
                return SolveRecursive(index + 1);


            for (int num = 1; num <= 9; num++)
            {
                if (IsValid(index, num))
                {
                    Board[index] = num;
                    if (SolveRecursive(index + 1))
                        return true;
                    Board[index] = 0;
                }
            }

            return false;
        }

        private bool IsValid(int index, int number)
        {
            int row = index / 9;
            int col = index % 9;
            int squareRow = row / 3;
            int squareCol = col / 3;

            for (int i = 0; i < 9; i++)
            {
                if (Board[row * 9 + i] == number)
                    return false;

                if (Board[col + i * 9] == number)
                    return false;

                int x = squareCol * 3 + i % 3;
                int y = squareRow * 3 + i / 3;
                if (Board[x + 9 * y] == number)
                    return false;
            }

            return true;
        }

    }
}
