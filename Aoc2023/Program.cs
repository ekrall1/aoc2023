// See https://aka.ms/new-console-template for more information
class Program
{
    static void Main(string[] args)
    {
        Invoker invoker = new Invoker();
        RunDayReceiver receiver = new RunDayReceiver();
        invoker.Setup(new RunDay(receiver, args[0], args[1], args[2]));
        invoker.DoAocDay();
    }
}