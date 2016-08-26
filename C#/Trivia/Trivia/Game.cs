using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyTrivia
{
    public class Game
    {
        private const int NumberOfCoinsRequiredToWin = 6;
        private const string PopCategory = "Pop";
        private const string ScienceCategory = "Science";
        private const string SportsCategory = "Sports";
        private const string RockCategory = "Rock";
        private const int MaximumNumberOfPlayers = 6;
        private const int MinimumNumberOfPlayers = 2;


        List<string> players = new List<string>();

        int[] boardPosition = new int[MaximumNumberOfPlayers];
        int[] purses = new int[MaximumNumberOfPlayers];

        bool[] inPenaltyBox = new bool[MaximumNumberOfPlayers];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        int currentPlayer = 0;
        bool isGettingOutOfPenaltyBox;

        public Game()
        {
            for (int i = 0; i < 50; i++)
            {
                popQuestions.AddLast("Pop Question " + i);
                scienceQuestions.AddLast(("Science Question " + i));
                sportsQuestions.AddLast(("Sports Question " + i));
                rockQuestions.AddLast(createRockQuestion(i));
            }
        }

        public String createRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public bool isPlayable()
        {
            return (numberOfPlayers() >= MinimumNumberOfPlayers);
        }

        public bool add(String playerName)
        {


            players.Add(playerName);
            boardPosition[numberOfPlayers()] = 0;
            purses[numberOfPlayers()] = 0;
            inPenaltyBox[numberOfPlayers()] = false;

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + players.Count);
            return true;
        }

        public int numberOfPlayers()
        {
            return players.Count;
        }

        public void roll(int roll)
        {
            Console.WriteLine(players[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(players[currentPlayer] + " is getting out of the penalty box");
                    boardPosition[currentPlayer] = boardPosition[currentPlayer] + roll;
                    if (boardPosition[currentPlayer] > 11) boardPosition[currentPlayer] = boardPosition[currentPlayer] - 12;

                    Console.WriteLine(players[currentPlayer]
                            + "'s new location is "
                            + boardPosition[currentPlayer]);
                    Console.WriteLine("The category is " + currentCategory());
                    askQuestion();
                }
                else
                {
                    Console.WriteLine(players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {

                boardPosition[currentPlayer] = boardPosition[currentPlayer] + roll;
                if (boardPosition[currentPlayer] > 11) boardPosition[currentPlayer] = boardPosition[currentPlayer] - 12;

                Console.WriteLine(players[currentPlayer]
                        + "'s new location is "
                        + boardPosition[currentPlayer]);
                Console.WriteLine("The category is " + currentCategory());
                askQuestion();
            }

        }

        private void askQuestion()
        {
            if (currentCategory() == PopCategory)
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (currentCategory() == ScienceCategory)
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (currentCategory() == SportsCategory)
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (currentCategory() == RockCategory)
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }


        private String currentCategory()
        {
            if (boardPosition[currentPlayer] == 0) return PopCategory;
            if (boardPosition[currentPlayer] == 4) return PopCategory;
            if (boardPosition[currentPlayer] == 8) return PopCategory;
            if (boardPosition[currentPlayer] == 1) return ScienceCategory;
            if (boardPosition[currentPlayer] == 5) return ScienceCategory;
            if (boardPosition[currentPlayer] == 9) return ScienceCategory;
            if (boardPosition[currentPlayer] == 2) return SportsCategory;
            if (boardPosition[currentPlayer] == 6) return SportsCategory;
            if (boardPosition[currentPlayer] == 10) return SportsCategory;
            return RockCategory;
        }

        public bool wasCorrectlyAnswered()
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    purses[currentPlayer]++;
                    Console.WriteLine(players[currentPlayer]
                            + " now has "
                            + purses[currentPlayer]
                            + " Gold Coins.");

                    bool winner = HasPlayerNotWon();
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;

                    return winner;
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer == players.Count) currentPlayer = 0;
                    return true;
                }



            }
            else
            {

                Console.WriteLine("Answer was correct!!!!");
                purses[currentPlayer]++;
                Console.WriteLine(players[currentPlayer]
                        + " now has "
                        + purses[currentPlayer]
                        + " Gold Coins.");

                bool winner = HasPlayerNotWon();
                currentPlayer++;
                if (currentPlayer == players.Count) currentPlayer = 0;

                return winner;
            }
        }

        public bool wrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(players[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return true;
        }


        private bool HasPlayerNotWon()
        {
            return !(purses[currentPlayer] == NumberOfCoinsRequiredToWin);
        }
    }

}
