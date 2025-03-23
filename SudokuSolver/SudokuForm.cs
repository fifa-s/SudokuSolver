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

        private CheckBox AnimateCheckbox;

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
                this.SolveButton.Visible = false;
                this.RestartButton.Visible = true;
                this.RandomBoardButton.Enabled = false;

                Solver solver = new Solver(this.Sudoku);
                if (this.AnimateCheckbox.Checked) 
                    solver.SolveAnimate();
                else
                    solver.Solve();

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
                    this.Sudoku.SetAt(i, 0);
                }
            };
            Controls.Add(this.RestartButton);

            this.RandomBoardButton = new Button
            {
                Size = new Size(150, 25),
                Text = "Random Board",
            };
            this.RandomBoardButton.Click += (s, e) =>
            {
                int[][] board = RandomBoard.Get();
                for (int i = 0; i < board.Length; i++)
                {
                    for (int j = 0; j < board[0].Length; j++)
                    {
                        this.Sudoku.SetAt(i + 9 * j, board[j][i]);
                    }
                }
            };
            Controls.Add(this.RandomBoardButton);

            this.AnimateCheckbox = new CheckBox
            {
                Text = "Animate"
            };
            Controls.Add(this.AnimateCheckbox);
        }


        private void ScreenResize()
        {
            this.SolveButton.Location = new Point(
                (this.Width - this.SolveButton.Width) / 2,
                (BOARD_MARGIN - this.SolveButton.Height) / 2
            );
            this.RestartButton.Location = this.SolveButton.Location;

            this.Sudoku.Width = Math.Min(this.Width, this.Height) - 2 * BOARD_MARGIN;
            this.Sudoku.Location = new Point((this.Width - this.Sudoku.Width) / 2, BOARD_MARGIN);

            Point p = this.Sudoku.Location;
            p.Y -= BOARD_MARGIN - this.RandomBoardButton.Height;
            this.RandomBoardButton.Location = p;

            p.Y += this.AnimateCheckbox.Height;
            this.AnimateCheckbox.Location = p;

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
