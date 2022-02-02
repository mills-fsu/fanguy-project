namespace Bin.CIS4930
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Utils.PrintBlue("*** CIS4930 Task Manager ***");

            // initialize some basic classes
            var actionManager = new ActionManager();
            var commandParser = new CommandParser(actionManager.UnrecognizedCommand);

            // create all our command mappings
            commandParser.Add("create",   actionManager.Create);
            commandParser.Add("delete",   actionManager.Delete);
            commandParser.Add("edit",     actionManager.Edit);
            commandParser.Add("complete", actionManager.Complete);
            commandParser.Add("list",     actionManager.List);
            commandParser.Add("search",   actionManager.Search);
            commandParser.Add("help",     commandParser.Help);
            commandParser.Add("clear",    Console.Clear);

            // before running the REPL, print the help message
            commandParser.Help();

            // create the REPL and then start running it
            var repl = new REPL("todo$ ", commandParser);
            repl.Run();

            Utils.PrintBlue("*** CIS4930 Task Manager ***");
        }
    }
}