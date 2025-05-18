namespace Aoc2023
{
    public class Grid
    {
        public Dictionary<(int, int), char> GridMap { get; set; }
        public Dictionary<(int, int), string> StringGridMap { get; set; }
        public int Rows { get; set; }
        public List<int> Cols { get; set; }
        public (int, int)[] Dxdy { get; set; }  // directions

        public Grid((int, int)[] dxdy)
        {
            this.Rows = 0;
            this.Cols = new List<int>();
            this.GridMap = new Dictionary<(int, int), char>();
            this.StringGridMap = new Dictionary<(int, int), string>();
            this.Dxdy = dxdy;
        }

        public void Create(List<string> input)
        {
            this.Rows = input.Count;

            for (int i = 0; i < Rows; i++)
            {
                this.Cols.Add(input[i].Length);

                for (int j = 0; j < Cols[i]; j++)
                {
                    this.GridMap[(i, j)] = input[i][j];
                }
            }
        }

        public List<char> NeighborsOfChar((int, int) loc)
        {
            List<char> neighbors = new List<char>();

            for (int i = 0; i < Dxdy.Length; i++)
            {
                int dx = loc.Item1 + Dxdy[i].Item1;
                int dy = loc.Item2 + Dxdy[i].Item2;

                if (dx < 0 || dy < 0)
                {
                    continue;
                }

                if (dx >= this.Rows || dy >= this.Cols[dx])
                {
                    continue;
                }

                neighbors.Add(this.GridMap[(dx, dy)]);
            }

            return neighbors;
        }

        public List<(int, int)> NeighborsOfCoord((int, int) loc)
        {
            List<(int, int)> neighbors = new List<(int, int)>();

            for (int i = 0; i < Dxdy.Length; i++)
            {
                int dx = loc.Item1 + Dxdy[i].Item1;
                int dy = loc.Item2 + Dxdy[i].Item2;

                if (dx < 0 || dy < 0)
                {
                    continue;
                }

                if (dx >= this.Rows || dy >= this.Cols[dx])
                {
                    continue;
                }

                neighbors.Add((dx, dy));
            }

            return neighbors;
        }

        public (List<(int, int)> Wrapped, List<(int, int)> Unwrapped) InfiniteRepeatingNeighborsOfCoord((int, int) loc, int rows, int cols)
        {
            List<(int, int)> neighbors = new List<(int, int)>();
            List<(int, int)> neighborsUnwrapped = new List<(int, int)>();

            for (int i = 0; i < Dxdy.Length; i++)
            {

                int newY = loc.Item1 + Dxdy[i].Item1;
                int newX = loc.Item2 + Dxdy[i].Item2;

                int wrappedY = ((newY % rows) + rows) % rows;
                int wrappedX = ((newX % cols) + cols) % cols;

                neighbors.Add((wrappedY, wrappedX));
                neighborsUnwrapped.Add((newY, newX));
            }

            return (Wrapped: neighbors, Unwrapped: neighborsUnwrapped);
        }

        public List<(int, int, (int, int))> NeighborsOfCoordWithDirection((int, int, (int, int)) loc)
        {
            List<(int, int, (int, int))> neighbors = new List<(int, int, (int, int))>();

            int dx = loc.Item1 + loc.Item3.Item1;
            int dy = loc.Item2 + loc.Item3.Item2;

            if (dx < 0 || dy < 0 || dx >= this.Rows || dy >= this.Cols[dx])
            {
                return neighbors;
            }

            neighbors.Add((dx, dy, (loc.Item3.Item1, loc.Item3.Item2)));
            return neighbors;
        }

        public IEnumerable<DijkstraGraph.NodeState> NeighborsOfCoordDay17(DijkstraGraph.NodeState loc, int min, int max)
        {
            if (loc.X == 0 && loc.Y == 0 && loc.DX == 0 && loc.DY == 0 && loc.Steps == 0)
            {
                var directions = new[] { (1, 0), (0, 1) };

                foreach (var (dx, dy) in directions)
                {
                    int newX = loc.X + dx;
                    int newY = loc.Y + dy;

                    if (newX < Rows && newY < Cols[newX])
                    {
                        yield return new DijkstraGraph.NodeState(X: newX, Y: newY, DX: dx, DY: dy, Steps: 1);
                    }
                }
                yield break;
            }

            if (loc.Steps < max)
            {
                foreach (var d in new[] { (loc.DX, loc.DY) })
                {
                    int dx = loc.X + d.Item1;
                    int dy = loc.Y + d.Item2;

                    if (dx < 0 || dy < 0)
                        continue;

                    if (dx >= this.Rows || dy >= this.Cols[dx])
                        continue;

                    var newNeighbor = new DijkstraGraph.NodeState(X: dx, Y: dy, DX: loc.DX, DY: loc.DY, Steps: loc.Steps + 1);
                    yield return newNeighbor;
                }
            }
            if (loc.Steps >= min)
            {
                foreach (var d in new[] { (-loc.DY, loc.DX), (loc.DY, -loc.DX) })
                {
                    int dx = loc.X + d.Item1;
                    int dy = loc.Y + d.Item2;

                    if (dx < 0 || dy < 0)
                        continue;

                    if (dx >= this.Rows || dy >= this.Cols[dx])
                        continue;

                    var newNeighbor = new DijkstraGraph.NodeState(X: dx, Y: dy, DX: d.Item1, DY: d.Item2, Steps: 1);
                    yield return newNeighbor;
                }
            }
        }
    }


}