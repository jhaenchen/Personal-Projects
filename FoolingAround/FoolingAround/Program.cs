using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Timer = System.Timers.Timer;

namespace FoolingAround
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                
                Console.WriteLine("Choose your option:\nFinbonacci\nTyping\nHangman");
                var response = Console.ReadLine();
                switch (response.ToLower())
                {
                    case ("fibonacci"):
                        Console.Clear();
                        fibonacci();
                        break;
                    case ("typing"):
                        Console.Clear();
                        typingGame();
                        break;
                    case ("hangman"):
                        Console.Clear();
                        Console.WriteLine("What's the thing to guess?");
                        var toGuess = Console.ReadLine();
                        hangMan(toGuess);
                        break;
                }
            }
            
        }

        private static void fibonacci()
        {
            int howManyIterations = int.Parse(Console.ReadLine());
            var theArray = new double[howManyIterations + 2];
            theArray[0] = 0;
            theArray[1] = 1;
            int counter = 2;
            for (counter = 2; counter < howManyIterations; counter++)
            {
                theArray[counter] = theArray[counter - 1] + theArray[counter - 2];
                Console.WriteLine("{0} + {1} = {2}", theArray[counter - 2], theArray[counter - 1],
                    theArray[counter - 1] + theArray[counter - 2]);
            }
        }
        
        private static void flowGame()
        {
        }

        private static void namePlay()
        {
            Console.Clear();
            Console.WriteLine("Loading dictionary, please wait...");
            string contents =
                new WebClient().DownloadString(@"http://www.mieliestronk.com/corncob_lowercase.txt");
            Console.Clear();
            string[] dictionary = contents.Split(Environment.NewLine.ToCharArray());
            dictionary = dictionary.Where((value, index) => index % 2 == 0).ToArray();
            Array.Sort(dictionary, compareStringsByLength);
            var dictionaryList = dictionary.ToList();
            Console.WriteLine("What's your name?");
            var name = Console.ReadLine();
            var newName = name;
            int counter = 0;
            List<KeyValuePair<string, string>> matches = new List<KeyValuePair<string, string>>();
            
            for (int i = 0; i < 26; i++)
            {
                newName = incrementString(newName);
                foreach (var term in dictionaryList)
                {
                    if (term.Contains(newName))
                    {
                        try
                        {
                            matches.Add(new KeyValuePair<string, string>(newName,term));
                        }
                        catch (System.ArgumentException)
                        {
                            
                            
                        }
                        
                        counter++;
                    }
                }
            }
            Console.WriteLine("Found derivations: " + counter+"\n");
            foreach (var match in matches)
            {
                Console.WriteLine(match.Key+" : "+match.Value+"\n");
            }
            Console.ReadLine();
        }

        private static string incrementString(string t)
        {
            var array = t.ToCharArray();
            for (int i = 0; i < t.Length; i++)
            {
                array[i]++;

            }
            return new string(array);
        }

        private static void hangMan(string title)
        {
            string titleBackup = title;
            var theGuess = string.Empty;
            bool justOne = false;
            for (int i = 0; i < title.Length; i++)
            {
                theGuess += " _ ";
            }
            
            while (true)
            {
                if (justOne)
                {
                    theGuess = string.Empty;
                    for (int i = 0; i < title.Length; i++)
                    {
                        theGuess += " _ ";
                    }
                    justOne = false;
                }
                Console.Clear();
                hangmanDisplayObj.display(hangmanDisplayObj.count);
                Console.WriteLine(theGuess);
                Console.WriteLine("Guess a letter...");
                var letterLine = Console.ReadLine();
                var letter = letterLine[0];
                bool goodGuess = false;
                for (int i = 0; i < title.Length; i++)
                {
                    if (title[i].ToString().ToLower().ToCharArray()[0] == letter.ToString().ToLower().ToCharArray()[0])
                    {
                        goodGuess = true;
                        int underscoreCounter = -1;
                        for (int k = 0; k < theGuess.Length; k++)
                        {
                            if (theGuess[k] == "_".ToCharArray()[0])
                            {
                                underscoreCounter++;
                            }
                            else if (theGuess[k] != " ".ToCharArray()[0])
                            {
                                underscoreCounter++;
                            }
                            if (underscoreCounter == i)
                            {
                                theGuess = theGuess.Remove(k, 1).Insert(k, letter.ToString());
                                break;
                            }
                        }


                    }
                    
                }
                if (!goodGuess)
                {
                    hangmanDisplayObj.display();
                    if (hangmanDisplayObj.count == 7)
                    {
                        Console.Clear();
                        hangmanDisplayObj.display(hangmanDisplayObj.count);
                        Console.WriteLine("Game over man!!!");
                        Console.WriteLine("The answer was: "+title);
                        Console.WriteLine("Enter any key to continue...");
                        Console.ReadLine();
                        hangmanDisplayObj.reset();
                        justOne = true;
                        
                    }
                }
                if (theGuess.Replace(" ","").ToLower().Replace("_","") == title.ToLower().Replace(" ",""))
                {
                    Console.Clear();
                    Console.WriteLine("You got it! The answer was "+title);
                    Console.WriteLine("Type anything to play again...");
                    Console.ReadLine();
                    hangmanDisplayObj.reset();
                    justOne = true;
                }
            }
        }

        private static class hangmanDisplayObj
        {
            private static string[] states = new string[8];
            public static int count = 0;
            static hangmanDisplayObj()
            {
                states[0] = "\n";
                states[1] = "  O \n";
                states[2] = "  O  \n  |\n";
                states[3] = "  O  \n  |/\n";
                states[4] = "  O  \n \\|/\n";
                states[5] = "  O  \n \\|/  \n  | \n";
                states[6] = "  O  \n \\|/  \n  | \n /\n";
                states[7] = "  O  \n \\|/  \n  | \n / \\\n";
            }

            public static void display()
            {
                Console.Write(states[count]+"\n \n \n \n \n");
                count++;
            }

            public static void display(int newCount)
            {
                Console.Write(states[newCount]);
                count = newCount;
            }
            public static void reset()
            {
                count = 0;
            }
        }

        private static int compareStringsByLength(string x, string y)
        {
            if (x.Length < y.Length)
            {
                return -1;
            }
            if (x.Length > y.Length)
            {
                return 1;
            }
            if (x.Length == y.Length)
            {
                return 0;
            }
            return 0;
        }

        private static void typingGame()
        {
            Console.Clear();
            Console.WriteLine("Loading dictionary, please wait...");
            string contents =
                new WebClient().DownloadString(@"http://www.mieliestronk.com/corncob_lowercase.txt");
            Console.Clear();
            string[] dictionary = contents.Split(Environment.NewLine.ToCharArray());
            dictionary = dictionary.Where((value, index) => index%2 == 0).ToArray();
            Array.Sort(dictionary,compareStringsByLength);
            var timer = new Timer();
            var stopwatch = new Stopwatch();
            var score = 0;
            int[] range = new int[2];
            range[0] = 0;
            range[1] = 1000;
            int rangeIncrementer = 1000;
            float secondToWait = 5000;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(string.Format( "Score: {0}",score));
                stopwatch.Reset();
                stopwatch.Start();
                var randomObject = new Random();
                int num = randomObject.Next(range[0],range[1]);
                num = num%dictionary.Length;
                Console.WriteLine(dictionary[num]);
                string response = string.Empty;


                try
                {
                    response = Reader.ReadLine(int.Parse(secondToWait.ToString()));
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("\nSorry, you waited too long.");
                    score--;
                    response = "var:NoResponse";
                    Thread.Sleep(1000);
                }
                if (response.ToLower() == dictionary[num].ToLower())
                {
                    Console.WriteLine("Got it!");
                    score++;
                    secondToWait -= 100;
                    range[0] += rangeIncrementer;
                    range[1] += rangeIncrementer;
                    rangeIncrementer *= 2;
                    Thread.Sleep(2000);
                }
                else if (response.ToLower() != dictionary[num].ToLower() && response != "var:NoResponse")
                {
                    Console.WriteLine("\nNope!");
                    score--;
                    Thread.Sleep(1000);
                }
            }
        }
    }
}