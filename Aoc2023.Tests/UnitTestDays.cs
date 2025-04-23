using Xunit;

namespace Aoc2023.Tests;

public class UnitTestDays
{
    [Fact]
    public void Test_Day1_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day1Part1.txt");


        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "1", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 142", output);
    }

    [Fact]
    public void Test_Day1_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day1Part2.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "1", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 281", output);
    }

    [Fact]
    public void Test_Day2_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day2.txt");


        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "2", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 8", output);
    }


    [Fact]
    public void Test_Day2_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day2.txt");


        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "2", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 2286", output);
    }

    [Fact]
    public void Test_Day3_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day3.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "3", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 4361", output);
    }

    [Fact]
    public void Test_Day4_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day4.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "4", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 13", output);
    }

    [Fact]
    public void Test_Day4_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day4.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "4", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 30", output);
    }

    [Fact]
    public void Test_Day5_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day5.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "5", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 35", output);
    }

    [Fact]
    public void Test_Day5_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day5.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "5", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 46", output);
    }

    [Fact]
    public void Test_Day6_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day6.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "6", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 288", output);
    }

    [Fact]
    public void Test_Day6_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day6.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "6", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 71503", output);
    }

    [Fact]
    public void Test_Day7_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day7.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "7", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 6440", output);
    }

    [Fact]
    public void Test_Day7_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day7.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "7", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 5905", output);
    }

    [Fact]
    public void Test_Day8_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day8.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "8", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 6", output);
    }

    [Fact]
    public void Test_Day9_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day9.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "9", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 114", output);
    }

    [Fact]
    public void Test_Day9_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day9.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "9", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 2", output);
    }

    [Fact]
    public void Test_Day10_Part1a()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10a.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 4", output);
    }

    [Fact]
    public void Test_Day10_Part1b()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10b.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 8", output);
    }

    [Fact]
    public void Test_Day10_Part1c()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10c.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 23", output);
    }

    [Fact]
    public void Test_Day10_Part1d()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10d.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 70", output);
    }

    [Fact]
    public void Test_Day10_Part1e()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10e.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 80", output);
    }

    [Fact]
    public void Test_Day10_Part2a()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10a.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 1", output);
    }

    [Fact]
    public void Test_Day10_Part2b()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10b.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 1", output);
    }

    [Fact]
    public void Test_Day10_Part2c()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10c.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 4", output);
    }

    [Fact]
    public void Test_Day10_Part2d()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10d.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 8", output);
    }

    [Fact]
    public void Test_Day10_Part2e()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day10e.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "10", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 10", output);
    }

    [Fact]
    public void Test_Day11_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day11.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "11", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 374", output);
    }

    [Fact]
    public void Test_Day11_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day11.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "11", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 82000210", output);
    }

    [Fact]
    public void Test_Day12_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day12.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "12", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 21", output);
    }

    [Fact]
    public void Test_Day12_Part2()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day12.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "12", "2"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 2 solution is: 525152", output);
    }

    [Fact]
    public void Test_Day13_Part1()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);
        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./TestInput/Day13.txt");

        // Act
        invkr.Setup(new RunDay(rcvr, filePath, "13", "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("part 1 solution is: 405", output);
    }
}