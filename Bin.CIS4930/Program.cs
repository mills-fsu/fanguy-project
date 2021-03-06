using CIS4930_Assignment1;
using Library.Assignment1;
using System;

namespace Assignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** CIS4930 To Do Manager ***");

            // initialize some basic classes
            var actionManager = new ActionManager();
            var commandParser = new CommandParser(actionManager.UnrecognizedCommand);

            // create all our command mappings
            commandParser.Add("create",   actionManager.Create);
            commandParser.Add("delete",   actionManager.Delete);
            commandParser.Add("edit",     actionManager.Edit);
            commandParser.Add("complete", actionManager.Complete);
            commandParser.Add("list",     actionManager.List);
            commandParser.Add("help",     actionManager.PrintHelp);

            // before running the REPL, print the help message
            actionManager.PrintHelp();

            // create the REPL and then start running it
            var repl = new REPL("todo$ ", commandParser);
            repl.Run();

            Console.WriteLine("*** Thanks for using the CIS4930 To Do Manager ***");
        }
    }
}