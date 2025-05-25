
using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_3
{
    public class Dice
    {
        public List<int> Faces { get; }

        public Dice(string config)
        {
            Faces = config.Split(',').Select(int.Parse).ToList();
            if (Faces.Count != 6)
                throw new ArgumentException("Each dice must have 6 values.");
        }

        public int Roll(int index) => Faces[index];

        public override string ToString()
        {
            return $"[{string.Join(",", Faces)}]";
        }

        public string ToDisplayString()
        {
            return string.Join(",", Faces);
        }
    }



    public class DiceParser
    {
        public static List<Dice> Parse(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("No dice provided. You must specify at least 3 dice.");

            if (args.Length < 3)
                throw new ArgumentException($"Only {args.Length} dice provided. You must provide at least 3 dice.");

            var diceList = new List<Dice>();

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (string.IsNullOrWhiteSpace(arg))
                    throw new ArgumentException($"Dice {i + 1} is empty. Each dice must have 6 comma-separated integers.");

                var parts = arg.Split(',');

                if (parts.Length != 6)
                    throw new ArgumentException($"Dice {i + 1} has {parts.Length} sides. Each dice must have exactly 6 numbers. Problem with: \"{arg}\"");

                var sides = new List<int>();
                foreach (var part in parts)
                {
                    if (!int.TryParse(part, out int number))
                        throw new ArgumentException($"Non-integer value found in dice {i + 1}. All sides must be integers. Problem with: \"{arg}\"");

                    sides.Add(number);
                }

                diceList.Add(new Dice(arg)); // or new Dice(sides) if you update the Dice constructor
            }

            return diceList;
        }
    }
}
