using System;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class SudokuForm : Form
    {
        const int BOARD_MARGIN = 100;

        private Button SolveButton;
        private Button RestartButton;
        private Button RandomBoardButton;

        private SudokuBoardControl Sudoku;

        public SudokuForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(800, 800);

            CreateMenu();

            this.Sudoku = new SudokuBoardControl();
            this.Sudoku.Location = new Point(BOARD_MARGIN, BOARD_MARGIN);
            Controls.Add(Sudoku);

            ScreenResize();
            this.Resize += (s, e) => ScreenResize();
        }

        private void CreateMenu()
        {
            this.SolveButton = new Button
            {
                Size = new Size(200, 50),
                Text = "Solve",
            };
            this.SolveButton.Click += (s, e) =>
            {
                this.SolveButton.Enabled = false;
                this.RandomBoardButton.Enabled = false;
                this.Sudoku.ReadOnly = true;

                Solver solver = new Solver(this.Sudoku.Board);
                solver.Solve();

                this.SolveButton.Visible = false;
                this.RestartButton.Visible = true;
                this.RestartButton.Enabled = true;
                this.RestartButton.Visible = true;
            };
            Controls.Add(this.SolveButton);

            this.RestartButton = new Button
            {
                Size = new Size(200, 50),
                Text = "Restart",
                Enabled = false,
                Visible = false
            };
            this.RestartButton.Click += (s, e) =>
            {
                this.Sudoku.ReadOnly = false;
                this.RestartButton.Enabled = false;
                this.RestartButton.Visible = false;
                this.SolveButton.Enabled = true;
                this.SolveButton.Visible = true;
                this.RandomBoardButton.Enabled = true;

                for (int i = 0; i < 81; i++)
                {
                    this.Sudoku.Board[i] = 0;
                }
            };
            Controls.Add(this.RestartButton);

            this.RandomBoardButton = new Button
            {
                Size = new Size(200, 50),
                Text = "Random Board",
            };
            this.RandomBoardButton.Click += (s, e) =>
            {
                int[][] board = RandomBoard.Get();
                for (int i = 0; i < board.Length; i++)
                {
                    for (int j = 0; j < board[0].Length; j++)
                    {
                        this.Sudoku.Board[i + 9 * j] = board[j][i];
                    }
                }
            };
            Controls.Add(this.RandomBoardButton);

        }


        private void ScreenResize()
        {
            SolveButton.Location = new Point(
                this.Width / 2 + 20,
                (BOARD_MARGIN - SolveButton.Height) / 2
            );
            this.RestartButton.Location = this.SolveButton.Location;

            RandomBoardButton.Location = new Point(
                this.Width / 2 - RandomBoardButton.Width - 20,
                (BOARD_MARGIN - RandomBoardButton.Height) / 2
            );

            this.Sudoku.Width = Math.Min(this.Width, this.Height) - 2 * BOARD_MARGIN;
            this.Sudoku.Location = new Point((this.Width - this.Sudoku.Width) / 2, BOARD_MARGIN);


            Invalidate();
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

    }
}
