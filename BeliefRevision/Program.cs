using System;

namespace BeliefRevision
{
	class Program
	{
		private static BeliefRevision _beliefRevision = new BeliefRevision();

        static void Main(string[] args)
        {

            var path = @"..\..\..\test1.txt";

            string[] hornClauses = System.IO.File.ReadAllLines(path);
            string sentence = "q | !r";

            // Validate the input: file and consol input
            if (!_beliefRevision.ValidateInput(hornClauses, sentence))
            {
                Console.WriteLine("");
            }

            Console.WriteLine("The KB before resolution:");
            _beliefRevision._clauses.ForEach(clause => Console.WriteLine(clause));

            // Do resolution 
            if (_beliefRevision.Resolution())
            {
                Console.WriteLine("The sentence {0} is logical entailed in the knowledge base. Performing expansion.", sentence);
                _beliefRevision.Expansion(_beliefRevision._sentence);
            }
            else
            {
                Console.WriteLine("The sentence {0} is not logical entailed in the knowledge base. Performing revision.", sentence);
                _beliefRevision.Revision(_beliefRevision._sentence);
            }

            Console.WriteLine("The updated KB:");
            _beliefRevision._clauses.ForEach(clause => Console.WriteLine(clause));

            Console.ReadLine();
        }
	}
}
