using Aoc2023.Cli;

/// <summary>
/// concrete command for Aoc run
/// </summary>
public class RunDay : ICommand
{
    // Converted fields to properties with getters and setters
    public RunDayReceiver Receiver { get; private set; }
    public string Day { get; set; }
    public string Part { get; set; }
    public string Filepath { get; set; }

    public RunDay(RunDayReceiver receiver, string filepath, string day, string part)
    {
        this.Receiver = receiver;
        this.Day = day;
        this.Part = part;
        this.Filepath = filepath;
    }

    public void Execute()
    {
        this.Receiver.RunDay(this.Filepath, this.Day, this.Part);
    }
}