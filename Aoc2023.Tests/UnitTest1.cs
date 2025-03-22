using System;
using System.IO;
using Xunit;
using Aoc2023;

namespace Aoc2023.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_Command()
    {
        // Arrange
        var writer = new StringWriter();
        Console.SetOut(writer);

        var invkr = new Invoker();
        var rcvr = new RunDayReceiver();

        // Act
        invkr.Setup(new RunDay(rcvr, "1"));
        invkr.DoAocDay();

        // Assert
        var output = writer.ToString().Trim();
        Assert.Equal("Run Day 1", output);
    }
}