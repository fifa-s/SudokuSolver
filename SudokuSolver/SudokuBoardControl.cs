﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver
{
    // To set size, set Width
    public partial class SudokuBoardControl : Control
    {
        const int CELL_PADDING = 6;

        public SudokuBoard Board = new SudokuBoard(new int[81]);

        private bool _readOnly;
        public bool ReadOnly
        {
            get { return this._readOnly; }
            set
            {
                this._readOnly = value;
                
                foreach (TextBox cell in this.Cells)
                {
                    cell.ReadOnly = this.ReadOnly;
                    if (this._readOnly)
                        cell.BackColor = Color.FromArgb(240, 240, 240);
                    else
                        cell.BackColor = Color.White;
                }
                Invalidate();
            }
        }

        private TextBox[] Cells = new TextBox[81];

        public SudokuBoardControl()
        {
            for (int i = 0; i < 81; i++)
            {
                TextBox cell = new TextBox();
                cell.MaxLength = 1;
                cell.TextAlign = HorizontalAlignment.Center;
                cell.AutoSize = false;
                cell.BorderStyle = BorderStyle.None;
                cell.KeyPress += (s, e) =>
                {
                    if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)) || e.KeyChar == '0')
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        TextBox textBox = (TextBox)s;
                        if (!textBox.ReadOnly)
                            Board[Array.IndexOf(Cells, textBox)] = int.Parse(e.KeyChar.ToString());
                    }
                };
                Cells[i] = cell;
                this.Controls.Add(cell);
            }
            LayoutCells();

            this.Resize += (s, e) => LayoutCells();
            Board.ValueChanged += OnUpdateBoard;
        }

        public void OnUpdateBoard(object sender, ValueChangedEventArgs e)
        {
            if (e.EntireBoard)
            {
                for (int i = 0; i < 81; i++)
                {
                    if (Board[i] == 0)
                        Cells[i].Text = "";
                    else
                        Cells[i].Text = Board[i].ToString();
                }
            }
            else
            {
                if (Board[e.Index] == 0)
                    Cells[e.Index].Text = "";
                else
                    Cells[e.Index].Text = Board[e.Index].ToString();
            }
        }

        private void LayoutCells()
        {
            this.Height = this.Width;
            float boxSize = (float)(this.Width) / 9F;
            Graphics g = Cells[0].CreateGraphics();
            float dpi = g.DpiY;
            int offset = CELL_PADDING + CELL_PADDING / 2;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    TextBox cell = Cells[x + y * 9];
                    cell.Size = new Size(
                        (int)(boxSize - CELL_PADDING * 2),
                        (int)(boxSize - CELL_PADDING * 2)
                    );
                    cell.Location = new Point(
                        (int)(x * boxSize + offset),
                        (int)(y * boxSize + offset)
                    );
                    cell.Font = new Font("Arial", Math.Max((boxSize * 72) / dpi - 12, 1));

                }
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics g = pe.Graphics;
            if (this.ReadOnly)
                g.Clear(Color.FromArgb(240, 240, 240));
            else
                g.Clear(Color.White);
            Pen pen1 = new Pen(Color.Black, CELL_PADDING);
            Pen pen2 = new Pen(Color.Black, CELL_PADDING / 2);

            int offset = CELL_PADDING / 2;
            float boxSize = (float)(this.Width) / 9F - (float)offset / 9F;

            for (int i = 0; i < 10; i++)
            {
                Pen pen = (i % 3 == 0) ? pen1 : pen2;
                int p = (int) (boxSize * i);
                g.DrawLine(pen, new Point(p + offset, 0), new Point(p + offset, this.Height));
                g.DrawLine(pen, new Point(0, p + offset), new Point(this.Height, p + offset));
            }

        }
    }
}
