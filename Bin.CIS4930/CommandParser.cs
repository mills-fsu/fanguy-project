using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS4930_Assignment1
{
    class CommandParser
    {
        /// <summary>
        /// The delegate type for every single possible REPL command's action
        /// </summary>
        public delegate void ActionDelegate();

        // matches the command string to an action to be invoked
        private Dictionary<string, ActionDelegate> _actions;
        // the action to take when no command can be matched
        private ActionDelegate _fallbackAction;

        /// <summary>
        /// Create a new CommandParser with the given fallback action
        /// </summary>
        /// <param name="fallbackAction">an action to invoke when input matches no known command</param>
        public CommandParser(ActionDelegate fallbackAction)
        {
            _actions = new Dictionary<string, ActionDelegate>();
            _fallbackAction = fallbackAction;
        }

        /// <summary>
        /// Add a new command name, action pair
        /// </summary>
        /// <param name="actionName">the exact text of the command to match</param>
        /// <param name="action">the action to be invoked when this text is matched</param>
        public void Add(string actionName, ActionDelegate action)
        {
            _actions.Add(actionName, action);
        }

        public void Help()
        {
            Console.WriteLine();
            Console.Write("commands: ");

            var commands = _actions.ToList().Select(p => p.Key).ToList();
            var commandStr = string.Join(", ", commands);

            Console.Write(commandStr);
            Console.WriteLine();
        }

        /// <summary>
        /// Try to match the given command text and invoke its action, calling
        /// the fallback action if necessary.
        /// </summary>
        /// <param name="command">the user-entered text to match against</param>
        /// <returns>true if the program should continue, or false if the program should end</returns>
        public bool Match(string command)
        { 
            // special case: when quit or exit is entered, return false to 
            // trigger an end to the program
            if (command == "quit" || command == "exit")
            {
                return false;
            }

            // if we can find the command in the Dictionary, invoke its associated action
            if (_actions.ContainsKey(command))
            {
                Console.Clear();
                _actions[command].Invoke();
                Console.WriteLine();
            }
            // otherwise, invoke the fallback action
            else
            {
                _fallbackAction();
            }

            // if we reached here, we want to continue running, so return true
            return true;
        }
    }
}
