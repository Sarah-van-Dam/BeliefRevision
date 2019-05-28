using System;

namespace BeliefRevision
{
	class Program
	{
		private static BeliefRevision _beliefRevision = new BeliefRevision();

        static void Main(string[] args)
        {
            // Load the belief base
            Console.WriteLine("Please enter the name of the file containing the Belief Base. Example: test1");
            string fileName = Console.ReadLine();
            var path = @"..\..\..\BeliefBases\"+fileName+".txt";
            string[] hornClauses = System.IO.File.ReadAllLines(path);

            // Input the sentence to compare against
            Console.WriteLine("Please enter the sentence to compare against. Example: q | !r");
            string sentence = Console.ReadLine();

            // Validate the input: file and consol input
            if (!_beliefRevision.ValidateInput(hornClauses, sentence))
            {
                Console.WriteLine("");
            }

            Console.WriteLine("\nThe KB before resolution:");
            _beliefRevision._clauses.ForEach(clause => Console.WriteLine(clause));

            string validatedSentence = _beliefRevision._sentence;

            // Do resolution 
            if (_beliefRevision.Resolution(_beliefRevision._clauses, validatedSentence))
            {
                Console.WriteLine("\nThe sentence {0} is logical entailed in the knowledge base! Performing expansion...", validatedSentence);
                _beliefRevision.Expansion(validatedSentence);
            }
            else
            {
                Console.WriteLine("\nThe sentence {0} is not logical entailed in the knowledge base! Performing revision...", validatedSentence);
                _beliefRevision.Revision(validatedSentence);
            }

            Console.WriteLine("\nThe updated KB:");
            _beliefRevision._clauses.ForEach(clause => Console.WriteLine(clause));

            Console.WriteLine("\nPress the Enter key to exit program...");
            Console.ReadLine();
        }
	}
}
