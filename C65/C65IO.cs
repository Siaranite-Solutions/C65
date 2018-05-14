using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.HAL;

namespace C65
{
    public class C65IO
    {
		private TextScreen Screen = new TextScreen();
		private Cosmos.HAL.PS2Keyboard Keyboard;

		private const int MaxRows = 24;
		private const int MaxCols = 79;

		private int CursorRow;
		private int CursorColumn;

		public C65IO()
		{
			this.Clear();
			CursorColumn = 0;
			CursorRow = 0;

			this.PrintLine("C65 8-bit MOS 6502 Emulator by Arawn Davies");
			this.PrintLine("Based on CBM11 by Scott Hutter");
			this.PrintLine("65535 bytes free.");
		}

		public void Clear()
		{
			for (int x = 0; x < MaxRows; x++)
			{
				for (int y = 0; y < MaxCols; y++)
				{
					Screen[x, y] = (byte)' ';
				}
			}
			CursorColumn = 0;
			CursorRow = 0;

			Screen.SetCursorPos(CursorColumn, CursorRow);
		}

		public char ReadKey()
		{
			char key = Console.ReadKey().KeyChar;

			if (key == 2408) key = (char)8;
			if (key == 10) key = (char)13;

			return key;
		}

		public void Print(char c)
		{
			switch (c)
			{
				case (char)8:
					{
						if (CursorColumn > 0)
						{
							CursorColumn--;
						}
						break;
					}
				case (char)13:
					{
						CursorColumn = 0;
						CursorRow++;
						break;
					}
				default:
					{
						Screen[CursorColumn, CursorRow] = (byte)c;
						CursorColumn++;
						break;
					}

			}

			if (CursorColumn > MaxCols)
			{
				CursorRow++;
				CursorColumn = 0;
			}

			if (CursorRow > MaxRows)
			{
				CursorRow = MaxRows;
				ScrollDown();
			}

			Screen.SetCursorPos(CursorColumn, CursorRow);
		}

		public void Print(int row, int col, string s)
		{
			if (row > MaxRows || col > MaxCols || row < 0 || col < 0)
				return;

			CursorRow = row;
			CursorColumn = col;

			this.Print(s);
		}

		public void Print(string s)
		{
			for (int x = 0; x < s.Length; x++)
				this.Print(s[x]);
		}

		public void PrintLine(string s)
		{
			this.Print(s);

			this.Print((char)13);
		}

		private void ScrollDown()
		{
			for (int i = 1; i < MaxRows + 1; i++)
				ScrollUp(i);

			for (int y = 0; y <= MaxCols; y++)
				Screen[y, MaxCols] = (byte)' ';
		}

		private void ScrollUp(int no)
		{
			if (no > 0 && no < MaxRows + 1)
			{
				for (int c = 0; c < MaxCols + 1; c++)
				Screen[c, no - 1] = Screen[c, no];
			}
		}

		public void Poke(ushort offset, byte b)
		{
			int row = offset / MaxCols;
			int col = offset % MaxCols;

			Screen[col, row] = b;
		}

		public void CursorUp()
		{
			if (CursorRow > 0) CursorRow--;
		}

		public void CursorResetLine()
		{
			CursorColumn = 0;

			for (int y = 0; y <= MaxCols; y++)
				Screen[y, MaxRows] = (byte)' ';
		}

		public void CursorToEOL()
		{
			for (CursorColumn = MaxCols; CursorColumn >= 0; CursorColumn--)
			{
				if (Screen[CursorColumn, CursorRow] != ' ')
				{ CursorColumn++; break; }
			}
		}


		private string ReadLine()
		{
			return ReadLine(false);
		}

		private string ReadLine(bool UpperCase)
		{
			char c = (char) 0;
			string s = "";

			while (c != 13)
			{

				c = this.ReadKey();

				if (UpperCase) c = UCase(c);

				this.Print(c);

				if (c != 13)
					s = s + c;
			};

			return s;
		}


		private char UCase(char c)
		{
			if (c > 0x60 && c < 0x7b) c = (char)(c - 0x20);
			return c;
		}
	}
}
