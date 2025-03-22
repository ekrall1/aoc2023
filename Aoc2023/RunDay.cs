using Aoc2023.Cli;

/// <summary>
/// concrete command for Aoc run
/// </summary>
public class RunDay : ICommand
{
    private RunDayReceiver _receiver;
    private string _day;
    public RunDay(RunDayReceiver receiver, string day) {
        this._receiver = receiver;
        this._day = day;
    }
    public void Execute () {
        this._receiver.RunDay(this._day);
    }
}