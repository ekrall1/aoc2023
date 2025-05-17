using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;


public partial class Day20 : Day
{
    public List<string> Input { get; private set; }
    public Dictionary<string, List<string>> System { get; private set; }

    public Day20(string filepath)
    {
        this.Input = new InputReader(filepath).ReadLines();
        this.System = new Dictionary<string, List<string>>();
    }

    enum Pulse { Low, High }

    interface IModule
    {
        string Name { get; }
        List<string> Outputs { get; }
        IEnumerable<(string target, Pulse pulse)> Receive(string sender, Pulse pulse);
    }

    private class Broadcaster : IModule
    {
        public string Name { get; set; }
        public List<string> Outputs { get; set; } = new();
        public IEnumerable<(string target, Pulse pulse)> Receive(string sender, Pulse pulse) =>
            Outputs.Select(output => (output, pulse));
        public Broadcaster(string name) => Name = name;
    }

    private class FlipFlop : IModule
    {
        public string Name { get; set; }
        public List<string> Outputs { get; set; } = new();
        public bool IsOn { get; set; } = false;

        public FlipFlop(string name) => Name = name;
        public IEnumerable<(string target, Pulse pulse)> Receive(string sender, Pulse pulse)
        {
            if (pulse == Pulse.High)
            {
                return Enumerable.Empty<(string target, Pulse pulse)>();
            }

            IsOn = !IsOn;
            var outputPulse = IsOn ? Pulse.High : Pulse.Low;
            return Outputs.Select(output => (output, outputPulse));
        }
    }

    private class Conjunction : IModule
    {
        public string Name { get; set; }
        public List<string> Outputs { get; set; } = new();
        public readonly Dictionary<string, Pulse> lastReceived = new();

        public Conjunction(string name) => Name = name;

        public void AddInput(string inputName)
        {
            if (!lastReceived.ContainsKey(inputName))
                lastReceived[inputName] = Pulse.Low;  // start with low pulse for each input
        }

        public IEnumerable<(string target, Pulse pulse)> Receive(string sender, Pulse pulse)
        {
            lastReceived[sender] = pulse;
            Pulse outPulse = lastReceived.Values.All(p => p == Pulse.High) ? Pulse.Low : Pulse.High;
            return Outputs.Select(output => (output, outPulse));
        }
    }

    private class Propagation
    {
        private readonly Dictionary<string, IModule> modules = new();
        public IEnumerable<IModule> GetModules() => modules.Values;

        public void AddModule(IModule module)
        {
            modules[module.Name] = module;
        }

        public void Connect(string from, string to)
        {
            if (modules.TryGetValue(from, out var fromModule))
            {
                fromModule.Outputs.Add(to);

                if (modules.TryGetValue(to, out var toModule) && toModule is Conjunction conj)
                    conj.AddInput(from);  // if to is a conj, add from to the conj's inputs
            }
        }

        public (long lowCount, long highCount, Dictionary<string, long> shotsDict) Simulate(long buttonPresses, IModule? magicConjunction)
        {
            long lowCount = 0, highCount = 0;
            long i = 0;
            Dictionary<string, long> conjunctionInput = new();

            while (i < buttonPresses)
            {
                i++;
                var queue = new Queue<(string sender, string receiver, Pulse pulse)>();
                queue.Enqueue(("button", "broadcaster", Pulse.Low));

                while (queue.Count > 0)
                {
                    var (sender, receiver, pulse) = queue.Dequeue();

                    if (magicConjunction != null && receiver == magicConjunction.Name && pulse == Pulse.High)
                        if (!conjunctionInput.ContainsKey(sender))
                            conjunctionInput[sender] = i;

                    if (pulse == Pulse.Low) lowCount++;
                    else highCount++;

                    if (!modules.TryGetValue(receiver, out var module))
                        continue;

                    foreach (var (target, outPulse) in module.Receive(sender, pulse))
                    {
                        queue.Enqueue((receiver, target, outPulse));
                    }
                }
            }

            return (lowCount, highCount, conjunctionInput);
        }
    }

    private string Solve(int part)
    {
        var propagation = new Propagation();
        var connections = new List<(string from, string to)>();

        foreach (string line in Input)
        {
            string[] parts = line.Split(" -> ");
            string name = parts[0];
            string sourceName;
            var targets = parts[1].Split(", ", StringSplitOptions.TrimEntries);
            IModule module;

            if (name == "broadcaster")
            {
                sourceName = name;
                module = new Broadcaster(name);
            }
            else if (name.StartsWith('%'))
            {
                sourceName = name[1..];
                module = new FlipFlop(sourceName);
            }
            else
            {
                sourceName = name[1..];
                module = new Conjunction(sourceName);
            }

            propagation.AddModule(module);

            foreach (var target in targets)
            {
                connections.Add((sourceName, target));
            }
        }

        foreach (var (from, to) in connections)
        {
            propagation.Connect(from, to);
        }
        IModule? magicConjunction = null;

        if (part == 2)
        {
            List<IModule> magicConjunctions = propagation.GetModules()
                .Where(module => module.Outputs.Contains("rx"))
                .ToList();
            magicConjunction = magicConjunctions.Count > 0 ? magicConjunctions[0] : null;
        }

        long shots = part == 1 ? 1000L : 10000L;
        (long low, long high, Dictionary<string, long> shotsDict) = propagation.Simulate(shots, magicConjunction);

        if (part == 1)
        {
            return (low * high).ToString();
        }
        else if (part == 2)
        {
            return Factor.Lcm<long>([.. shotsDict.Values]).ToString();
        }
        else
        {
            throw new ArgumentException("Invalid part number");
        }
    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);
}