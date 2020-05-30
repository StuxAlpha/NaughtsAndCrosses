using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace NaughtsAndCrosses
{
    class Program
    {
        static void Main()
        {
            NaughtsAndCrossesGameController.StartNaughtsAndCrossesGame();
            
            /*char[,] gameState = new char[3, 3];
            for (int x = 0; x < gameState.GetLength(0); x++)
            {
                for (int y = 0; y < gameState.GetLength(1); y++)
                {
                    gameState[x, y] = ' ';
                }
            }

            char playerSymbol = 'X';


            for(int i = 0; i < 9; i++)
            {
                gameState = NaughtsAndCrossesComputerPlayer.PlayNextMove(gameState, oOrX);
                PrintGameState(gameState);
                if (oOrX == 'X')
                {
                    oOrX = 'O';
                }
                else
                {
                    oOrX = 'X';
                }

            }

            static void PrintGameState(char[,] gameStateToPrint)
            {
                string rowToPrint = "|";
                Console.WriteLine("-------");
                for (int x = 0; x < gameStateToPrint.GetLength(0); x++)
                {
                    for (int y = 0; y < gameStateToPrint.GetLength(1); y++)
                    {
                        rowToPrint = rowToPrint + gameStateToPrint[x, y] + "|";
                    }
                    Console.WriteLine(rowToPrint);
                    rowToPrint = "|";
                    Console.WriteLine("-------");
                }
            }

            /*int EmptySpaces(string[,] gameStateToCheck)
            {
                int emptySpaces = 0;
                for (int x = 0; x < gameState.GetLength(0); x++)
                {
                    for (int y = 0; y < gameState.GetLength(1); y++)
                    {
                        if(gameState[x, y] == " ")
                        {
                            emptySpaces++;
                        }
                    }
                }
                return emptySpaces;
            }*/
        }
    }
}
