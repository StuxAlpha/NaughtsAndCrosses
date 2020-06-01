using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using NaughtsAndCrosses;
using System;

namespace NaughtsAndCrossesTest
{
    public class Tests
    {
        //This array contains 8 sets of 3 coordinates that represent the possible scoring lines in the game
        // 3 accross, 3 down, and 2 diagonals
        readonly int[,,] scoringLines = new int[8, 3, 2]
            {{{0,0},{0,1},{0,2}},
             {{1,0},{1,1},{1,2}},
             {{2,0},{2,1},{2,2}},
             {{0,0},{1,0},{2,0}},
             {{0,1},{1,1},{2,1}},
             {{0,2},{1,2},{2,2}},
             {{0,0},{1,1},{2,2}},
             {{2,0},{1,1},{0,2,}}};

        //Returns the number of elements in the passed array equal to the passed character.
        public int CountChar(char[,] gameStateToCount, char charToCount)
        {
            int charCount = 0;

            for (int x = 0; x < gameStateToCount.GetLength(0); x++)
            {
                for (int y = 0; y < gameStateToCount.GetLength(1); y++)
                {
                    if (gameStateToCount[x, y] == charToCount)
                    {
                        charCount++;
                    }
                }
            }

            return charCount;
        }

        //Returns the number of unique plays in the passed game state that would result in a win for the passed symbol.
        int WinningPlayCount(char[,] gameStateToCheck, char playerSymbol)
        {
            int result = 0;
            int numberOfPlayerSymbolInLine = 0;
            bool opponentSymbolInLine = false;
            int[] winningPlayLocation = { 0, 0 };
            List<int[]> uniqueWinningPlayLocations = new List<int[]>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {


                    if (gameStateToCheck[scoringLines[i, j, 0], scoringLines[i, j, 1]] == playerSymbol)
                    {
                        numberOfPlayerSymbolInLine++;
                    }
                    if (gameStateToCheck[scoringLines[i, j, 0], scoringLines[i, j, 1]] != playerSymbol && gameStateToCheck[scoringLines[i, j, 0], scoringLines[i, j, 1]] != ' ')
                    {
                        opponentSymbolInLine = true;
                    }
                    if (gameStateToCheck[scoringLines[i, j, 0], scoringLines[i, j, 1]] == ' ')
                    {
                        winningPlayLocation[0] = scoringLines[i, j, 0];
                        winningPlayLocation[1] = scoringLines[i, j, 1];
                    }
                }

                if (opponentSymbolInLine == false && numberOfPlayerSymbolInLine == 2)
                {
                    if (!uniqueWinningPlayLocations.Contains(winningPlayLocation))
                    {
                        result++;
                        int[] locationToAdd = { winningPlayLocation[0], winningPlayLocation[1] };
                        uniqueWinningPlayLocations.Add(locationToAdd);
                    }
                }

                numberOfPlayerSymbolInLine = 0;
                opponentSymbolInLine = false;
            }
            return result;
        }

        //For the passed symbol, returns true if that player has already won in the passed game state, and false otherwise.
        public bool PlayerHasWon(char[,] gameStateToCheck, char playerSymbol)
        {
            bool result = false; ;
            int numberOfPlayerSymbolInLine = 0;

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

        public List<char> NewCharsInGameStateAfterMove(char[,] gameStateBeforeMove, char[,] gameStateAfterMove)
        {
            List<char> newChars = new List<char>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (gameStateBeforeMove[x, y] != gameStateAfterMove[x, y])
                    {
                        newChars.Add(gameStateAfterMove[x, y]);
                    }
                }
            }

            return newChars;
        }

        //Checks if the element in each respective location for the two passed arrays are identical.
        public bool GameStatesAreEqual(char[,] gameState1, char[,] gameState2)
        {
            bool gameStatesEqual = true;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (gameState1[x, y] != gameState2[x, y])
                    {
                        gameStatesEqual = false;
                    }
                }
            }

            return gameStatesEqual;
        }

        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'O')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'X')]
        public void ReturnsAValidSizedArrayWhenPassedValidSizedArray(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        //Tests that when making a move the computer returns an array of valid size given a valid sized argument.
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            char[,] result;
            result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol);
            if (result.GetLength(0) == 3 && result.GetLength(1) == 3)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'Q', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', '1',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', '0', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', 'x', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', 'o', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'O')]
        [TestCase('O', 'D', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', '9', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', '5',
                  'O', 'O', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  'z', 'X', 'O',
                  'X', ' ', 'X', 'X')]
        public void ThrowsAnExceptionIfPassedArrayContainsAnyElementsOtherThanX_O_OrSpace(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        //Tests that when making a move the computer player returns an array only containing valid elements
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            //char[,] result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol);

            bool passedArrayContainsOnlyO_X_Space = true;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (testGameState[x, y] != ' ' && testGameState[x, y] != 'X' && testGameState[x, y] != 'O')
                    {
                        passedArrayContainsOnlyO_X_Space = false;
                    }
                }
            }

            if (passedArrayContainsOnlyO_X_Space)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Throws<ArgumentException>(() => NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol));
            }
        }

        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'O')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'X')]
        public void ReturnsAnArrayWithAllExistingOAndXLocationsPreservedFromTheArgument(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        //Tests that when making a move the computer player does not undo any previous moves
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            char[,] result;
            result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol);
            bool testIsTrue = true;

            for (int x = 0; x < result.GetLength(0); x++)
            {
                for (int y = 0; y < result.GetLength(1); y++)
                {
                    if (testGameState[x, y] == 'X')
                    {
                        if (result[x, y] != 'X')
                        {
                            testIsTrue = false;
                        }
                    }
                    if (testGameState[x, y] == 'O')
                    {
                        if (result[x, y] != 'O')
                        {
                            testIsTrue = false;
                        }
                    }
                }
            }

            if (testIsTrue)
            {
                Assert.Pass();
            }
        }


        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', ' ')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', '1')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', '0')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'x')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'y')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'p')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'z')]
        public void ReturnsArrayUnalteredIfPassedWrongSizeArray(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            char[,] result;
            result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol);

            Assert.Fail();

        }

        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', ' ')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', '1')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', '0')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'x')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'y')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'p')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'z')]
        public void ThrowsAnExceptionIfSecondArgumentNotOOrX(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            if (playerSymbol != 'X' && playerSymbol != 'O')
            {
                Assert.Throws<ArgumentException>(() => NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol));
            }
            else
            {
                Assert.Pass();
            }
        }

        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'O')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', 'X', 'O',
                  'X', 'X', 'O',
                  'X', 'O', 'X', 'X')]
        public void ReturnsArrayWithExactlyOneLessSpaceUnlessNoEmptySpacesPresent(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            char[,] result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol);

            if (CountChar(testGameState, ' ') == 0)
            {
                Assert.Pass();
            }
            else
            {
                if(CountChar(testGameState, ' ') - 1  == CountChar(result, ' '))
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

            Assert.Fail();
        }

        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'O')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'X')]
        public void AllChangedElementsOfArrayAreChangedToTheSecondArgument(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            char[,] result;
            result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol);

            for (int x = 0; x < result.GetLength(0); x++)
            {
                for (int y = 0; y < result.GetLength(1); y++)
                {
                    if (testGameState[x, y] != result[x, y])
                    {
                        if (result[x, y] != playerSymbol)
                        {
                            Assert.Fail();
                        }
                    }
                }
            }

            Assert.Pass();
        }


        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'O')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'X')]
        public void ReturnsArrayUnalteredIfNoEmptySpacesInArray(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            char[,] result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol);
            if(CountChar(testGameState, ' ') == 0)
            {
                if(result == testGameState)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Pass();
            }
        }

        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'O')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'X')]
        //If can win in one move, always makes that move
        public void PlayerAlwaysMakesAWinningMoveIfAble(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        {
            char[,] altTestGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            char[,] result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(altTestGameState, playerSymbol);

            if(WinningPlayCount(altTestGameState,playerSymbol) > 0)
            {
                if(PlayerHasWon(result, playerSymbol))
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Pass();
            }
        }

        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'X')]
        [TestCase('O', ' ', ' ',
                  ' ', ' ', ' ',
                  'O', ' ', ' ', 'X')]
        [TestCase('X', ' ', ' ',
                  ' ', ' ', ' ',
                  'X', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  ' ', ' ', ' ', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'X')]
        [TestCase('O', ' ', 'O',
                  ' ', ' ', ' ',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', ' ', 'O',
                  ' ', 'X', ' ',
                  'X', ' ', 'O', 'O')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'O')]
        [TestCase('O', 'X', 'X',
                  ' ', 'X', ' ',
                  'O', 'O', 'X', 'X')]
        [TestCase('O', ' ', 'O',
                  'X', 'X', 'O',
                  'X', ' ', 'X', 'X')]
        //If opponent can win in one move, always blocks that move - unless player can win this move
        public void PlayerAlwaysBlockOpponentWinOnNextMoveUnlessPlayerWinsWithThisMove(char aa, char ab, char ac, char ba, char bb, char bc, char ca, char cb, char cc, char playerSymbol)
        {
            char[,] testGameState = new char[3, 3] { { aa, ab, ac }, { ba, bb, bc }, { ca, cb, cc } };
            char[,] result = NaughtsAndCrosses.NaughtsAndCrossesComputerPlayer.PlayNextMove(testGameState, playerSymbol);

            bool resultIsWin =  PlayerHasWon(result, playerSymbol);

            char opponentSymbol = 'X';
            if(playerSymbol == opponentSymbol)
            {
                opponentSymbol = 'O';
            }


            int numberOfWinningMovesForOpponent = WinningPlayCount(testGameState, opponentSymbol);
            bool opponentWinBlocked = false;
            if(WinningPlayCount(result,opponentSymbol) == 0)
            {
                opponentWinBlocked = true;
            }

            //If the opponent has more than 1 winning move, it is not possible to block their win (unless the player wins this move)
            if (numberOfWinningMovesForOpponent == 1)
            {
                if (resultIsWin || opponentWinBlocked)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Pass();
            }
        }


    }
}