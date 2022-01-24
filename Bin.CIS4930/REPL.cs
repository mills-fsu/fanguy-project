using Library.Assignment1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Assignment1.TaskManager;

namespace CIS4930_Assignment1
{
    class REPL
    {
        private string _prompt;
        private CommandParser _parser;

        /// <summary>
        /// Create a new Read-Evaluate-Print-Loop with the given prompt and parser
        /// </summary>
        /// <param name="prompt">what to prompt the user with on every iteration</param>
        /// <param name="parser">an already-initialized parser to match user input</param>
        public REPL(string prompt, CommandParser parser)
        {
            _prompt = prompt;
            _parser = parser;
        }

        /// <summary>
        /// Begin the REPL, printing the prompt, getting user input, and then matching
        /// the given command using the CommandParser
        /// </summary>
        public void Run()
        {
            bool shouldContinue = true;
            while (shouldContinue)
            {
                string action = Utils.GetNonEmptyStringInput(_prompt);
                shouldContinue = _parser.Match(action);
            }
        }
    }
}
