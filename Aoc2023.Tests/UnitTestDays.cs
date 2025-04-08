using System;
using System.IO;
using Xunit;
using Aoc2023;

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
    public void Test_Day3_Part1() {
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
    public void Test_Day4_Part1() {
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
}