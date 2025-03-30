using System;
using System.Linq;

namespace SudokuSolver
{
	public class ValueChangedEventArgs : EventArgs
	{
        public ValueChangedEventArgs()
        {
            this.Index = -1;
            this.EntireBoard = true;
        }

        public ValueChangedEventArgs(int index)
		{
			this.Index = index;
			this.EntireBoard = false;
		}

        public int Index { get; private set; }
		public bool EntireBoard { get; private set; }
	}

	public class SudokuBoard
	{
		private int[] m_Values;

		public SudokuBoard(int[] values)
		{
			if (values.Length != 81)
			{
				throw new ArgumentException("values.Length must be 81");
			}
			Values = values;
		}

		public int[] Values
		{
			get { return m_Values; }
			set
			{
				if (value.Any(v => v < 0 || v > 9))
				{
					throw new ArgumentOutOfRangeException("Values must be between 0 and 9");
				}
				m_Values = value;
			}
		}

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

		public int this[int index]
		{
			get => Values[index];
			set
			{
				if (value < 0 || value > 9)
				{
					throw new ArgumentOutOfRangeException("value must be between 0 and 9");
				}
				if (value != Values[index])
				{
					Values[index] = value;
					this.ValueChanged?.Invoke(this, new ValueChangedEventArgs(index));
				}
			}
		}
    }
}