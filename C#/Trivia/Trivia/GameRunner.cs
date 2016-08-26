using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UglyTrivia;

namespace Trivia
{
    public class GameRunner
    {

        private static bool notAWinner;

        public static void Main(String[] args)
        {
            Random rand = (args.Length == 0 ? new Random() : new Random(args[0].GetHashCode()));

            Func<int> getNextDiceRoll = () => rand.Next(5) + 1;
            Func<int> getNextRandomNumber = () => rand.Next(9);

            GameLoop(getNextDiceRoll, getNextRandomNumber, new ConsoleWriter());
        }

        public static void GameLoop(Func<int> getNextDiceRoll, Func<int> getNextRandomNumberBetweenZeroAndNine, IWriter output)
        {
            Game aGame = new Game(output);

            aGame.AddPlayer("Chet");
            aGame.AddPlayer("Pat");
            aGame.AddPlayer("Sue");
            
            
            do
            {

                aGame.roll(getNextDiceRoll());

                if (getNextRandomNumberBetweenZeroAndNine() == 7)
                {
                    notAWinner = aGame.wrongAnswer();
                }
                else
                {
                    notAWinner = aGame.AnswerCorrectly();
                }



            } while (notAWinner);

        }


    }

}

