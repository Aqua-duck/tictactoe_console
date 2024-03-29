﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace tictactoe_console
{
    class Program
	{
		int win = 0;
		//0 = no win
		//1 = player
		//2 = computer
        //3 = draw
		int[,] gridstate = new int[3, 3] { 
			{ 0, 0, 0 }, 
			{ 0, 0, 0 }, 
			{ 0, 0, 0 }
		};
		enum Gridstates {_, X, O};
		//0 = blank
		//1 = player
		//2 = computer

		int[,] rcount = new int[3,2];
		int[,] ccount = new int[3,2];
		int[,] dcount = new int[2,2];

		Random rnd = new Random();

        static void Main(string[] args)
		{
			Program program = new Program();
			String welcome = "Tic Tac Toe!";
			int gridVerified = 0;

			Start:
			foreach (char c in welcome)
			{
				Console.Write(c);
				Thread.Sleep(50);
			}
			Thread.Sleep(1000);
			Console.WriteLine("");
			Console.WriteLine("Press Any Key to Continue");
			Console.ReadKey();
			Console.WriteLine("Let's Play - You are X");
			
			while (program.win == 0)
			{
				gridVerified = 0;
				program.Draw();

				while (gridVerified == 0)
				{
					Console.WriteLine("Make your move");
					Console.Write("Row: ");
					int row = program.userInput()-1;
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
                program.Draw();
				//Checks win condition for player
				program.winCondition();
                program.checkDraw();
                program.clearWinArray();

                //Runs cpu turn
                if (program.win == 0)
                {
                    Console.WriteLine("Computer's turn...");
                    program.Computer();
                    //Checks win condition for cpu
                    program.winCondition();
                    program.checkDraw();
                    program.clearWinArray();
                }
            }

			//Check if win condition met for...
				//Player 1
			if (program.win == 1)
			{
				Console.WriteLine("Player 1 Wins!");
				program.win = 0;
                program.clearWinArray();
				Array.Clear(program.gridstate, 0, 9);
				goto Start;
			}
				//Player 2
			if (program.win == 2)
			{
				Console.WriteLine("Computer Wins!");
				program.win = 0;
                program.clearWinArray();
				Array.Clear(program.gridstate, 0, 9);
				goto Start;
			}
				//Somehow win was set to an integer not between 0 and 2
            if (program.win == 3)
            {
                Console.WriteLine("Draw!");
                program.win = 0;
                program.clearWinArray();
                Array.Clear(program.gridstate, 0, 9);
                goto Start;
            }
			else
			{
				Console.WriteLine("Something fucked up");
				program.win = 0;
				Array.Clear(program.gridstate, 0, 9);
				goto Start;
			}
		}
		void Draw()
		{
			for (int i = 0; i < gridstate.GetLength(0); i++)
			{
				for (int j = 0; j < gridstate.GetLength(1); j++)
				{
					Gridstates grid = (Gridstates)gridstate[i, j];
					Console.Write("{0} ", grid);
				}
				Console.Write(Environment.NewLine, Environment.NewLine);
			}
		}
		void Computer()
		{
			int[,] corners = new int[4, 2]
			{
				{ 0, 0 }, //top left
				{ 0, 2 }, //top right
				{ 2, 0 }, //bottom left
				{ 2, 2 } //bottom right
			};
			//int[,] emptyspaces = new int[0,2];
			List<Coords> emptyspaces = new List<Coords>();

            //Starting move
            Debug.WriteLine("+--------| Computer Awake! |--------+");
			if (checkDraw() == 8)
			{
                Debug.WriteLine("First turn detected");
				if (gridstate[0, 0] == 0 && gridstate[0, 2] == 0 && gridstate[2, 0] == 0 && gridstate[2, 2] == 0)
				{
                    Debug.WriteLine("No corners picked - so I will");
                    int k = rnd.Next(3);
					gridstate[corners[k, 0], corners[k, 1]] = 2;
                    //start = false;
					goto EndCpuTurn;
				}
				else if (gridstate[1, 1] == 0)
				{
                    Debug.WriteLine("Corner pick detected - I will pick center");
					gridstate[1, 1] = 2;
					//start = false;
					goto EndCpuTurn;
				}
			}
            Debug.WriteLine("Not first turn");
            //All subsequent moves
            winCondition();
			for (int i = 0; i < 3; i++)
			{

				/* BLOCK + WIN METHOD*/

				for (int k = 0; k < 2; k++)
				{
                    //Check to see if searching for player or computer win conditions
                    if (k == 0)
                    {
                        Debug.WriteLine("Checking player win conditions...");
                        Debug.WriteLine(" ");
                    }
                    if (k == 1)
                    {
                        Debug.WriteLine("Checking computer win conditions...");
                        Debug.WriteLine(" ");
                    }
                    Debug.WriteLine("Searching for row {0} win condition...", i+1);
                    //Search rcount for player win condition
                    Debug.WriteLine("This is what I see: {0}", rcount[i, k]);
					if (rcount[i, k] == 2)
					{
                        Debug.WriteLine("Found a possible win condition.  Finding empty space...");
						//Search in row i
						for (int j = 0; j < 3; j++)
						{
							//Find empty space
							if (gridstate[i, j] == 0)
							{
                                //Pick empty space
                                Debug.WriteLine("Found empty space!");
								gridstate[i, j] = 2;
								goto EndCpuTurn;
							}
						}
                        Debug.WriteLine("No empty space found");
					}
                    Debug.WriteLine("No row {0} win condition found.", i+1);
                    Debug.WriteLine("Searching for column {0} win condition...", i+1);
                    //Search ccount for player win condition
                    Debug.WriteLine("This is what I see: {0}", ccount[i, k]);
                    if (ccount[i, k] == 2)
					{
                        Debug.WriteLine("Found a possible win condition.  Finding empty space...");
                        //Search column i
                        for (int j = 0; j < 3; j++)
						{
							//Find empty space
							if (gridstate[j, i] == 0)
							{
                                Debug.WriteLine("Found empty space!");
                                //Pick empty space
                                gridstate[j, i] = 2;
								goto EndCpuTurn;
							}
						}
                        Debug.WriteLine("No empty space found");
                    }
                    Debug.WriteLine("No column {0} win condition found.", i+1);
                    Debug.WriteLine("Searching for diagonal win condition...");
                    //Search dcount for player win condition
                    Debug.WriteLine("This is what I see: {0}", dcount[0, k]);
                    if (dcount[0, k] == 2)
					{
                        Debug.WriteLine("Found a possible win condition.  Finding empty space...");
                        //Search first diagonal
                        for (int j = 0; j < 3; j++)
						{ 
							if (gridstate[j, j] == 0)
							{
                                Debug.WriteLine("Found empty space!");
                                //Pick empty space
                                gridstate[j, j] = 2;
								goto EndCpuTurn;
							}
						}
                        Debug.WriteLine("No empty space found");
                    }
                    //Same but for second diagonal
                    Debug.WriteLine("This is what I see: {0}", dcount[1, k]);
                    if (dcount[1,k] == 2)
					{
                        Debug.WriteLine("Found a possible win condition.  Finding empty space...");
                        for (int j = 0; j < 3; j++)
						{
							if (gridstate[2-j,j] == 0)
							{
                                Debug.WriteLine("Found empty space!");
                                gridstate[2 - j, j] = 2;
								goto EndCpuTurn;
							}
						}
                        Debug.WriteLine("No empty space found");
                    }
                    Debug.WriteLine("None found for this i. Searching next i...");
                    Debug.WriteLine("--------------------------------------------------");
				}
			}

            /* Move if no block or win condition */
            //In theory this should only be able to occur on turn 2
            Debug.WriteLine("No win condition found.  Attempting to set up win condition for myself...");
            Debug.WriteLine("Did I play a corner?");
			for (int i = 0; i < 4; i++)
			{
                Debug.WriteLine("Did I play ({0}, {1})?", corners[i,0]+1, corners[i,1]+1);
				//Check if corner is played by cpu
				if (gridstate[corners[i,0],corners[i,1]] == 2)
				{
                    Debug.WriteLine("Corner found!  Attempting to play another corner...");
					//Create random number k between 0 and 3
					int k = rnd.Next(3);
					//While k is equal to i (same index as checked space)
					while (k == i)
					{
						//New random number
						k = rnd.Next(3);
                        if (k != 1)
                        {
                            continue;
                        }
					}
					//Play corner with index k
                    while (gridstate[corners[k, 0], corners[k, 1]] != 0)
                    {
                        k = rnd.Next(3);
                        Debug.WriteLine("This spot was already taken!");
                        if (gridstate[corners[k, 0], corners[k, 1]] == 0)
                        {
                            continue;
                        }
                    }
                    if (gridstate[corners[k, 0], corners[k, 1]] == 0)
                    {
                        gridstate[corners[k, 0], corners[k, 1]] = 2;
                    }
					goto EndCpuTurn;
				}
                Debug.WriteLine("Nope!");
			}
            Debug.WriteLine("I did not play a corner.  Did I play the center?");
			//Check if center is played by cpu
			if (gridstate[1,1] == 2)
			{
                Debug.WriteLine("I played the center!");
                Debug.WriteLine("Let me find all the empty spaces on the board...");
                Debug.WriteLine("Found empty spaces:");
				//Find empty spaces
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						if (gridstate[i,j] == 0)
						{
							emptyspaces.Add(new Coords(i, j));
                            Debug.WriteLine("({0}, {1})", new Coords(i, j).x+1, new Coords(i, j).y+1);
						}
					}
				}
                Debug.WriteLine("Picking from the empty spaces I found...");
				//Chooses random number with range of number of elements in list emptyspaces
				int k = rnd.Next(emptyspaces.Count);
				gridstate[emptyspaces[k].x, emptyspaces[k].y] = 2;
                Debug.WriteLine("I made my move at ({0}, {1})!", emptyspaces[k].x+1, emptyspaces[k].y+1);
				goto EndCpuTurn;
			}
            Debug.WriteLine("Unable to make a move.");
            EndCpuTurn:
                clearWinArray();
                Debug.WriteLine("+--------| Computer Asleep |--------+");
		}
		void winCondition()
		{
			//Crawl through gameboard array rows -> columns
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					//Rows
					if (gridstate[i, j] == 1)
					{
						rcount[i, 0] += 1;
					}
					if (gridstate[i, j] == 2)
					{
						rcount[i, 1] += 1;
					}

					//Columns
					if (gridstate[j, i] == 1)
					{
						ccount[i, 0] += 1;
					}
					if (gridstate[j, i] == 2)
					{
						ccount[i, 1] += 1;
					}
				}
				//Diagonals
				if (gridstate[i, i] == 1)
				{
					dcount[0, 0] += 1;
				}
				if (gridstate[i, i] == 2)
				{
					dcount[0, 1] += 1;
				}
				if (gridstate[(2 - i), i] == 1)
				{
					dcount[1, 0] += 1;
				}
				if (gridstate[(2 - i), i] == 2)
				{
					dcount[1, 1] += 1;
				}

				//Check Wins
				if (rcount[i,0] == 3 || ccount[i,0] == 3 || dcount[0,0] == 3 || dcount[1, 0] == 3)
				{
					win = 1;
				}
				if (rcount[i,1] == 3 || ccount[i,1] == 3 || dcount[0,1] == 3 || dcount[1, 1] == 3)
				{
					win = 2;
				}
                Debug.WriteLine("|-checkWin-|");
				Debug.WriteLine("|Row {2}: {0} {1}|", rcount[i,0], rcount[i,1], i+1);
				Debug.WriteLine("|Col {2}: {0} {1}|", ccount[i,0], ccount[i,1], i+1);
			}
			Debug.WriteLine("|Dia {2}: {0} {1}|", dcount[0, 0], dcount[0, 1], 1);
			Debug.WriteLine("|Dia {2}: {0} {1}|", dcount[1, 0], dcount[1, 1], 2);
            Debug.WriteLine("|----------|");
        }
        void clearWinArray()
        {
            Array.Clear(rcount, 0, 6);
            Array.Clear(ccount, 0, 6);
            Array.Clear(dcount, 0, 4);
            Debug.WriteLine("Win Condition Arrays cleared");
        }
        int checkDraw()
        {
            int emptyspaces = new int();

            for (int i = 0; i<3; i++)
            {
                for (int j = 0; j<3; j++)
                {
                    if (gridstate[i,j] == 0)
                    {
                        emptyspaces += 1;
                    }
                }
            }
            Debug.WriteLine("Empty Spaces Detected: {0}", emptyspaces);
            if (emptyspaces == 0)
            {
                win = 3;
            }
            return emptyspaces;
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
	public class Coords
	{
		public Coords(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public int x { get; set; }
		public int y { get; set; }
	}
}
