using Aoc2023.Cli;

public class Invoker {
    private ICommand? _cmd;

    public void Setup(ICommand command) {
        this._cmd = command;
    }

    public void DoAocDay() {
        if (this._cmd is ICommand) {
            this._cmd.Execute();
        }
    }
}