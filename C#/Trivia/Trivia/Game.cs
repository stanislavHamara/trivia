using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Trivia;

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
        private const string AnswerWasCorrectMessage = "Answer was correct!!!";
        private const string AnswerWasIncorrectMessage = "Question was incorrectly answered";
        private const int FinalBoardPosition = 11;
        
        private readonly IWriter _output;

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

        public Game(IWriter output)
        {
            _output = output;

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

        public bool AddPlayer(String playerName)
        {


            players.Add(playerName);
            boardPosition[numberOfPlayers()] = 0;
            purses[numberOfPlayers()] = 0;
            inPenaltyBox[numberOfPlayers()] = false;

            _output.WriteLine(playerName + " was added");
            _output.WriteLine("They are player number " + players.Count);
            return true;
        }

        public int numberOfPlayers()
        {
            return players.Count;
        }

        public void roll(int roll)
        {
            _output.WriteLine(players[currentPlayer] + " is the current player");
            _output.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    _output.WriteLine(players[currentPlayer] + " is getting out of the penalty box");
                    boardPosition[currentPlayer] = boardPosition[currentPlayer] + roll;
                    if (boardPosition[currentPlayer] > FinalBoardPosition) boardPosition[currentPlayer] = boardPosition[currentPlayer] - 12;

                    _output.WriteLine(players[currentPlayer]
                            + "'s new location is "
                            + boardPosition[currentPlayer]);
                    _output.WriteLine("The category is " + currentCategory());
                    askQuestion();
                }
                else
                {
                    _output.WriteLine(players[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {

                boardPosition[currentPlayer] = boardPosition[currentPlayer] + roll;
                if (boardPosition[currentPlayer] > FinalBoardPosition) boardPosition[currentPlayer] = boardPosition[currentPlayer] - 12;

                _output.WriteLine(players[currentPlayer]
                        + "'s new location is "
                        + boardPosition[currentPlayer]);
                _output.WriteLine("The category is " + currentCategory());
                askQuestion();
            }

        }

        private void askQuestion()
        {
            if (currentCategory() == PopCategory)
            {
                _output.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (currentCategory() == ScienceCategory)
            {
                _output.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (currentCategory() == SportsCategory)
            {
                _output.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (currentCategory() == RockCategory)
            {
                _output.WriteLine(rockQuestions.First());
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

        public bool AnswerCorrectly()
        {
            if (inPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    _output.WriteLine(AnswerWasCorrectMessage);
                    purses[currentPlayer]++;
                    _output.WriteLine(players[currentPlayer]
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

                _output.WriteLine(AnswerWasCorrectMessage);
                purses[currentPlayer]++;
                _output.WriteLine(players[currentPlayer]
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
            _output.WriteLine(AnswerWasIncorrectMessage);
            _output.WriteLine(players[currentPlayer] + " was sent to the penalty box");
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
