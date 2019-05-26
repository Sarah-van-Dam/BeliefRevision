using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BeliefRevision
{
    public class BeliefRevision
    {
        private List<string> _clauses;
        private string _sentence;

        // --------------------------------------------------------------------------------------------
        public bool ValidateInput(string[] args)
        {
            // Pattern for a list of Horn-clauses
            string pattern = @"^([!]?[a-z]{1})([|][!]?[a-z]{1})+$";

            _sentence = args[1];
            //_sentence = "p|!q";

            // Validate the file and sentence
            //if (args.Length < 2)
            //{
            //    Console.WriteLine("Input too short");
            //    return false;
            //}

            // Read all Horn-clauses
            string[] clauses = File.ReadAllLines(args[0]);
            //string[] clauses = new string[] { "!p | q", "p | !q" };
            _clauses = new List<string>(clauses);
            for (var i = 0; i < _clauses.Count; i++)
                _clauses[i] = _clauses[i].Replace(" ", "");   // remove all blanks

            _sentence = _sentence.Replace(" ", ""); // remove all blanks

            // Validate sentence
            ValidateClause(pattern, _sentence);

            // Validate all rules
            foreach (string line in _clauses)
                ValidateClause(pattern, line);

            return true;
        }

        private bool ValidateClause(string pattern, string line)
        {
            Match result = Regex.Match(line, pattern); // Find out if it has the potential to be a Horn clause
            if (!result.Success)
            {
                Console.WriteLine("The clause {0} is not a Horn clause by regex", line);
                return false;
            }

            var noLiterals = line.Count(x => x == '|') + 1;
            var noNegations = line.Count(x => x == '!');
            //Console.WriteLine("Number of literals {0} and number of negations {1}", noLiterals, noNegations);

            if (!(noNegations == noLiterals | noNegations == noLiterals - 1))
            {
                Console.WriteLine("The clause {0} is not a Horn clause", line);
                return false;
            }

            return true;
        }

        public bool Resolution()
        {
            // Create the set of clauses on which to do resolution.
            List<string> rsClauses = _clauses;

            // Negate the sentence and add the resulting clauses to the resolution clauses.
            string[] rsSentence = _sentence.Split('|');
            for (int i = 0; i < rsSentence.Length; i++)
            {
                if (rsSentence[i].Contains('!'))
                {
                    rsSentence[i] = rsSentence[i].Replace("!", "");
                }
                else
                {
                    rsSentence[i] = "!" + rsSentence[i];
                }
                rsClauses.Add(rsSentence[i]);
            }

            // Perform resolution
            List<string> newClauses = new List<string>();

            do
            {
                for (int i = 0; i < rsClauses.Count; i++)
                {
                    for (int j = i+1; j < rsClauses.Count; j++)
                    {
                        string resolvents = Resolve(rsClauses[i], rsClauses[j]);
                        if (resolvents.Equals(""))
                            return true;
                        newClauses.Add(resolvents);
                    }
                }
                var count = 0;
                foreach (var clause in newClauses)
                {
                    if (rsClauses.Contains(clause))
                        count++;
                }
                if (count == newClauses.Count)
                {
                    return false;
                }
                rsClauses.AddRange(newClauses);
            } while (true);


        }

        private string Resolve(string v1, string v2)
        {
            List<string> symClause1 = new List<string>(v1.Split('|'));
            List<string> symClause2 = new List<string>(v2.Split('|'));
            List<string> literalsToRemove = new List<string>();
            foreach (string literal in symClause1)
            {
                if (symClause2.Contains(GetNegated(literal))) // remove opposite literals
                {
                    literalsToRemove.Add(literal);
                }
            }

            foreach (string literal in literalsToRemove)
            {
                symClause1.Remove(literal);
                symClause2.Remove(GetNegated(literal));
            }

            // Return the literals left
            string newClause = "";
            if(symClause1.Count > 0)
            {
                foreach (string clause in symClause1)
                {
                    if(newClause.Equals(""))
                        newClause += clause;
                    else
                        if(!newClause.Contains(clause))
                            newClause += "|" + clause;
                }
            }
            if (symClause2.Count > 0)
            {
                foreach (string clause in symClause2)
                {
                    if (newClause.Equals(""))
                        newClause += clause;
                    else
                        if (!newClause.Contains(clause))
                        newClause += "|" + clause;
                }
            }
            return newClause;
        }

        private string GetNegated(string v1)
        {
            if (v1.Contains('!'))
            {
                v1 = v1.Replace("!", "");
            }
            else
            {
                v1 = "!" + v1;
            }
            return v1;
        }
    }
}
