using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    public class ProbabilityCalculator
    {
        public static double CalculateWinProbability(Dice a, Dice b)
        {
            int wins = 0;
            for (int i = 0; i < a.Faces.Count; i++)
                for (int j = 0; j < b.Faces.Count; j++)
                    if (a.Faces[i] > b.Faces[j]) wins++;

            return wins / 36.0;
        }
    }

    public class HelpTable
    {
        public static void Display(List<Dice> diceList)
        {
            Console.WriteLine("   | " + string.Join(" | ", Enumerable.Range(0, diceList.Count)) + " |");
            Console.WriteLine(new string('-', diceList.Count * 5 + 4));
            for (int i = 0; i < diceList.Count; i++)
            {
                Console.Write($"{i} | ");
                for (int j = 0; j < diceList.Count; j++)
                {
                    if (i == j) Console.Write(" -  |");
                    else
                    {
                        double prob = ProbabilityCalculator.CalculateWinProbability(diceList[i], diceList[j]);
                        Console.Write($"{prob:0.00}|");
                    }
                }
                Console.WriteLine();
            }
        }
    }


}
