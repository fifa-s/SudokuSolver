using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
	public class ValueChangedEventArgs : EventArgs
	{
		public ValueChangedEventArgs(int index)
		{
			this.Index = index;
		}

		public int Index { get; private set; }
	}

	class SudokuBoard
	{
		private int[] m_anValues;

		public SudokuBoard(int[] values)
		{
			m_anValues = values;
		}

		public event EventHandler<ValueChangedEventArgs> ValueChanged;

		public int this[int index]
		{
			get => m_anValues[index];
			set
			{
				if (value != m_anValues[index])
				{
					m_anValues[index] = value;
					this.ValueChanged?.Invoke(this, new ValueChangedEventArgs(index));
				}
			}
		}
	}
}