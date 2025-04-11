using Aoc2023.Cli;

/// <summary>
/// concrete command for Aoc run
/// </summary>
public class RunDay : ICommand
{
    private RunDayReceiver _receiver;
    private string _day;
    private string _part;
    private string _filepath;
    public RunDay(RunDayReceiver receiver, string filepath, string day, string part)
    {
        this._receiver = receiver;
        this._day = day;
        this._part = part;
        this._filepath = filepath;
    }
    public void Execute()
    {
        this._receiver.RunDay(this._filepath, this._day, this._part);
    }
}