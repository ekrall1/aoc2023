using Aoc2023.Cli;

public class Invoker
{
    // Added a property with a getter and private setter for _cmd
    public ICommand? Command
    {
        get => _cmd;
        private set => _cmd = value;
    }

    private ICommand? _cmd;

    public void Setup(ICommand command)
    {
        this.Command = command;
    }

    public void DoAocDay()
    {
        if (this.Command is ICommand)
        {
            this.Command.Execute();
        }
    }
}