using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace NaughtsAndCrosses
{
    public static class NaughtsAndCrossesComputerPlayer
    {
        //Array containing the line coordinates for all 8 lines within the 3x3 game grid.
        //Used by several methods for checking for winning game states.
        private static readonly int[,,] scoringLines = new int[8, 3, 2]
            {{{0,0},{0,1},{0,2}},
             {{1,0},{1,1},{1,2}},
             {{2,0},{2,1},{2,2}},
             {{0,0},{1,0},{2,0}},
             {{0,1},{1,1},{2,1}},
             {{0,2},{1,2},{2,2}},
             {{0,0},{1,1},{2,2}},
             {{2,0},{1,1},{0,2,}}};

        //Checks the passed array is 3x3 and only contains Xs, Os and spaces, and checks if the player symbol is an X or O.
        //Returns True if all the above is true.
        //Throws an exception otherwise, with a message about what argument was invalid.
        static private bool ArgumentsAreValid(char[,] gameState, char playerSymbol)
        {
            if (playerSymbol != 'X' && playerSymbol != 'O')
            {
                throw new ArgumentException(String.Format("Invalid Player Symbol. Given as '" + playerSymbol + "'. Must be 'X' or 'O'."));
            }
            else
            {
                if (gameState.GetLength(0) == 3 && gameState.GetLength(0) == 3)
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if (gameState[x, y] != 'X' && gameState[x, y] != 'O' && gameState[x, y] != ' ')
                            {
                                throw new ArgumentException(String.Format("Invalid Game State content. All symbols in game state myst be 'X', 'O', or space."));
                            }
                        }
                    }
                else
                {
                    throw new ArgumentException(String.Format("Invalid Game State size. Given as " + gameState.GetLength(0) + "x" + gameState.GetLength(1) + ". Must be 3x3."));
                }
            }

            return true;
        }

        //Returns the number of times the specificer character appears in the given array.
        static private int CountChar(char[,] gameState, char charToCount)
        {
            int charCount = 0;

            for (int x = 0; x < gameState.GetLength(0); x++)
            {
                for (int y = 0; y < gameState.GetLength(1); y++)
                {
                    if (gameState[x, y] == charToCount)
                    {
                        charCount++;
                    }
                }
            }

            return charCount;
        }

        //Returns the indexes of a location that would be a winning move for the given player symbol, for the given game state.
        //Returns null if there is no winning move.
        static private int[] WinningPlayLocation(char[,] gameState, char playerSymbol)
        {
            char opponentSymbol = 'O';
            if (playerSymbol == 'O')
            {
                opponentSymbol = 'X';
            }

            for (int i = 0; i < 8; i++)
            {
                int numberOfPlayerSymbolInLine = 0;
                bool opponentSymbolInLine = false;
                int[] winningPlayLocation = new int[2];


                for (int j = 0; j < 3; j++)
                {
                    if (gameState[scoringLines[i, j, 0], scoringLines[i, j, 1]] == playerSymbol)
                    {
                        numberOfPlayerSymbolInLine++;
                    }
                    if (gameState[scoringLines[i, j, 0], scoringLines[i, j, 1]] == opponentSymbol)
                    {
                        opponentSymbolInLine = true;
                    }
                    if (gameState[scoringLines[i, j, 0], scoringLines[i, j, 1]] == ' ')
                    {
                        winningPlayLocation[0] = scoringLines[i, j, 0];
                        winningPlayLocation[1] = scoringLines[i, j, 1];
                    }
                }

                if (opponentSymbolInLine == false && numberOfPlayerSymbolInLine == 2)
                {
                    return winningPlayLocation;
                }
            }

            return null;
        }

        static public char[,] PlayNextMove(char[,] gameState, char playerSymbol)
        {
            bool moveMade = false;
            int[] movePosition;
            char opponentSymbol = 'O';
            if(playerSymbol == 'O')
            {
                opponentSymbol = 'X';
            }


            //Tests whether the passed arguments are valid
            if(!ArgumentsAreValid(gameState, playerSymbol))
            {
                Console.WriteLine("Invalid Arguments.");

                return gameState;
            }

            //Copies the passed game state into a new array to return
            char[,] result = new char[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    result[x, y] = gameState[x, y];
                }
            }

            //Makes a winning move if possible
            movePosition = WinningPlayLocation(result, playerSymbol);
            if(movePosition != null)
            {
                result[movePosition[0], movePosition[1]] = playerSymbol;
                moveMade = true;
            }

            //Block opponent's winning move
            if (moveMade == false)
            {
                movePosition = WinningPlayLocation(result, opponentSymbol);
                if (movePosition != null)
                {
                    result[movePosition[0], movePosition[1]] = playerSymbol;
                    moveMade = true;
                }
            }

            //Play in a free space that doesn't have an opponent's symbol in the same line
            /*if (moveMade == false)
            {
                bool[,] validLocations = new bool[3, 3];
                for( int x = 0; x < 3; x++)
                {
                    for(int y = 0; y < 3; y++)
                    {
                        validLocations[x, y] = true;
                    }
                }
                
                for (int i = 0; i < 8; i++)
                {
                    bool opponentSymbolInLine = false;

                    for (int j = 0; j < 3; j++)
                    {
                        if (gameState[scoringLines[i, j, 0], scoringLines[i, j, 1]] == opponentSymbol)
                        {
                            validLocations[scoringLines[i, j, 0], scoringLines[i, j, 1]] = false;
                            opponentSymbolInLine = true;
                        
                        }

                        if (gameState[scoringLines[i, j, 0], scoringLines[i, j, 1]] == playerSymbol)
                        {
                            validLocations[scoringLines[i, j, 0], scoringLines[i, j, 1]] = false;
                        }

                    }

                    for (int j = 0; j< 3; j++)
                    {
                        if (gameState[scoringLines[i, j, 0], scoringLines[i, j, 1]] == ' ' && opponentSymbolInLine == true)
                        {
                            validLocations[scoringLines[i, j, 0], scoringLines[i, j, 1]] = false;
                        }
                    }
                }

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {

                        if (moveMade == false)
                        {
                            if (validLocations[x, y] == true)
                            {
                                result[x, y] = playerSymbol;
                                moveMade = true;
                            }
                        }
                    }
                }
            }*/

            //Play in any free space
            if (moveMade == false)
            {
                if (CountChar(gameState, ' ') > 0)
                {
                    int[] freeSpace = new int[2];
                    List<int[]> freeSpaces = new List<int[]>();
                    int chosenSpaceIndex;

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if (result[x, y] == ' ')
                            {
                                freeSpace[0] = x;
                                freeSpace[1] = y;
                                freeSpaces.Add(freeSpace);
                            }
                        }
                    }

                    Random rng = new Random();
                    chosenSpaceIndex = rng.Next(freeSpaces.Count);
                    result[freeSpaces[chosenSpaceIndex][0], freeSpaces[chosenSpaceIndex][1]] = playerSymbol;
                }
            }

            return result;
        }
    }
}
