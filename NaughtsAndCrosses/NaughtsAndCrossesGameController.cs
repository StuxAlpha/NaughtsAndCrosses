using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;

namespace NaughtsAndCrosses
{
    static class NaughtsAndCrossesGameController
    {

        public static void StartNaughtsAndCrossesGame()
        {
            char[,] gameState = new char[3, 3];
            char playerSymbol = ' ';
            char opponentSymbol = 'X';

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    gameState[x, y] = ' ';
                }
            }

            Console.WriteLine("Would you like to be Os or Xs?");
            while (playerSymbol == ' ')
            {
                string input = Console.ReadLine();
                if(input[0] == 'X'|| input[0] == 'x' || input[0] == 'O' || input[0] == 'o')
                {
                    playerSymbol = Char.ToUpper(input[0]);
                }
                else
                {
                    Console.WriteLine("Please type O or X.");
                }
            }
            Console.Clear();

            if(playerSymbol == 'X')
            {
                opponentSymbol = 'O';
            }

            Random rng = new Random();
            int coinFlip = rng.Next(2);

            if (coinFlip == 0)
            {
                Console.WriteLine("You are going first! You are " + playerSymbol + "s.");
                PrintGameState(gameState);
                HumanPlayerMove(gameState, playerSymbol);

                Console.Clear();
                Console.WriteLine("");
                PrintGameState(gameState);
                Console.WriteLine("Press ENTER to continue.");
                Console.ReadLine();
                
                Console.Clear();
                Console.WriteLine("Your opponent has taken a turn.");
            }
            else
            {
                Console.WriteLine("Your opponent has gone first!");
            }
            gameState = NaughtsAndCrossesComputerPlayer.PlayNextMove(gameState, opponentSymbol);
            PrintGameState(gameState);
            Console.WriteLine("Press ENTER to continue.");
            Console.ReadLine();

            while (CountChar(gameState, ' ') > 0)
            {

                Console.Clear();
                Console.WriteLine("Spaces left: " + CountChar(gameState, ' '));

                Console.WriteLine("Your turn! You are " + playerSymbol + "s.");
                PrintGameState(gameState);
                HumanPlayerMove(gameState, playerSymbol);

                if(PlayerHasWon(gameState, playerSymbol))
                {
                    Console.Clear();
                    Console.WriteLine("YOU WON!!!");
                    PrintGameState(gameState);
                    break;
                }

                if (CountChar(gameState, ' ') > 0)
                {
                    Console.Clear();
                    Console.WriteLine("Spaces left: " + CountChar(gameState, ' '));

                    Console.WriteLine("Your opponent took a turn.");
                    gameState = NaughtsAndCrossesComputerPlayer.PlayNextMove(gameState, opponentSymbol);
                    PrintGameState(gameState);
                    Console.WriteLine("Press ENTER to continue.");
                    Console.ReadLine();
                    if (PlayerHasWon(gameState, opponentSymbol))
                    {
                        Console.Clear();
                        Console.WriteLine("You lost :(");
                        PrintGameState(gameState);
                        break;
                    }
                }
            }
            if (!PlayerHasWon(gameState, playerSymbol) && !PlayerHasWon(gameState, opponentSymbol))
            {
                Console.Clear();
                Console.WriteLine("It was a draw!");
                PrintGameState(gameState);
            }
        }

        private static void HumanPlayerMove(char[,] gameState, char playerSymbol)
        {
            List<int> playLocations = new List<int>();
            String input;
            int chosenGridNumber = -1;

            for (int y = 0; y < 3; y++)
            {
                for(int x = 0; x < 3; x++)
                {
                    if(gameState[x,y] == ' ')
                    {
                        playLocations.Add(x + 1 + (y * 3));
                    }
                }
            }

            Console.WriteLine("Enter the number of the square you want to play:");
            while (chosenGridNumber < 0)
            {
                input = Console.ReadLine();
                if (int.TryParse(input, out chosenGridNumber))
                {
                    if (!playLocations.Contains(chosenGridNumber))
                    {
                        chosenGridNumber = -1;
                        Console.Clear();
                        Console.WriteLine("Invalid Selection. Choose an empty square.");
                        PrintGameState(gameState);
                        Console.WriteLine("You are " + playerSymbol + ". Enter the number of the square you want to play: ");
                    }
                }
                else
                {
                    chosenGridNumber = -1;
                }
            }

            if (chosenGridNumber <= 3)
            {
                gameState[chosenGridNumber - 1, 0] = playerSymbol;
            }
            else
            {
                if (chosenGridNumber <= 6)
                {
                    gameState[chosenGridNumber - 4, 1] = playerSymbol;
                }
                else
                {
                    gameState[chosenGridNumber - 7, 2] = playerSymbol;

                }
            }
        }

        static void PrintGameState(char[,] gameStateToPrint)
        {
            int cell = 1;
            string rowToPrint = "|";
            for (int y = 0; y < 3; y++)
            {
                Console.WriteLine(cell + "-----" + (cell + 1) + "-----" + (cell + 2) + "------");
                
                Console.WriteLine("|     |     |     |");

                for (int x = 0; x < 3; x++)
                {
                    rowToPrint = rowToPrint + "  " + gameStateToPrint[x, y] + "  " + "|";
                }
                Console.WriteLine(rowToPrint);
                Console.WriteLine("|     |     |     |");
                rowToPrint = "|";
                cell += 3;
            }
            Console.WriteLine("-------------------");
        }

        private static bool PlayerHasWon(char[,] gameStateToCheck, char playerSymbol)
        {
            bool result = false; ;
            int numberOfPlayerSymbolInLine = 0;
            int[,,] scoringLines = new int[8, 3, 2]
            {{{0,0},{0,1},{0,2}},
             {{1,0},{1,1},{1,2}},
             {{2,0},{2,1},{2,2}},
             {{0,0},{1,0},{2,0}},
             {{0,1},{1,1},{2,1}},
             {{0,2},{1,2},{2,2}},
             {{0,0},{1,1},{2,2}},
             {{2,0},{1,1},{0,2,}}};

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameStateToCheck[scoringLines[i, j, 0], scoringLines[i, j, 1]] == playerSymbol)
                    {
                        numberOfPlayerSymbolInLine++;
                    }
                }

                if (numberOfPlayerSymbolInLine == 3)
                {
                    result = true;
                }

                numberOfPlayerSymbolInLine = 0;
            }

            return result;
        }

        public static int CountChar(char[,] gameStateToCount, char charToCount)
        {
            int charCount = 0;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (gameStateToCount[x, y] == charToCount)
                    {
                        charCount++;
                    }
                }
            }

            return charCount;
        }
    }
}
