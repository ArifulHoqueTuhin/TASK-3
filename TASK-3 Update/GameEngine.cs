
using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_3
{
    public class GameManager
    {
        private List<Dice> _diceList;

        public GameManager(List<Dice> diceList)
        {
            _diceList = diceList;
        }

        public void Start()
        {
            Console.WriteLine("Let's determine who makes the first move.");

            var protocol = new FairNumberProtocol();
            protocol.Init(2);

            Console.WriteLine($"I selected a random value in the range 0..1 (HMAC={protocol.Hmac.ToUpper()}).");
            Console.WriteLine("Try to guess my selection.");
            Console.WriteLine("0 - 0");
            Console.WriteLine("1 - 1");
            Console.WriteLine("X - exit");
            Console.WriteLine("? - help");

            int userGuess;
            while (true)
            {
                Console.Write("Your selection: ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "x") return;
                if (input == "?") { HelpTable.Display(_diceList); continue; }
                if (input == "0" || input == "1") { userGuess = int.Parse(input); break; }

                Console.WriteLine("Invalid input. Please enter 0, 1, X or ?.");
            }

            Console.WriteLine($"My selection: {protocol.ComputerNumber} (KEY={Convert.ToBase64String(protocol.Key)}).");
            bool userFirst = userGuess == protocol.ComputerNumber;

            int userIndex, compIndex;

            if (userFirst)
            {
                Console.WriteLine("You make the first move and choose your dice:");
                for (int i = 0; i < _diceList.Count; i++) Console.WriteLine($"{i} - {_diceList[i]}");
                Console.WriteLine("X - exit\n? - help");

                while (true)
                {
                    Console.Write("Your selection: ");
                    string? diceInput = Console.ReadLine()?.Trim().ToLower();

                    if (diceInput == "x") return;
                    if (diceInput == "?") { HelpTable.Display(_diceList); continue; }
                    if (int.TryParse(diceInput, out userIndex) && userIndex >= 0 && userIndex < _diceList.Count) break;

                    Console.WriteLine("Invalid dice index. Try again.");
                }

                compIndex = Enumerable.Range(0, _diceList.Count).First(i => i != userIndex);
                Console.WriteLine($"I make the second move and choose the {_diceList[compIndex]} dice.");
            }
            else
            {
                compIndex = RandomGenerator.SecureRandomInt(_diceList.Count);
                Console.WriteLine($"I make the first move and choose the {_diceList[compIndex]} dice.");

                Console.WriteLine("Choose your dice:");
                for (int i = 0; i < _diceList.Count; i++)
                    if (i != compIndex) Console.WriteLine($"{i} - {_diceList[i]}");
                Console.WriteLine("X - exit\n? - help");

                while (true)
                {
                    Console.Write("Your selection: ");
                    string? diceInput = Console.ReadLine()?.Trim().ToLower();

                    if (diceInput == "x") return;
                    if (diceInput == "?") { HelpTable.Display(_diceList); continue; }
                    if (int.TryParse(diceInput, out userIndex) &&
                        userIndex >= 0 && userIndex < _diceList.Count &&
                        userIndex != compIndex) break;

                    Console.WriteLine("Invalid dice index. Try again.");
                }

                Console.WriteLine($"You choose the {_diceList[userIndex]} dice.");
            }


            int userRoll = -1, compRoll = -1;

            if (userFirst)
            {
                userRoll = PerformRoll("your", _diceList[userIndex]);
                compRoll = PerformRoll("my", _diceList[compIndex]);
            }
            else
            {
                compRoll = PerformRoll("my", _diceList[compIndex]);
                userRoll = PerformRoll("your", _diceList[userIndex]);
            }

            if (userRoll > compRoll)
                Console.WriteLine($"You win ({userRoll} > {compRoll})!");
            else if (userRoll < compRoll)
                Console.WriteLine($"Computer wins ({compRoll} > {userRoll})!");
            else
                Console.WriteLine("It's a draw.");
        }

        private int PerformRoll(string who, Dice dice)
        {
            Console.WriteLine($"It's time for {who} roll.");
            var protocol = new FairNumberProtocol();
            protocol.Init(6);

            Console.WriteLine($"I selected a random value in the range 0..5 (HMAC={protocol.Hmac.ToUpper()}).");
            Console.WriteLine("Add your number modulo 6.");
            for (int i = 0; i < 6; i++) Console.WriteLine($"{i} - {i}");
            Console.WriteLine("X - exit\n? - help");

            int userAdd;
            while (true)
            {
                Console.Write("Your selection: ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "x") Environment.Exit(0);
                if (input == "?") { HelpTable.Display(_diceList); continue; }
                if (int.TryParse(input, out userAdd) && userAdd >= 0 && userAdd < 6) break;

                Console.WriteLine("Invalid input. Enter 0 to 5, X to exit, or ? for help.");
            }

            int compNumber = protocol.ComputerNumber;
            int fairResult = (compNumber + userAdd) % 6;

            Console.WriteLine($"My number is {compNumber} (KEY={Convert.ToBase64String(protocol.Key)}).");
            Console.WriteLine($"The fair number generation result is {compNumber} + {userAdd} = {fairResult} (mod 6).");
            int roll = dice.Roll(fairResult);
            Console.WriteLine($"{who.First().ToString().ToUpper() + who.Substring(1)} roll result is {roll}.");

            return roll;
        }
    }
}
