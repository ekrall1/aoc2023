using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Aoc2023;
using Aoc2023.Days;
using Aoc2023.Input;
using Aoc2023.Utils;

public class Day11 : Day
{
    private List<string> input;
    private readonly Grid grid;

    public Day11(string filepath)
    {
        this.input = new InputReader(filepath).ReadLines();
        this.ExpandInput();
        this.grid = new Grid(dxdy: [(0, 1), (0, -1), (1, 0), (-1, 0)]);
        this.grid.Create(input);
    }

    private void ExpandInput()
    {
        List<string> newInput = [];

        // rows
        for (int r = 0; r < this.input.Count; r++)
        {
            newInput.Add(this.input[r]);
            if (this.input[r].ToHashSet<char>().Count == 1) {
                newInput.Add(this.input[r]);
            }
        }

        // cols
        int addedCols = 0;
        for (int c = 0; c < this.input[0].Count(); c++)
        {
            string column = "";
            for (int r = 0; r < this.input.Count; r++)
            {
                column += this.input[r][c];
            }
            if (column.ToHashSet<char>().Count == 1)
            {
                for (int r = 0; r < newInput.Count; r++)
                {
                    var tmpRemaining = newInput[r][(c + addedCols + 1)..];
                    newInput[r] = newInput[r][0..(c + addedCols + 1)];
                    newInput[r] += '.';
                    newInput[r] += tmpRemaining;
                }
                addedCols++;
            }
        }

        // convert galaxies to numbers
        int ctr = 1;
        List<char[]> mutNewInput = newInput.Select(row => row.ToCharArray()).ToList();

        for (int r = 0; r < mutNewInput.Count; r++)
        {
            for (int c = 0; c < mutNewInput[r].Count(); c++)
            {
                if (mutNewInput[r][c] == '#')
                {
                    mutNewInput[r][c] = (char)(ctr + '0');
                    ctr++;
                }
            }
        }

        this.input = mutNewInput.Select(row => new String(row)).ToList();
    }

    private string Solve(int part)
    {
        for (int r = 0; r < this.input.Count(); r++)
        {
            Console.WriteLine(this.input[r]);
        }
        return "";

    }

    string Day.Part1() => Solve(1);
    string Day.Part2() => Solve(2);

}