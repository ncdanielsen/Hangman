using System;
using System.Collections.Generic;
using System.IO;

namespace Hangman
{
    class Program
    {
        static string correctWord;
        private static int maxGuesses;
        private static string maskedWord;
        static string[] maskedArray;
        static List<string> guessedChars;
        static Player currentPlayer;

        static void Main(string[] args)
        {
            try
            {
                StartGame();
                PlayGame();
                EndGame();
            }
            catch (Exception e)
            {
                e.ToString();
                Console.WriteLine("Oops, something went wrong");
            }
        }

        private static void StartGame()
        {
            maxGuesses = 1000;
            correctWord = WordPicker();
            guessedChars = new List<string>();
            maskedArray = new string[correctWord.Length];
            for (int i = 0; i < maskedArray.Length; i++)
            {
                maskedArray.SetValue("_", i);
            }
            AskForUsersName();
            Console.Clear();
        }

        private static void PlayGame()
        {
            while (maskedWord != correctWord.ToLower() && guessedChars.Count < maxGuesses)
            {                
                AskForLetter();
                UpdateWord();
            }
        }

        private static bool CheckLetter(string input)
        {
            if (guessedChars.Contains(input))
            {
                Console.WriteLine("You have already guessed that word");
                return true;
            }
            else if (input.Length != 1)
            {
                Console.WriteLine("Only one character");
                return true;
            }
            else if (correctWord.Contains(input) || correctWord.Contains(input.ToUpper()) )
            {
                for (int i = 0; i < correctWord.Length; i++)
                {
                    if (correctWord.ToLower()[i] == input[0])
                    {
                        maskedArray[i] = input;
                        currentPlayer.Score++;
                    }
                }
                guessedChars.Add(input.ToLower());
                Console.WriteLine("Correct");
                return false;
            }
            else
            {
                Console.WriteLine("You guessed wrong");
                guessedChars.Add(input.ToLower());
                return false;
            }
        }

        private static void EndGame()
        {
            Console.Clear();
            if (maskedWord.ToLower() == correctWord.ToLower())
                Console.WriteLine("You Won");
            else
                Console.WriteLine("You lost");
            Console.WriteLine($"Correct word was {correctWord}");
            Console.WriteLine($"Thank you for playing, {currentPlayer.Name}");
            Console.WriteLine($"Your number of guesses is {guessedChars.Count}");
            Console.WriteLine("Your score is {0}", currentPlayer.Score);
            Console.WriteLine("Guessed characters");
            foreach (var item in guessedChars)
            {
                Console.Write(item + " ");
            }
            Console.ReadLine();
        }
        static void DisplayMaskedWord()
        {
            maskedWord = "";
            foreach (string c in maskedArray)
            {
                maskedWord += c;
            }
            Console.WriteLine(maskedWord);
        }

        static void UpdateWord()
        {
            maskedWord = "";
            foreach (string c in maskedArray)
            {
                maskedWord += c;
            }
        }

        static void AskForLetter()
        {
            string input;
            do
            {
                
                DisplayMaskedWord();
                Console.WriteLine("Please enter one character:");
                input = Console.ReadLine();
                Console.Clear();
            } while (CheckLetter(input.ToLower()));            
        }
        static void AskForUsersName()
        {
            Console.WriteLine("What is your name?");
            string input = Console.ReadLine();
            if (input.Length < 2)
            {
                Console.WriteLine("Your name has to be atleast two characters long");
                AskForUsersName();
            }
            else
                currentPlayer = new Player(input);
        }
        static string WordPicker()
        {
            string[] wordArray;

            try
            {
                string filePath = @"C:\IT\Prosjekter\OnlineCourses\Net-Core-Scratch\Hangman\GameFiles\words.txt";
                wordArray = File.ReadAllLines(filePath);
            }
            catch(Exception)
            {
                wordArray = new string[] { "Apple", "Mango", "Ketchup"};   
            }

            Random rng = new Random();
            int number = rng.Next(wordArray.Length);
            return wordArray[number];
        }
    }
}
