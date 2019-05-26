using System;

namespace BeliefRevision
{
	class Program
	{
		private static BeliefRevision _beliefRevision = new BeliefRevision();

        static void Main(string[] args)
        {
            // Validate the input: file and consol input
            if (!_beliefRevision.ValidateInput(args))
            {
                Console.WriteLine("");
            }

            // Do resolution 
            if (_beliefRevision.Resolution())
            {
                Console.WriteLine("The sentence {0} is logical entailed in the knowledge base", args[1]);
            }
            else
            {
                Console.WriteLine("The sentence {0} is not logical entailed in the knowledge base", args[1]);
            }
        }
	}
}
