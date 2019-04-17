using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace tictactoe_console
{
	class Program
	{
		int win = 0;
		//0 = no win
		//1 = player
		//2 = computer
		int[,] gridstate = new int[3, 3] { 
			{ 0, 0, 0 }, 
			{ 0, 0, 0 }, 
			{ 0, 0, 0 }
		};
		//0 = blank
		//1 = x
		//2 = o
		static void Main(string[] args)
		{
			Program program = new Program();
			String welcome = "Tic Tac Toe!";
			int gridVerified = 0;

			foreach (char c in welcome)
			{
				Console.WriteLine(c);
				Thread.Sleep(50);
			}
			Thread.Sleep(1000);
			Console.WriteLine("");
			Console.WriteLine("Press Any Key to Continue");
			Console.ReadKey();
			Console.WriteLine("Let's Play - You are 1");
			
			while (program.win == 0)
			{
				gridVerified = 0;
				program.Draw();
				while (gridVerified == 0)
				{
					Console.WriteLine("Make your move:");
					Console.Write("Row: ");
					int row = 0;

					/*while (Console.ReadLine().GetType() != typeof(int))
					{
						Console.WriteLine("No");
						Console.Write("Row: ");
					}
					row = Convert.ToInt32(Console.ReadLine());
					*/
					row = program.userInput()-1;
					while (row < 0 || row > 2 || row.GetType() != typeof(int))
					{
						Console.WriteLine("Row must be between 1 and 3");
						row = program.userInput()-1;
					}
					Console.Write("Column: ");
					int column = program.userInput()-1;
					while (column < 0 || column > 2 || column.GetType() != typeof(int))
					{
						Console.WriteLine("Column must be between 1 and 3");
						column = program.userInput()-1;
					}
					if (program.gridstate[row, column] == 0)
					{
						program.gridstate[row, column] = 1;
						gridVerified = 1;
					}
					else
					{
						Console.WriteLine("This space is not available, please choose another");
					}
				}
			}
			if (program.win == 1)
			{
				Console.WriteLine("PLayer 1 Wins!");
			}
			else
			{
				Console.WriteLine("Something fucked up");
			}
		}
		void Draw()
		{
			for (int i = 0; i < gridstate.GetLength(0); i++)
			{
				for (int j = 0; j < gridstate.GetLength(1); j++)
				{
					Console.Write("{0} ", gridstate[i, j]);
				}
				Console.Write(Environment.NewLine, Environment.NewLine);
			}
		}
		void Computer()
		{

		}
		void winCondition()
		{
			for (int i = 0; i < 3; i++)
			{
				int row1 = 0;
				row1 += (int)gridstate.GetValue(i, 0);
				if (row1 == 3)
				{
					win = 1;
				}
				else if (row1 == 6)
				{
					win = 2;
				}
			}
		}
		int userInput()
		{
			bool isInt = false;
			int test = 4;
			while (isInt == false)
			{
				try
				{
					test = Convert.ToInt32(Console.ReadLine());
					isInt = true;
				}
				catch
				{
					Console.WriteLine("You must input an integer");
				}
			}
			return test;
		}
	}
}
